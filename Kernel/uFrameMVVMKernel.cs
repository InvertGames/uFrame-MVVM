using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;
using UniRx;

public class uFrameMVVMKernel : MonoBehaviour {

    private static GameContainer _container;
    private static IEventAggregator _eventAggregator;

    private static bool _isKernelLoaded;
    private List<ISystemService> _services;
    private List<ISystemLoader> _systemLoaders;

    public static IEnumerator InstantiateSceneAsyncAdditively(string sceneName)
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
                _container.RegisterInstance<IGameContainer>(_container);
                _container.RegisterInstance<IEventAggregator>(EventAggregator);
         
            }
            return _container;
        }
    }

    public static IEventAggregator EventAggregator
    {
        get { return _eventAggregator ?? (_eventAggregator = new EventAggregator()); }
        set { _eventAggregator = value; }
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

    void Awake()
    {
        if (Instance != null)
        {
            throw new Exception("Loading Kernel twice is not a good practice!");
        }
        else
        {
            Instance = this;
            //if (this.gameObject.GetComponent<MainThreadDispatcher>() == null)
            //    this.gameObject.AddComponent<MainThreadDispatcher>();
            DontDestroyOnLoad(gameObject);
            StartCoroutine(Startup());
        }
    }

    private IEnumerator Startup()
    {
        var attachedSystemLoaders = gameObject.GetComponentsInChildren(typeof(ISystemLoader)).OfType<ISystemLoader>();

        foreach (var systemLoader in attachedSystemLoaders)
        {
            this.Publish(new SystemLoaderEvent() { State = SystemState.Loading, Loader = systemLoader });
            systemLoader.Container = Container;
            systemLoader.EventAggregator = EventAggregator;
            systemLoader.Load();
            SystemLoaders.Add(systemLoader);
            this.Publish(new SystemLoaderEvent() { State = SystemState.Loaded, Loader = systemLoader });
        }

        var attachedServices = gameObject.GetComponentsInChildren(typeof(SystemServiceMonoBehavior))
            .OfType<SystemServiceMonoBehavior>()
            .Where(_=>_.isActiveAndEnabled)
            .ToArray();

        foreach (var service in attachedServices)
        {
            Container.RegisterService(service);
            service.EventAggregator = EventAggregator; // JUST IN CASE, MAN
            Services.Add(service);        
        }

        Container.InjectAll();

        foreach (var service in Container.ResolveAll<ISystemService>())
        {
            this.Publish(new ServiceLoaderEvent() { State = ServiceState.Loading, Service = service });
            service.Setup();
            yield return StartCoroutine(service.SetupAsync());
            this.Publish(new ServiceLoaderEvent() { State = ServiceState.Loaded, Service = service });
        }

        this.Publish(new SystemsLoadedEvent()
        {
            Kernel = this
        });

        _isKernelLoaded = true;
        
        this.Publish(new KernalLoadedEvent()
        {
            Kernel = this
        });
        yield return new WaitForEndOfFrame(); //Ensure that everything is bound
        yield return new WaitForEndOfFrame();
        this.Publish(new GameReadyEvent());
    }

}

public class SystemsLoadedEvent
{
    public uFrameMVVMKernel Kernel;
}

public class KernalLoadedEvent
{
    public uFrameMVVMKernel Kernel;
}

public class GameReadyEvent 
{
    
}

public class LoadSceneCommand
{

    public string SceneName { get; set; }
    public ISceneSettings Settings { get; set; }

}

public class UnloadSceneCommand
{
    public string SceneName { get; set; }
}

public class SystemLoaderEvent
{
    public SystemState State { get; set; }
    public ISystemLoader Loader { get; set; }
}

public class ServiceLoaderEvent
{
    public ServiceState State { get; set; }
    public ISystemService Service { get; set; }
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
    public static void RegisterService(this IGameContainer container, ISystemService service)
    {
        container.RegisterInstance<ISystemService>(service, service.GetType().Name);
        //container.RegisterInstance(typeof(TService), service, false);
        container.RegisterInstance(service.GetType(), service);
    }

    public static void RegisterService<TService>(this IGameContainer container, ISystemService service)
    {
        container.RegisterInstance<ISystemService>(service, service.GetType().Name);
        container.RegisterInstance(typeof(TService), service);
    }

    public static void RegisterSceneLoader(this IGameContainer container, ISceneLoader sceneLoader)
    {
        container.RegisterInstance<ISceneLoader>(sceneLoader, sceneLoader.GetType().Name, false);
        //container.RegisterInstance(typeof(TService), service, false);
        container.RegisterInstance(sceneLoader.GetType(), sceneLoader, false);
    }


    public static void Publish(this uFrameMVVMKernel mvvmKernel, object evt)
    {
        uFrameMVVMKernel.EventAggregator.Publish(evt);
    }
    
    public static IObservable<T> OnEvent<T>(this uFrameMVVMKernel mvvmKernel)
    {
        return uFrameMVVMKernel.EventAggregator.GetEvent<T>();
    }
}