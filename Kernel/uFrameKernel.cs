using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using uFrame.Attributes;
using uFrame.IOC;
using UniRx;

namespace uFrame.Kernel
{
    public class uFrameKernel : MonoBehaviour
    {

        private static UFrameContainer _container;
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

        public static uFrameKernel Instance { get; set; }

        public static IUFrameContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new UFrameContainer();
                    _container.RegisterInstance<IUFrameContainer>(_container);
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
            get { return _systemLoaders ?? (_systemLoaders = new List<ISystemLoader>()); }
        }

        public List<ISystemService> Services
        {
            get { return _services ?? (_services = new List<ISystemService>()); }
        }

        private void Awake()
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
            var attachedSystemLoaders =
                gameObject.GetComponentsInChildren(typeof (ISystemLoader)).OfType<ISystemLoader>();

            foreach (var systemLoader in attachedSystemLoaders)
            {
                this.Publish(new SystemLoaderEvent() {State = SystemState.Loading, Loader = systemLoader});
                systemLoader.Container = Container;
                systemLoader.EventAggregator = EventAggregator;
                systemLoader.Load();
                SystemLoaders.Add(systemLoader);
                this.Publish(new SystemLoaderEvent() {State = SystemState.Loaded, Loader = systemLoader});
            }

            var attachedServices = gameObject.GetComponentsInChildren(typeof (SystemServiceMonoBehavior))
                .OfType<SystemServiceMonoBehavior>()
                .Where(_ => _.enabled)
                .ToArray();

            foreach (var service in attachedServices)
            {
                Container.RegisterService(service);
                Services.Add(service);
            }

            Container.InjectAll();
            var allServices = Container.ResolveAll<ISystemService>().ToArray();
            foreach (var service in allServices)
            {
                this.Publish(new ServiceLoaderEvent() {State = ServiceState.Loading, Service = service});
                service.Setup();
                yield return StartCoroutine(service.SetupAsync());
                this.Publish(new ServiceLoaderEvent() {State = ServiceState.Loaded, Service = service});
            }

            this.Publish(new SystemsLoadedEvent()
            {
                Kernel = this
            });

            _isKernelLoaded = true;

            this.Publish(new KernelLoadedEvent()
            {
                Kernel = this
            });
            yield return new WaitForEndOfFrame(); //Ensure that everything is bound
            yield return new WaitForEndOfFrame();
            this.Publish(new GameReadyEvent());
        }

        public void OnDestroy()
        {
            _container = null;
            IsKernelLoaded = false;
            Services.Clear();
            SystemLoaders.Clear();
            EventAggregator = null;
            Instance = null;
        }

        public void ResetKernel()
        {
            DestroyImmediate(Instance.gameObject);
            _container = null;
            IsKernelLoaded = false;
            Services.Clear();
            SystemLoaders.Clear();
            EventAggregator = null;
            Instance = null;
        }
  
        public static void DestroyKernel(string levelToLoad = null)
        {

            Instance.ResetKernel();
            if (levelToLoad != null)
                Application.LoadLevel(levelToLoad);

        }
    }

    public class SystemsLoadedEvent
    {
        public uFrameKernel Kernel;
    }
    /// <summary>
    /// 
    /// </summary>
    [uFrameEvent("Kernel Loaded")]
    public class KernelLoadedEvent
    {
        public uFrameKernel Kernel;
    }
    /// <summary>
    /// 
    /// </summary>
    [uFrameEvent("Game Ready")]
    public class GameReadyEvent
    {

    }
    /// <summary>
    /// 
    /// </summary>
    
    public class LoadSceneCommand
    {

        public string SceneName { get; set; }
        public ISceneSettings Settings { get; set; }
        public bool RestrictToSingleScene { get; set; }
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
        public static void RegisterService(this IUFrameContainer container, ISystemService service)
        {
            container.RegisterInstance<ISystemService>(service, service.GetType().Name);
            //container.RegisterInstance(typeof(TService), service, false);
            container.RegisterInstance(service.GetType(), service);
        }

        public static void RegisterService<TService>(this IUFrameContainer container, ISystemService service)
        {
            container.RegisterInstance<ISystemService>(service, service.GetType().Name);
            container.RegisterInstance(typeof (TService), service);
        }

        public static void RegisterSceneLoader(this IUFrameContainer container, ISceneLoader sceneLoader)
        {
            container.RegisterInstance<ISceneLoader>(sceneLoader, sceneLoader.GetType().Name, false);
            //container.RegisterInstance(typeof(TService), service, false);
            container.RegisterInstance(sceneLoader.GetType(), sceneLoader, false);
        }


        public static void Publish(this uFrameKernel mvvmKernel, object evt)
        {
            uFrameKernel.EventAggregator.Publish(evt);
        }

        public static IObservable<T> OnEvent<T>(this uFrameKernel mvvmKernel)
        {
            return uFrameKernel.EventAggregator.GetEvent<T>();
        }
    }
}