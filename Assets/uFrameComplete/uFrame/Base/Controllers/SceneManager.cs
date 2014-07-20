using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;


/// <summary>
/// The main entry point for a game that is managed and accessible via GameManager. Only one will
/// available at a time.  This class when derived form should setup the container and load anything needed to properly
/// run a game.  This could include ViewModel Registering in the Container, Instantiating Views, Instantiating or Initializing Controllers.
/// </summary>
public abstract class SceneManager : ViewContainer
{
    private List<ViewBase> _rootViews;
    private SceneContext _context;

    public IGameContainer Container
    {
        get { return GameManager.Container; }
        set
        {
            
        }
    }

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
    /// This method is called when this controller has started loading
    /// </summary>
    public virtual void OnLoaded()
    {
    }

    /// <summary>
    /// This method is called when the load function has completed
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
        
        GameManager.SwitchGame(this);
    }

    public virtual void Setup()
    {
    }

    public virtual void Unload()
    {
        StopAllCoroutines();
    }
    protected virtual void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.AddGame(this);
    }

    protected virtual void OnDestroy()
    {
        // Keep the controllers OnDestroy method from being invoked
        //base.OnDestroy();
        if (GameManager.Instance != null)
            GameManager.Instance.RemoveGame(this);
    }



    /// <summary>
    /// The settings at which the level will be loaded
    /// </summary>
    /// <value>The settings.</value>
    public static ISwitchLevelSettings Settings
    {
        get
        {
            return GameManager.SwitchLevelSettings;
        }
    }

    public void RegisterRootView(ViewBase viewBase)
    {
        RootViews.Add(viewBase);
    }

    public List<ViewBase> RootViews
    {
        get { return _rootViews ?? (_rootViews = new List<ViewBase>()); }
        set { _rootViews = value; }
    }

    public ViewModel SetupViewModel(Controller controller, string identifier)
    {
        var contextViewModel = Context[identifier];
        if (contextViewModel == null)
        {
            
            return controller.CreateEmpty(identifier);
        }
        return contextViewModel;
    }
    public ViewModel RequestViewModel(ViewBase viewBase, Controller controller, string identifier)
    {
        var contextViewModel = Context[identifier];

        if (contextViewModel == null)
        {
            contextViewModel = Container.Resolve(viewBase.ViewModelType) as ViewModel;
            if (contextViewModel == null)
            {
                contextViewModel = Container.Resolve<ViewModel>(identifier);
                if (contextViewModel == null)
                {
                    contextViewModel = controller.CreateEmpty(identifier);
                    Context[identifier] = contextViewModel;
                    if (!viewBase.IsMultiInstance || viewBase.ForceResolveViewModel)
                    {
                        Container.RegisterInstance(viewBase.ViewModelType, contextViewModel,
                            string.IsNullOrEmpty(identifier) ? null : identifier);
                    }
                }
                else
                {
                    //contextViewModel.Identifier = identifier;
                    //// Make sure it's added to the context
                    //Context[identifier] = contextViewModel;
                }
            }
            else
            {
                //contextViewModel.Identifier = identifier;
                //// Make sure it's added to the context
                //Context[identifier] = contextViewModel;
            }
        }
        else
        {
           
            
           // return contextViewModel;
        }
        if (contextViewModel != null)
        {
            // If its just an empty view model we need to initialize it
            if (viewBase.OverrideViewModel)
            {
                contextViewModel.Identifier = identifier;
                Context[identifier] = contextViewModel;

                viewBase.InitializeData(contextViewModel);
                controller.WireCommands(contextViewModel);
                controller.Initialize(contextViewModel);
            }
           
        }

        return contextViewModel;
    }
}

[Obsolete("GameType has been renamed to SceneManager.  Please rename.")]
public class GameType
{
    
}