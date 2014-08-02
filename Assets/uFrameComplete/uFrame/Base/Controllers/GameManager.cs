using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Debug = UnityEngine.Debug;

/// <summary>
/// A singleton that manages our current Scene Manager and all the games types in the scene.
/// This component will persist through every scene
/// </summary>
public class GameManager : MonoBehaviour
{
    public Color _AmbientLight = new Color(0.2f, 0.2f, 0.2f, 1.0f);

    public float _FlareStrength = 1.0f;

    public bool _Fog;

    public Color _FogColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    public float _FogDensity = 0.01f;

    public FogMode _FogMode = FogMode.ExponentialSquared;

    public float _HaloStrength = 0.5f;

    public float _LinearFogEnd = 300.0f;

    public float _LinearFogStart = 0.0f;

    /// <summary>
    /// A level that displays a progress bar and message
    /// </summary>
    public string _LoadingLevel;

    public Material _SkyboxMaterial;

    /// <summary>
    /// Set this to the game that will load when the game starts
    /// </summary>
    public SceneManager _Start;

    public string _StartupScene;

    [HideInInspector]
    public string _ViewModelScriptsPath = "@ElementPath/";

    [HideInInspector]
    public string _ViewPrefabsPath = "@ElementPath/Resources/";

    [HideInInspector]
    public string _ViewsScriptsPath = "@ElementPath/";

    private static IGameContainer _container;

    private static LevelLoadViewModel _loadingViewModel;

    private List<SceneManager> _sceneManagers = new List<SceneManager>();

    /// <summary>
    /// The current running game
    /// </summary>
    public static SceneManager ActiveSceneManager
    {
        get { return _activeSceneManager ?? Instance._Start; }
        private set { _activeSceneManager = value; }
    }

    public static IGameContainer Container
    {
        get
        {
            if (_container == null)
            {
                _container = new GameContainer();
                _container.RegisterInstance(Progress);
            }
            return _container;
        }
    }

    private static LevelLoadViewModel _progress;
    private static SceneManager _activeSceneManager;

    public static LevelLoadViewModel Progress
    {
        get
        {
            return _progress ?? (_progress = new LevelLoadViewModel());
        }
    }

    /// <summary>
    /// The current instance of GameManager
    /// </summary>
    public static GameManager Instance { get; set; }

    /// <summary>
    /// The view model that is used for loading a scene.  Bind to this to be notified of progress changes
    /// </summary>
    /// <value>The loading view model.</value>
    public static LevelLoadViewModel LoadingViewModel
    {
        get { return _loadingViewModel ?? (_loadingViewModel = new LevelLoadViewModel()); }
        set { _loadingViewModel = value; }
    }

    public static ISwitchLevelSettings SwitchLevelSettings { get; set; }
    public static void ProgressUpdated(string message, float progress)
    {
        Progress.Status = message;
        Progress.Progress = progress;
    }
    public Type ContainerType
    {
        get;
        set;
    }

    /// <summary>
    /// A list of all the game in the scene.
    /// Each game registers itself with this manager and is added to this list.
    /// </summary>
    public List<SceneManager> SceneManagers
    {
        get { return _sceneManagers; }
        set { _sceneManagers = value; }
    }

    public static Coroutine Transition<T>(Action<T> setup, UpdateProgressDelegate progress = null) where T : SceneManager
    {

        return Transition(Instance.SceneManagers.OfType<T>().First());
    }

    /// <summary>
    /// This switches the game from one to the other invoking a sequence of actions
    /// SwitchGame
    ///     - Invoke the current controllers Unload() method.
    ///     - Set the CurrentController Property to the new game
    ///     - New Controller Load() method is invoked via StartCoroutine
    ///     - New Controller OnLoading() method is invoked
    ///     - After the Load() Coroutine method is complete it will invoke the ActiveGame Game's OnLoaded() method
    /// </summary>
    /// <typeparam name="TGame">The Scene Manager</typeparam>
    /// <param name="progress"></param>
    /// <param name="setup"></param>
    /// <param name="controller"></param>
    /// <returns></returns>
    public static Coroutine Transition<TGame>(TGame controller, Action<TGame> setup = null, UpdateProgressDelegate progress = null) where TGame : SceneManager
    {
        if (controller == null)
        {
            throw new Exception(String.Format("{0} is null or was not found while trying to load the game. Have you loaded the correct scene? Try", typeof(TGame).Name));
        }

        if (ActiveSceneManager != null)
        {
            ActiveSceneManager.Unload();
            ActiveSceneManager.enabled = false;
            ActiveSceneManager.gameObject.SetActive(false);
        }

        ActiveSceneManager = controller;
        ActiveSceneManager.gameObject.SetActive(true);
        ActiveSceneManager.enabled = true;

        if (setup != null)
        {
            setup(controller);
        }
        ActiveSceneManager.OnLoading();
        return Instance.StartCoroutine(LoadSceneManager(progress ?? DefaultUpdateProgress));
    }

    /// <summary>
    /// Transitions to another scene and loads additional scene if specified.
    /// game assuming that it will exist in the scene after loading is finished.
    /// </summary>
    /// <typeparam name="T">The SceneManager type that will exist in the first scene specified.</typeparam>
    public static void TransitionLevel<T>(SwitchLevelSettings<T> settings) where T : SceneManager
    {
        if (ActiveSceneManager != null)
        {
            ActiveSceneManager.Unload();
            ActiveSceneManager.enabled = false;
            ActiveSceneManager.gameObject.SetActive(false);
            ActiveSceneManager = null;
        }

        SwitchLevelSettings = settings;

        Application.LoadLevel(Instance._LoadingLevel);
    }

    /// <summary>
    /// Transitions to another scene and loads additional scene if specified.
    /// game assuming that it will exist in the scene after loading is finished.
    /// </summary>
    /// <typeparam name="T">The SceneManager type that will exist in the first scene specified.</typeparam>
    /// <param name="setup">Perform additonal setup when the scene has transitioned.</param>
    /// <param name="levels">The SceneManager type that will exist in the first scene specified.</param>
    public static void TransitionLevel<T>(Action<T> setup, params string[] levels) where T : SceneManager
    {
        if (levels.Length < 1)
        {
            throw new Exception("There must be at least one level name passed to the SwitchGameAndLevel method.");
        }
        var settings = new SwitchLevelSettings<T>()
        {
            Setup = setup,
            Levels = levels
        };
        TransitionLevel(settings);
    }

    /// <summary>
    /// Registers a SceneManger with this game manager.  This will invoke setup on the manager as well as disable it.
    /// </summary>
    /// <param name="sceneManager">The scene manager to register.</param>
    public virtual void RegisterSceneManager(SceneManager sceneManager)
    {
        if (SceneManagers.Contains(sceneManager)) return;
        sceneManager.Container = Container;
        sceneManager.Setup();
        SceneManagers.Add(sceneManager);
        sceneManager.enabled = false;
        sceneManager.gameObject.SetActive(false);
    }

    /// <summary>
    /// Applies the render settings specified in the inspector.
    /// </summary>
    public void ApplyRenderSettings()
    {
        RenderSettings.fog = _Fog;
        RenderSettings.fogColor = _FogColor;
        RenderSettings.fogMode = _FogMode;
        RenderSettings.fogDensity = _FogDensity;

        RenderSettings.fogStartDistance = _LinearFogStart;
        RenderSettings.fogEndDistance = _LinearFogEnd;

        RenderSettings.ambientLight = _AmbientLight;
        RenderSettings.skybox = _SkyboxMaterial;

        RenderSettings.haloStrength = _HaloStrength;

        RenderSettings.flareStrength = _FlareStrength;
    }

    public virtual void OnEnable()
    {

    }

    /// <summary>
    /// On awake will apply the render settings and will begin startup which will "boot" the scenemanager.
    /// </summary>
    public void Awake()
    {
        ApplyRenderSettings();

        if (Instance != null)
        {
            // If the instace already exist destroy this
            Instance._Start = _Start;
            Instance.Startup();

        }
        else
        {
            Instance = this;
            // Don't destory this through scene switching
            DontDestroyOnLoad(gameObject);
            Startup();

            //_Start.Container;
        }
    }
    /// <summary>
    /// Checks if the gamemanager has already been loaded.  If so it will copy necessary info and destroy
    /// itself.  This also calls "Transition" in order to load the "Start" scene manager of the scene.
    /// </summary>
    public void Start()
    {
        if (Instance != null && Instance != this)
        {
            Instance._Start = this._Start;
            //SwitchGame(_Start);
            DestroyImmediate(gameObject);
        }
        else
        {
            if (_Start != null)
                Transition(_Start);
        }
    }

    /// <summary>
    /// Startup will register every scenemanager in the scene. As well as set the "ActiveSceneManager' to the specified
    /// 'Start' scene manager specified in the inspector.
    /// </summary>
    public virtual void Startup()
    {
        SceneManagers.Clear();
        var games = FindObjectsOfType<SceneManager>();
        foreach (var game in games)
        {
            RegisterSceneManager(game);
        }
        if (_Start == null)
        {

            Debug.LogError(
                "You need to set the scene manager option on the game manager for this scene. Double-Click to navigate there.",this.gameObject);
        }
        ActiveSceneManager = _Start;
    }

    /// <summary>
    /// Loads the current render settings of a scene.
    /// </summary>
    public void LoadRenderSettings()
    {
        _Fog = RenderSettings.fog;
        _FogColor = RenderSettings.fogColor;
        _FogMode = RenderSettings.fogMode;
        _FogDensity = RenderSettings.fogDensity;

        _LinearFogStart = RenderSettings.fogStartDistance;
        _LinearFogEnd = RenderSettings.fogEndDistance;

        _AmbientLight = RenderSettings.ambientLight;
        _SkyboxMaterial = RenderSettings.skybox;

        _HaloStrength = RenderSettings.haloStrength;

        _FlareStrength = RenderSettings.flareStrength;
    }

    
    /// <summary>
    /// When this is destroyed check if we are the "current instance" and set "Instance" to null.
    /// Note: This should really never happen.  But in some test cases is necessary.
    /// </summary>
    public void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    /// <summary>
    /// Removes the Scene Manager from this manager.  This will only happen if a Game is destroyed
    /// </summary>
    /// <param name="sceneManager"></param>
    public virtual void UnRegisterSceneManager(SceneManager sceneManager)
    {
        SceneManagers.Remove(sceneManager);
    }


    protected static void DefaultUpdateProgress(string message, float progress)
    {
        Debug.Log(String.Format("Loading: {0}% - {1}", progress * 100f, message));
    }

    /// <summary>
    /// Begins the the "Load" coroutine of the scenemanger passing the appropriate progress delegate.
    /// </summary>
    /// <param name="progress"></param>
    /// <returns></returns>
    private static IEnumerator LoadSceneManager(UpdateProgressDelegate progress)
    {
        // yield return new WaitForEndOfFrame();
        yield return Instance.StartCoroutine(ActiveSceneManager.Load(progress));
        ActiveSceneManager.OnLoaded();
    }

    /// <summary>
    /// Is this a pro license?
    /// </summary>
    public static bool IsPro
    {
        get
        {
            return Application.HasProLicense();
        }
    }
    /// <summary>
    /// Do not use async loading on "TransitionLevel"
    /// </summary>
    public bool _DontUseAsyncLoading = false;

    /// <summary>
    /// The uFrame Boot loader that willbegin the startup process
    /// </summary>
    /// <returns></returns>
    public static IEnumerator Load()
    {

        // Wait till all other things have processed in the scene before loading.
        yield return new WaitForEndOfFrame();
        // Grab the game manager instance transform so we can ignore it
        var gameTransform = Instance.transform;
        // Grab the root objects in the scene to destroy them later
        var list = new List<Transform>();
        // The loading screen should be small
        foreach (Transform p in FindObjectsOfType(typeof(Transform)))
        {
            if (p == gameTransform)
                continue;

            if (p.parent == null)
                list.Add(p);
        }
        // For progress tracking
        var progressValue = 0f;
        if (SceneManager.Settings.Levels != null)
        {
            var numberOfLevelsToLoad = (SceneManager.Settings.Levels == null ? 1 : SceneManager.Settings.Levels.Length) + 1;
            var progressFactor = numberOfLevelsToLoad;
            var progressBase = 0f;
            for (int index = 0; index < SceneManager.Settings.Levels.Length; index++)
            {
                var level = SceneManager.Settings.Levels[index];
                if (IsPro && Instance != null && !Instance._DontUseAsyncLoading)
                {
                    AsyncOperation asyncOperation;
                    asyncOperation = Application.LoadLevelAdditiveAsync(level);
                    while (!asyncOperation.isDone)
                    {
                        progressValue = (asyncOperation.progress + progressBase) / progressFactor;
                        ProgressUpdated(String.Format("Loading '{0}'", level), progressValue);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                else
                {
                    ProgressUpdated(String.Format("Loading '{0}'", level), numberOfLevelsToLoad);
                    Application.LoadLevelAdditive(level);
                }

                progressBase += 1f;
            }
        }

        Action<string, float> progressUpdateWithFactor =
            delegate(string s, float f)
            {
                progressValue += (1f - progressValue) * f;
                ProgressUpdated(s, progressValue);
            };

        var sceneManager = Instance.SceneManagers.Find(p => p.GetType() == SceneManager.Settings.StartManagerType);
        while (sceneManager == null)
        {
            yield return new WaitForSeconds(0.1f);
            sceneManager = Instance.SceneManagers.Find(p => p.GetType() == SceneManager.Settings.StartManagerType);
        }
        yield return Transition(sceneManager, SceneManager.Settings.InvokeControllerSetup, new UpdateProgressDelegate(progressUpdateWithFactor));
        foreach (var t in list)
        {
            Destroy(t.gameObject);
        }
        ProgressUpdated("Complete", 1f);
    }
}