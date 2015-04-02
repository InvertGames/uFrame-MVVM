using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx;

public class uFrameKernel : MonoBehaviour {
    private static GameContainer _container;
    private static IEventAggregator _eventAggregator;
    private static IViewResolver _viewResolver;
    private static bool _isKernelLoaded;
    private List<IScene> _loadedScenes;
    private List<ISceneLoader> _sceneLoaders;
    private List<ISystemService> _services;
    private List<IEnumerator> _scenesQueue;


    public bool _enableKernelEventsLogging = false;

    void Awake()
    {
        Debug.Log("Kernel is about to load");

        if (Instance != null)
        {
            throw new Exception("Loading Kernel twice is not a good practice!");
        }
        else
        {
            Instance = this;
            if (this.gameObject.GetComponent<MainThreadDispatcher>() == null)
                this.gameObject.AddComponent<MainThreadDispatcher>();
            DontDestroyOnLoad(gameObject);
            StartCoroutine(Startup());
        }
    }

    public static bool IsKernelLoaded
    {
        get { return _isKernelLoaded; }
        set { _isKernelLoaded = value; }
    }

    public static IGameContainer Container
    {
        get
        {
            if (_container == null)
            {
                _container = new GameContainer();
                _container.RegisterInstance<IEventAggregator>(EventAggregator);
                _container.RegisterInstance<IViewResolver>(new ViewResolver());
            }
            return _container;
        }
    }

    public static IEventAggregator EventAggregator
    {
        get { return _eventAggregator ?? (_eventAggregator = new EventAggregator()); }
        set { _eventAggregator = value; }
    }

    public static IViewResolver ViewResolver
    {
        get { return _viewResolver ?? (_viewResolver = Container.Resolve<IViewResolver>()); }
        set { _viewResolver = value; }
    }

    public List<IScene> LoadedScenes
    {
        get { return _loadedScenes ?? (_loadedScenes = new List<IScene>()); }
    }
    public List<ISceneLoader> SceneLoaders
    {
        get
        {
            return _sceneLoaders ?? (_sceneLoaders = new List<ISceneLoader>());
        }
    }
    public List<ISystemService> Services
    {
        get
        {
            return _services ?? (_services = new List<ISystemService>());
        }
    }

    public static IEnumerator LoadSceneAsyncAdditively(string sceneName)
    {
        var time = System.DateTime.Now;
        var asyncOperation = Application.LoadLevelAdditiveAsync(sceneName);
        
        while (!asyncOperation.isDone)
        {
            yield return new WaitForSeconds(0.1f);
        }
        var timeSpan = System.DateTime.Now - time;
        Debug.Log(string.Format("Loaded scene {0} in {1}ms",sceneName,timeSpan.TotalMilliseconds));


    }

    public List<IEnumerator> ScenesQueue
    {
        get { return _scenesQueue ?? (_scenesQueue = new List<IEnumerator>()); }
    }

    public IEnumerator LoadSceneInternal(string sceneName)
    {
        yield return StartCoroutine(LoadSceneAsyncAdditively(sceneName));
        var sceneRoot = FindObjectsOfType<Scene>()
                        .FirstOrDefault(scene => string.IsNullOrEmpty(scene.Name));

        if (sceneRoot == null) throw new Exception(string.Format("No IScene root is defined for {0} scene", sceneName));
        else
        {
            sceneRoot.Name = sceneName;
        }
    }

    public void QueueSceneLoad(string sceneName)
    {
        ScenesQueue.Add(LoadSceneInternal(sceneName));
    }

    public void QueueScenesLoad(params string[] sceneNames)
    {
        foreach (var sceneName in sceneNames)
        {
            ScenesQueue.Add(LoadSceneInternal(sceneName));
        }
    }

    protected IEnumerator ExecuteLoadAsync()
    {
        foreach (var sceneLoader in ScenesQueue)
        {
            yield return StartCoroutine(sceneLoader);
        }
        ScenesQueue.Clear();
    }

    public void ExecuteLoad()
    {
        StartCoroutine(ExecuteLoadAsync());
    }

    public IEnumerator SetupScene(IScene sceneRoot)
    {

        this.Publish(new SceneLoaderEvent()
        {
            State = SceneState.Loading,
            SceneRoot = sceneRoot
        });

        Action<float, string> updateDelegate = (v, m) =>
        {
            this.Publish(new SceneLoaderEvent()
            {
                State = SceneState.Update,
                Progress = v,
                ProgressMessage = m
            });
        };

        var sceneLoader = SceneLoaders.FirstOrDefault(loader => loader.SceneType == sceneRoot.GetType());

        if (sceneLoader == null) throw new Exception("No scene loader is defined for scene of type " + sceneRoot.GetType());

        yield return StartCoroutine(sceneLoader.Load(sceneRoot,updateDelegate));

        LoadedScenes.Add(sceneRoot);

        this.Publish(new SceneLoaderEvent()
        {
            State = SceneState.Loaded,
            SceneRoot = sceneRoot
        });


    }

    protected IEnumerator UnloadSceneAsync(string name)
    {
        var sceneRoot = LoadedScenes.FirstOrDefault(s => s.Name == name);
        if(sceneRoot!=null) yield return StartCoroutine(this.UnloadSceneAsync(sceneRoot));
        else yield break;
    }

    protected IEnumerator UnloadSceneAsync(IScene sceneRoot)
    {
        var sceneLoader = SceneLoaders.FirstOrDefault(loader => loader.SceneType == sceneRoot.GetType());
        if (sceneLoader == null) throw new Exception("No scene loader is defined for scene of type " + sceneRoot.GetType());
        Action<float, string> updateDelegate = (v, m) =>
        {
            this.Publish(new SceneLoaderEvent()
            {
                State = SceneState.Unloading,
                Progress = v,
                ProgressMessage = m
            });
        };

        yield return StartCoroutine(sceneLoader.Unload(sceneRoot,updateDelegate));
        LoadedScenes.Remove(sceneRoot);
        GameObject.Destroy((sceneRoot as MonoBehaviour).gameObject);
    }

    public void UnloadScene(string name)
    {
        StartCoroutine(UnloadSceneAsync(name));
    }

    public void UnloadScene(IScene sceneRoot)
    {
        StartCoroutine(UnloadSceneAsync(sceneRoot));
    }

    private IEnumerator Startup()
    {

        if (_enableKernelEventsLogging)
        {
            EventAggregator.GetEvent<SceneLoaderEvent>().Subscribe(_ =>
            {
                Debug.Log(string.Format("[Scene Loader]: {0} is {1}", _.SceneRoot.GetType(), _.State));
            });
        }


        var attachedServices = gameObject.GetComponentsInChildren(typeof(SystemServiceMonoBehavior)).OfType<SystemServiceMonoBehavior>();
        foreach (var service in attachedServices)
        {
            Container.RegisterService(service);
            service.Setup();
            Debug.Log("Starting to load: "+service.GetType());        
            yield return StartCoroutine(service.SetupAsync());
            Services.Add(service); //TODO: is that really needed??
        }


        var attachedSystemLoaders = gameObject.GetComponentsInChildren(typeof (ISystemLoader)).OfType<ISystemLoader>();
        foreach (var systemLoader in attachedSystemLoaders)
        {
            systemLoader.Container = Container;
            systemLoader.EventAggregator = EventAggregator;
            systemLoader.Load();
        }
        
        var attachedSceneLoaders = gameObject.GetComponentsInChildren(typeof(ISceneLoader)).OfType<ISceneLoader>();
        foreach (var sceneLoader in attachedSceneLoaders)
        {
            Container.RegisterSceneLoader(sceneLoader);
            Container.Inject(sceneLoader);
            SceneLoaders.Add(sceneLoader);
        }

        Container.InjectAll();

//
        foreach (var controller in Container.ResolveAll<Controller>())
        {
            Debug.Log(string.Format("Setting up for {0}", controller.GetType()));
            controller.Setup();
        }

        _isKernelLoaded = true;
    
        
    
    }

    public static uFrameKernel Instance { get; set; }

}

public class SceneLoaderEvent
{
    public SceneState State { get; set; }
    public IScene SceneRoot { get; set; }
    public float Progress { get; set; }
    public string ProgressMessage { get; set; }
}

public enum SceneState
{
    Loading,
    Update,
    Loaded,
    Unloading,
    Unloaded
}


public static class uFrameKernelExtensions
{
    public static void Publish(this uFrameKernel kernel, object evt)
    {
        uFrameKernel.EventAggregator.Publish(evt);
    }
}

