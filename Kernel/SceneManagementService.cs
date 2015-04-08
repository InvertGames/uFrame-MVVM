using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class SceneManagementService : SystemServiceMonoBehavior
{

    private List<IEnumerator> _scenesQueue;
    public List<IEnumerator> ScenesQueue
    {
        get { return _scenesQueue ?? (_scenesQueue = new List<IEnumerator>()); }
    }
    public List<IScene> LoadedScenes
    {
        get { return _loadedScenes ?? (_loadedScenes = new List<IScene>()); }
    }
    private List<IScene> _loadedScenes;
    public override void Setup()
    {
        base.Setup();
        this.OnEvent<LoadSceneCommand>().Subscribe(_ =>
        {
            this.LoadScene(_.SceneName, _.Settings);
        });

        this.OnEvent<UnloadSceneCommand>().Subscribe(_ =>
        {
            this.UnloadScene(_.SceneName);
        });
        this.OnEvent<SystemsLoadedEvent>().Subscribe(_ =>
        {
            var attachedSceneLoaders = _.Kernel.GetComponentsInChildren(typeof(ISceneLoader)).OfType<ISceneLoader>();
            foreach (var sceneLoader in attachedSceneLoaders)
            {
                uFrameMVVMKernel.Container.RegisterSceneLoader(sceneLoader);
                uFrameMVVMKernel.Container.Inject(sceneLoader);
                SceneLoaders.Add(sceneLoader);
            }
            _defaultSceneLoader = gameObject.GetComponent<DefaultSceneLoader>() ??
                                  gameObject.AddComponent<DefaultSceneLoader>();

        });

        this.OnEvent<SceneAwakeEvent>().Subscribe(_ => StartCoroutine(SetupScene(_.Scene)));

    }
    private List<ISceneLoader> _sceneLoaders;
    public List<ISceneLoader> SceneLoaders
    {
        get
        {
            return _sceneLoaders ?? (_sceneLoaders = new List<ISceneLoader>());
        }
    }  
    
    private DefaultSceneLoader _defaultSceneLoader;


    public IEnumerator LoadSceneInternal(string sceneName, ISceneSettings settings)
    {
        yield return StartCoroutine(uFrameMVVMKernel.InstantiateSceneAsyncAdditively(sceneName));
        var sceneRoot = FindObjectsOfType<Scene>()
            .FirstOrDefault(scene => string.IsNullOrEmpty(scene.Name));

        if (sceneRoot == null) throw new Exception(string.Format("No IScene root is defined for {0} scene", sceneName));
        else
        {
            sceneRoot.Name = sceneName;
            sceneRoot._SettingsObject = settings;
            this.Publish(new SceneLoaderEvent()
            {
                State = SceneState.Instantiated,
                SceneRoot = sceneRoot
            });
        }
    }

    public void QueueSceneLoad(string sceneName, ISceneSettings settings)
    {
        ScenesQueue.Add(LoadSceneInternal(sceneName, settings));
    }

    public void QueueScenesLoad(params string[] sceneNames)
    {
        foreach (var sceneName in sceneNames)
        {
            ScenesQueue.Add(LoadSceneInternal(sceneName, null));//TODO: Decide what to do when loading shit loads of levels with settings
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
        while (string.IsNullOrEmpty(sceneRoot.Name)) yield return null;
        this.Publish(new SceneLoaderEvent()
        {
            State = SceneState.Instantiated,
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

        var sceneLoader = SceneLoaders.FirstOrDefault(loader => loader.SceneType == sceneRoot.GetType()) ?? _defaultSceneLoader;

        yield return StartCoroutine(sceneLoader.Load(sceneRoot, updateDelegate));

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
        if (sceneRoot != null) yield return StartCoroutine(this.UnloadSceneAsync(sceneRoot));
        else yield break;
    }

    protected IEnumerator UnloadSceneAsync(IScene sceneRoot)
    {

        var sceneLoader = SceneLoaders.FirstOrDefault(loader => loader.SceneType == sceneRoot.GetType()) ?? _defaultSceneLoader;

        Action<float, string> updateDelegate = (v, m) =>
        {
            this.Publish(new SceneLoaderEvent()
            {
                State = SceneState.Unloading,
                Progress = v,
                ProgressMessage = m
            });
        };

        yield return StartCoroutine(sceneLoader.Unload(sceneRoot, updateDelegate));

        this.Publish(new SceneLoaderEvent() { State = SceneState.Unloaded, SceneRoot = sceneRoot });

        LoadedScenes.Remove(sceneRoot);
        Destroy((sceneRoot as MonoBehaviour).gameObject);

        this.Publish(new SceneLoaderEvent() { State = SceneState.Destructed, SceneRoot = sceneRoot });


    }

    public void UnloadScene(string name)
    {
        StartCoroutine(UnloadSceneAsync(name));
    }

    public void UnloadScene(IScene sceneRoot)
    {
        StartCoroutine(UnloadSceneAsync(sceneRoot));
    }

    public void UnloadScenes(string[] names)
    {
        foreach (var name in names)
        {
            StartCoroutine(UnloadSceneAsync(name));
        }
    }

    public void UnloadScenes(IScene[] sceneRoots)
    {
        foreach (var sceneRoot in sceneRoots)
        {
            StartCoroutine(UnloadSceneAsync(sceneRoot));
        }
    }

    public void LoadScene(string name, ISceneSettings settings)
    {
        this.QueueSceneLoad(name, settings);
        this.ExecuteLoad();
    }
    public void LoadScenes(params string[] sceneNames)
    {
        this.QueueScenesLoad(sceneNames);
        this.ExecuteLoad();
    }
}