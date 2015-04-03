using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx;

public class uFrameMVVMKernel : MonoBehaviour {

    private static GameContainer _container;
    private static IEventAggregator _eventAggregator;
    private static IViewResolver _viewResolver;
    private static bool _isKernelLoaded;
    private List<IScene> _loadedScenes;
    private List<ISceneLoader> _sceneLoaders;
    private List<ISystemService> _services;
    private List<ISystemLoader> _systemLoaders;
    private List<IEnumerator> _scenesQueue;
    private DefaultSceneLoader _defaultSceneLoader;
    public static bool IsKernelLoaded
    {
        get { return _isKernelLoaded; }
        set { _isKernelLoaded = value; }
    }

    public static uFrameMVVMKernel Instance { get; set; }

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
    
    public List<ISystemLoader> SystemLoaders
    {
        get
        {
            return _systemLoaders ?? (_systemLoaders = new List<ISystemLoader>());
        }
    }

    public List<ISystemService> Services
    {
        get
        {
            return _services ?? (_services = new List<ISystemService>());
        }
    }

    public List<IEnumerator> ScenesQueue
    {
        get { return _scenesQueue ?? (_scenesQueue = new List<IEnumerator>()); }
    }

    void Awake()
    {
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

    public static IEnumerator IstantiateSceneAsyncAdditively(string sceneName)
    {
        var asyncOperation = Application.LoadLevelAdditiveAsync(sceneName);
        float lastProgress = -1;
        while (!asyncOperation.isDone)
        {
            if (lastProgress != asyncOperation.progress)
            {
                EventAggregator.Publish(new SceneLoaderEvent()
                {
                    State = SceneState.Instantiating,
                    Name = sceneName,
                    Progress = asyncOperation.progress
                });
                lastProgress = asyncOperation.progress;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator LoadSceneInternal(string sceneName)
    {
        yield return StartCoroutine(IstantiateSceneAsyncAdditively(sceneName));
        var sceneRoot = FindObjectsOfType<Scene>()
            .FirstOrDefault(scene => string.IsNullOrEmpty(scene.Name));

        if (sceneRoot == null) throw new Exception(string.Format("No IScene root is defined for {0} scene", sceneName));
        else
        {
            sceneRoot.Name = sceneName;
            this.Publish(new SceneLoaderEvent()
            {
                State = SceneState.Instantiated,
                SceneRoot = sceneRoot
            });
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
            State = SceneState.Instantiating,
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

        yield return StartCoroutine(sceneLoader.Unload(sceneRoot,updateDelegate));
        
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
    
    public void LoadScene(string name)
    {
        this.QueueSceneLoad(name);
        this.ExecuteLoad();
    }
    public void LoadScenes(params string[] sceneNames)
    {
        this.QueueScenesLoad(sceneNames);
        this.ExecuteLoad();
    }

    private IEnumerator Startup()
    {

        var attachedServices = gameObject.GetComponentsInChildren(typeof(SystemServiceMonoBehavior)).OfType<SystemServiceMonoBehavior>();
        foreach (var service in attachedServices)
        {
            this.Publish(new ServiceLoaderEvent() { State = ServiceState.Loading, Service = service });
            Container.RegisterService(service);
            service.Setup();
            yield return StartCoroutine(service.SetupAsync());
            Services.Add(service); //TODO: is that really needed??
            this.Publish(new ServiceLoaderEvent() { State = ServiceState.Loaded, Service = service });
        
        }

        var attachedSystemLoaders = gameObject.GetComponentsInChildren(typeof (ISystemLoader)).OfType<ISystemLoader>();
        foreach (var systemLoader in attachedSystemLoaders)
        {
            this.Publish(new SystemLoaderEvent() { State = SystemState.Loading, Loader = systemLoader});
            systemLoader.Container = Container;
            systemLoader.EventAggregator = EventAggregator;
            systemLoader.Load();
            SystemLoaders.Add(systemLoader);
            this.Publish(new SystemLoaderEvent() { State = SystemState.Loaded, Loader = systemLoader });
        }
        
        var attachedSceneLoaders = gameObject.GetComponentsInChildren(typeof(ISceneLoader)).OfType<ISceneLoader>();
        foreach (var sceneLoader in attachedSceneLoaders)
        {
            Container.RegisterSceneLoader(sceneLoader);
            Container.Inject(sceneLoader);
            SceneLoaders.Add(sceneLoader);
        }
        _defaultSceneLoader = gameObject.GetComponent<DefaultSceneLoader>() ?? gameObject.AddComponent<DefaultSceneLoader>();

        Container.InjectAll();
  
        foreach (var controller in Container.ResolveAll<Controller>())
        {
            controller.Setup();
        }

        _isKernelLoaded = true;
    
    }
}

public class SystemLoaderEvent
{
    public SystemState State { get; set; }
    public ISystemLoader Loader { get; set; }
}

public class ServiceLoaderEvent
{
    public ServiceState State { get; set; }
    public SystemServiceMonoBehavior Service { get; set; }
}

public class SceneLoaderEvent
{
    public SceneState State { get; set; }
    public IScene SceneRoot { get; set; }
    public float Progress { get; set; }
    public string ProgressMessage { get; set; }
    public string Name { get; set; }
}

public enum SceneState
{
    Loading,
    Update,
    Loaded,
    Unloading,
    Unloaded,
    Instantiating,
    Instantiated,
    Destructed
}

public enum ServiceState
{
    Loading,
    Loaded,
    Unloaded,
}
public enum SystemState
{
    Loading,
    Loaded,
    Unloaded,
}

public static class uFrameKernelExtensions
{
    public static void Publish(this uFrameMVVMKernel mvvmKernel, object evt)
    {
        uFrameMVVMKernel.EventAggregator.Publish(evt);
    }
}

