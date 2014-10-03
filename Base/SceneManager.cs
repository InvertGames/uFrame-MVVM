using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The main entry point for a game that is managed and accessible via GameManager. Only one will
/// available at a time.  This class when derived form should setup the container and load anything needed to properly
/// run a game.  This could include ViewModel Registering in the Container, Instantiating Views, Instantiating or Initializing Controllers.
/// </summary>
public abstract class SceneManager : ViewContainer, ITypeResolver
{
    private List<ViewBase> _rootViews;
    private SceneContext _context;

    [Inject]
    public ICommandDispatcher CommandDispatcher
    {
        get; set;
    }

    /// <summary>
    /// The Dependency container for this scene.  If unset then it will use "GameManager.Container".
    /// </summary>
    public IGameContainer Container
    {
        get
        {
            return GameManager.Container;
        }
        set
        {

        }
    }

    /// <summary>
    /// The scene context for the current running scene.  Used for Saving and loading a scenes state.
    /// </summary>
    public SceneContext Context
    {
        get { return _context ?? (_context = new SceneContext(Container)); }
        set { _context = value; }
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
        get
        {
            return GameManager.SwitchLevelSettings;
        }
    }


    private List<Action> _initializers = new List<Action>();
    /// <summary>
    /// Used by the SceneManager when creating an instance before the scene loads.  This allows a view-model instance to be ready
    /// before a view-initializes it. This is used by the uFrame generators to initialize single isntance view-models.
    /// </summary>
    /// <typeparam name="TViewModel">The type of view-model to create.</typeparam>
    /// <param name="controller">The controller that the view-model should be initialized with</param>
    /// <param name="identifier">The identifier of the view-model to be created or loaded (if reloading a scenes state).</param>
    /// <returns>A new view model or the view-model with the identifier specified found in the scene context.</returns>
    public TViewModel SetupViewModel<TViewModel>(Controller controller, string identifier) where TViewModel : ViewModel, new()
    {
        // Create the ViewModel
        var contextViewModel = new TViewModel();
        // Register the instance under "ViewModel" so any single instance view-model can be accessed with ResolveAll<ViewModel>();
        Container.RegisterInstance<ViewModel>(contextViewModel as TViewModel, identifier);
        // Add an initializer so that apply the controller happens after everything has been injected
        _initializers.Add(()=> { contextViewModel.Controller = controller; controller.Initialize(contextViewModel); });
        return contextViewModel;
    }
    public ISerializerStorage LoadingStream { get; set; }

    public List<ViewBase> RootViews
    {
        get { return _rootViews ?? (_rootViews = new List<ViewBase>()); }
        set { _rootViews = value; }
    }

    public void Load(ISerializerStorage storage, ISerializerStream stream)
    {
        stream.TypeResolver = this;
        stream.DependencyContainer = Container;
        storage.Load(stream);
        var viewModels = stream.DeserializeObjectArray<ViewModel>("ViewModels").ToArray();
        var views = stream.DeserializeObjectArray<ViewBase>("Views").ToArray();
        foreach (var view in views)
        {
            
        }
        foreach (var viewModel in viewModels)
        {
            
        }

    }
    public void Save(ISerializerStorage storage, ISerializerStream stream)
    {
        stream.TypeResolver = this;
        stream.SerializeArray("Views", RootViews);
        stream.SerializeArray("ViewModels", Context.PersitantViewModels);
        storage.Save(stream);
    }

 
    /// <summary>
    /// This is method is called by each view in order to get it's view-model as well as place it in
    /// the SceneContext if the "Save & Load" option is checked in it's inspector
    /// </summary>
    /// <param name="viewBase">The view that is requesting it's view-model.</param>
    /// <param name="controller">The controller that should be assigned to the view-model if any.</param>
    /// <param name="identifier">The identifier of the view-model to be created or loaded (if reloading a scenes state).</param>
    /// <returns>A new view model or the view-model with the identifier specified found in the scene context.</returns>
    public ViewModel RequestViewModel(ViewBase viewBase, Controller controller, string identifier)
    {
        RootViews.Add(viewBase);
        var contextViewModel = Context[identifier];
        if (contextViewModel == null)
        {
            contextViewModel = Container.Resolve(viewBase.ViewModelType) as ViewModel;
            if (contextViewModel == null)
            {
                contextViewModel = Container.Resolve<ViewModel>(identifier);
                if (contextViewModel == null)
                {
                    contextViewModel = controller == null ? Activator.CreateInstance(viewBase.ViewModelType) as ViewModel : controller.CreateEmpty(identifier);

                    Context[identifier] = contextViewModel;

                    if (viewBase.ForceResolveViewModel)
                    {
                        
                        Container.RegisterInstance(viewBase.ViewModelType, contextViewModel,
                            string.IsNullOrEmpty(identifier) ? null : identifier);

                    }
                }
            }

            Context[identifier] = contextViewModel;
        }
        if (contextViewModel != null)
        {

            // Make sure its wired to the controller
            if (controller != null)
            contextViewModel.Controller = controller;


            // If its just an empty view model we need to initialize it
            if (viewBase.OverrideViewModel)
            {

                viewBase.InitializeData(contextViewModel);
                if (controller != null)
                controller.Initialize(contextViewModel);
            }

            if (viewBase.Save)
            {
                if (identifier != null)
                {
                    if (!Context.PersitantViewModels.ContainsKey(identifier))
                        Context.PersitantViewModels.Add(identifier, contextViewModel);
                }
            }

        }

        return contextViewModel;
    }

    public virtual void Initialize()
    {
        foreach (var initalizer in _initializers)
        {
            if (initalizer != null)
                initalizer();
        }
    }

    public Type GetType(string name)
    {
        return Type.GetType(name);
    }

    public string SetType(Type type)
    {
        return type.AssemblyQualifiedName;
    }

    public object CreateInstance(string name, string identifier)
    {
        var view = RootViews.FirstOrDefault(p => p.Identifier == identifier);
        if (view != null)
        {
            Debug.Log("Found View",view);
            return view;
        }
        Debug.Log("Couldn't find view" + name + identifier);
        return null;
    }
}

public static class SceneManagerExtensions
{

    public static string ToJson(this SceneManager sceneManager)
    {
        var stringStorage = new StringSerializerStorage();
        var stream = new JsonStream();
        sceneManager.Save(stringStorage,stream);
        return stringStorage.Result;
    }
}