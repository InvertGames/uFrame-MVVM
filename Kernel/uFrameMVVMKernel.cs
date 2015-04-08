using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class uFrameMVVMKernel : MonoBehaviour {

    private static GameContainer _container;
    private static IEventAggregator _eventAggregator;
    private static IViewResolver _viewResolver;
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
            if (this.gameObject.GetComponent<MainThreadDispatcher>() == null)
                this.gameObject.AddComponent<MainThreadDispatcher>();
            DontDestroyOnLoad(gameObject);
            StartCoroutine(Startup());
        }
    }

    private IEnumerator Startup()
    {

        var attachedServices = gameObject.GetComponentsInChildren(typeof(SystemServiceMonoBehavior)).OfType<SystemServiceMonoBehavior>();
        foreach (var service in attachedServices)
        {
            this.Publish(new ServiceLoaderEvent() { State = ServiceState.Loading, Service = service });
            service.EventAggregator = EventAggregator;
            service.Setup();
            Container.RegisterService(service);
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

        this.Publish(new SystemsLoadedEvent()
        {
            Kernel = this
        });
       
        Container.InjectAll();
  
        foreach (var controller in Container.ResolveAll<Controller>())
        {
            controller.Setup();
        }
        _isKernelLoaded = true;
        this.Publish(new KernalLoadedEvent()
        {
            Kernel = this
        });
        yield return new WaitForEndOfFrame();
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

public class ViewEvent
{
    public bool IsInstantiated { get; set; }
    public IScene Scene { get; set; }
    public ViewBase View { get; set; }
}
public class ViewDestroyedEvent : ViewEvent
{
   
}

public class ViewCreatedEvent :ViewEvent
{
    
}

public class GameReadyEvent : ViewEvent
{
    
}
public class InstantiateViewCommand
{
    public string Identifier { get; set; }
    public IScene Scene { get; set; }
    public ViewModel ViewModelObject { get; set; }
    public ViewBase Result { get; set; }
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
    
    public static IObservable<T> OnEvent<T>(this uFrameMVVMKernel mvvmKernel)
    {
        return uFrameMVVMKernel.EventAggregator.GetEvent<T>();
    }
}

