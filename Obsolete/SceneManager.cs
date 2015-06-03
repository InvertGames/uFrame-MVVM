using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using System.Reflection;
using uFrame.IOC;
using uFrame.Kernel;
using uFrame.MVVM;
using uFrame.Serialization;

/// <summary>
/// The main entry point for a game that is managed and accessible via GameManager. Only one will
/// available at a time.  This class when derived form should setup the container and load anything needed to properly
/// run a game.  This could include ViewModel Registering in the Container, Instantiating Views, Instantiating or Initializing Controllers.
/// </summary>
[Obsolete]
public abstract class SceneManager : uFrameComponent, ITypeResolver
{
    private List<ViewBase> _rootViews;

    [Inject]
    [Obsolete]
    public ICommandDispatcher CommandDispatcher { get; set; }


    /// <summary>
    /// The Dependency container for this scene.  If unset then it will use "GameManager.Container".
    /// </summary>
    public IUFrameContainer Container
    {
        get { return GameManager.Container; }
        set { }
    }

    /// <summary>
    /// This method should do any set up necessary to load the controller and is invoked when you call
    /// GameStateManager.SwitchGame().  This should call StartCoroutine(Controller.Load) on any
    /// regular controller in the scene.
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator Load(UpdateProgressDelegate progress)
    {
        yield return new WaitForSeconds(0.01f);
    }

    /// <summary>
    /// This method is called when the load function has completed
    /// </summary>
    public virtual void OnLoaded()
    {
        GameManager.EventAggregator.Publish(new LoadedEvent());
    }

    /// <summary>
    /// This method is called when this controller has started loading
    /// </summary>
    public virtual void OnLoading()
    {
    }

    /// <summary>
    /// This method simply starts the load method as a coroutine and should be overriden
    /// to add any reload logic that is necessary
    /// </summary>
    public virtual void Reload()
    {
        GameManager.Transition(this);
    }

    /// <summary>
    /// This method is called by the GameManager in order to register any dependencies.  It is one of the first things 
    /// to be invoked.  This method is called before the "Load" method.
    /// </summary>
    public virtual void Setup()
    {

    }

    /// <summary>
    /// This method should be used to property unload a scene when transitioning to another scene.
    /// </summary>
    public virtual void Unload()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// The awake method of this scenemanager simply registers itself with the GameManager
    /// </summary>
    protected virtual void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterSceneManager(this);
    }

    /// <summary>
    /// When this scene manager is destroy it is removed from the gamemanager.
    /// </summary>
    protected virtual void OnDestroy()
    {
        // Keep the controllers OnDestroy method from being invoked
        //base.OnDestroy();
        if (GameManager.Instance != null)
            GameManager.Instance.UnRegisterSceneManager(this);
    }

    /// <summary>
    /// The settings at which the level will be loaded with.  Used for transitioning from one scene to another.
    /// </summary>
    /// <value>The settings.</value>
    public static ISwitchLevelSettings Settings
    {
        get { return GameManager.SwitchLevelSettings; }
    }


    private List<ViewModel> _viewModels;

    /// <summary>
    /// Used by the SceneManager when creating an instance before the scene loads.  This allows a view-model instance to be ready
    /// before a view-initializes it. This is used by the uFrame generators to initialize single isntance view-models.
    /// </summary>
    /// <typeparam name="TViewModel">The type of view-model to create.</typeparam>
    /// <param name="controller">The controller that the view-model should be initialized with</param>
    /// <param name="identifier">The identifier of the view-model to be created or loaded (if reloading a scenes state).</param>
    /// <returns>A new view model or the view-model with the identifier specified found in the scene context.</returns>
    [Obsolete]
    public TViewModel SetupViewModel<TViewModel>(Controller controller, string identifier)
        where TViewModel : ViewModel
    {
        return null;
    }

    /// <summary>
    /// Used by the SceneManager when creating an instance before the scene loads.  This allows a view-model instance to be ready
    /// before a view-initializes it. This is used by the uFrame generators to initialize single isntance view-models.
    /// </summary>
    /// <typeparam name="TViewModel">The type of view-model to create.</typeparam>
    /// <param name="controller">The controller that the view-model should be initialized with</param>
    /// <param name="identifier">The identifier of the view-model to be created or loaded (if reloading a scenes state).</param>
    /// <returns>A new view model or the view-model with the identifier specified found in the scene context.</returns>

    public TViewModel SetupViewModel<TViewModel>(string identifier) where TViewModel : ViewModel
    {
        // Create the ViewModel
        var contextViewModel = Activator.CreateInstance(typeof (TViewModel), EventAggregator) as TViewModel;
        contextViewModel.Identifier = identifier;
        // Register the instance under "ViewModel" so any single instance view-model can be accessed with ResolveAll<ViewModel>();
        Container.RegisterViewModel<TViewModel>(contextViewModel, identifier);
        return contextViewModel;
    }

    public TViewModel CreateInstanceViewModel<TViewModel>(string identifier) where TViewModel : ViewModel
    {
        var contextViewModel = Activator.CreateInstance(typeof (TViewModel), EventAggregator) as TViewModel;
        contextViewModel.Identifier = identifier;
        return contextViewModel;
    }

    [Obsolete]
    public TViewModel CreateInstanceViewModel<TViewModel>(Controller controller, string identifier)
        where TViewModel : ViewModel
    {
        return null;
    }

    /// <summary>
    /// All of the views that have registered
    /// </summary>
    public List<ViewBase> PersistantViews
    {
        get { return _rootViews ?? (_rootViews = new List<ViewBase>()); }
        set { _rootViews = value; }
    }

    /// <summary>
    /// All of the view-models in the current scene marked for persistance
    /// </summary>
    public List<ViewModel> PersistantViewModels
    {
        get { return _viewModels ?? (_viewModels = new List<ViewModel>()); }
        set { _viewModels = value; }
    }

    public void LoadState(ISerializerStorage storage, ISerializerStream stream)
    {
        // Enforce required settings for scene state loading
        stream.DeepSerialize = true;
        stream.TypeResolver = this;
        stream.DependencyContainer = Container;
        storage.Load(stream);

        // STEP 1: Load the viewmodels
        var viewModels = stream.DeserializeObjectArray<ViewModel>("ViewModels");
        foreach (var viewModel in viewModels)
        {
            VoidMethod(viewModel);
            // Do something here maybe?
        }

        // STEP 2: LOAD THE VIEWS
        stream.TypeResolver = this;
        // Clear the reference objects because the view-models will share the same identifier with views.
        stream.ReferenceObjects.Clear();
        var views = stream.DeserializeObjectArray<ViewBase>("Views").ToArray();
        foreach (var view in views)
        {
            VoidMethod(view);
            // Do something here maybe?
        }
    }

    private void VoidMethod(object p)
    {
    }

    public void SaveState(ISerializerStorage storage, ISerializerStream stream)
    {
        stream.DeepSerialize = true;
        stream.TypeResolver = this;
        // Serialize The View Models
        stream.SerializeArray("ViewModels", PersistantViewModels);
        // Clear the references so view-models and view of the same identifier don't match up
        stream.ReferenceObjects.Clear();
        // Serialize the views
        stream.SerializeArray("Views", PersistantViews);
        // Serialize the stream
        storage.Save(stream);
    }

    public void RegisterView(ViewBase view, ViewModel viewModel = null)
    {
        var vm = viewModel ?? view.ViewModelObject;

        if (!PersistantViews.Contains(view))
            PersistantViews.Add(view);

        if (!PersistantViewModels.Contains(vm))
            PersistantViewModels.Add(vm);

        //vm.Identifier = view.Identifier;
    }

    [Obsolete]
    public ViewModel RequestViewModel(ViewBase viewBase, Controller controller)
    {
        return null;
    }

    /// <summary>
    /// This is method is called by each view in order to get it's view-model as well as place it in
    /// the SceneContext if the "Save & Load" option is checked in it's inspector
    /// </summary>
    /// <param name="viewBase">The view that is requesting it's view-model.</param>
    /// <param name="controller">The controller that should be assigned to the view-model if any.</param>
    /// <returns>A new view model or the view-model with the identifier specified found in the scene context.</returns>
    public ViewModel RequestViewModel(ViewBase viewBase)
    {
        if (viewBase.InjectView)
        {
            Container.Inject(viewBase);
        }
        // Attempt to resolve it by the identifier 
        var contextViewModel = Container.Resolve<ViewModel>(viewBase.Identifier);
        // If it doesn't resolve by the identifier we need to create it
        if (contextViewModel == null)
        {
            // Either use the controller to create it or create it ourselves
            contextViewModel = Activator.CreateInstance(viewBase.ViewModelType, EventAggregator) as ViewModel;
            contextViewModel.Identifier = viewBase.Identifier;
//            if (viewBase.ForceResolveViewModel)
//            {   
//                // Register it, this is usually when a non registered element is treated like a single-instance anways
//                Container.RegisterInstance(viewBase.ViewModelType, contextViewModel,
//                    string.IsNullOrEmpty(viewBase.Identifier) ? null : viewBase.Identifier);
//                // Register it under the generic view-model type
//                Container.RegisterInstance<ViewModel>(contextViewModel, viewBase.Identifier);
//            }
            //else
            //{
            //    // Inject the View-Model
            //    Container.Inject(contextViewModel);
            //}


            Publish(new ViewModelCreatedEvent()
            {
                ViewModel = contextViewModel
            });
        }
        // If we found a view-model
        if (contextViewModel != null)
        {
            // If the view needs to be overriden it will initialize with the inspector values
            if (viewBase.OverrideViewModel)
            {
                viewBase.InitializeData(contextViewModel);
            }
        }
        // Save if the "Save" checkbox in the view inspector is checked
//        if (viewBase.Save)
//        {
//            // Register a view for persistance
//            RegisterView(viewBase, contextViewModel);
//        }
        return contextViewModel;
    }

    protected virtual ViewBase ViewNotFoundOnLoad(string typeName, string identifier)
    {
        return null;
    }

    public virtual void Initialize()
    {
        var systemControllers = Container.ResolveAll<ISystemService>();
        foreach (var systemController in systemControllers)
        {
            systemController.Setup();
        }

    }

    Type ITypeResolver.GetType(string name)
    {
        return Type.GetType(name);
    }

    string ITypeResolver.SetType(Type type)
    {
        return type.AssemblyQualifiedName;
    }

    object ITypeResolver.CreateInstance(string name, string identifier)
    {
        var type = ((ITypeResolver) this).GetType(name);

#if NETFX_CORE 
    var isViewModel = type.GetTypeInfo().IsSubclassOf(typeof(ViewModel));
#else
        var isViewModel = typeof (ViewModel).IsAssignableFrom(type);
#endif
        // IsAssignableFrom doesn't work with winRT
        // typeof(ViewModel).IsAssignableFrom(type);

        if (isViewModel)
        {
            var contextViewModel = PersistantViewModels.FirstOrDefault(p => p.Identifier == identifier);
            if (contextViewModel != null)
            {
                return contextViewModel;
            }
            return Activator.CreateInstance(type);
        }
        var view = PersistantViews.FirstOrDefault(p => p.Identifier == identifier);
        if (view != null)
        {
            Debug.Log(string.Format("Loading View: {0} - {1}", name, identifier));
            return view;
        }
        return ViewNotFoundOnLoad(name, identifier);
    }

    public void UnRegisterView(ViewBase viewBase)
    {
        PersistantViews.Remove(viewBase);
    }
}

[Obsolete]
public static class SceneManagerExtensions
{

    public static void FromJson(this SceneManager sceneManager, string stateData)
    {
        var stringStorage = new StringSerializerStorage()
        {
            Result = stateData
        };
        var stream = new JsonStream();
        sceneManager.LoadState(stringStorage, stream);
    }

    public static string ToJson(this SceneManager sceneManager)
    {
        var stringStorage = new StringSerializerStorage();
        var stream = new JsonStream();
        sceneManager.SaveState(stringStorage, stream);
        return stringStorage.Result;
    }
}

public static class ContainerExtensions
{
}

public class LoadedEvent
{
}
