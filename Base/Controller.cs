using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if DLL
namespace Invert.Common.MVVM
{
#endif
public interface ICommandHandler
{
    void ExecuteCommand(ICommand command, object argument);
    void ExecuteCommand(ICommand command);
    void ExecuteCommand<TArgument>(ICommandWith<TArgument> command, TArgument argument);
}

/// <summary>
/// A controller is a group of commands usually to provide an abstract level
/// </summary>
public abstract class Controller : ICommandHandler
{

    private SceneContext _context;

#if TESTS

    /// <summary>
    /// The dependency container that this controller will use
    /// </summary>
    public IGameContainer Container { get; set; }
    /// <summary>
    /// The scene context that contains the View-Models for the current scene.
    /// </summary>
    public SceneContext Context
    {
        get { return _context; }
        set
        {
            _context = value;
            if (value != null)
                Container = value.Container;
        }
    }

#else
    /// <summary>
    /// The dependency container that this controller will use
    /// </summary>
    public IGameContainer Container {
        get
        {
            return GameManager.Container;
        }
        set
        {
            
        }
    }
    /// <summary>
    /// The scene context that contains the View-Models for the current scene.
    /// </summary>
    public SceneContext Context
    {
        get { return _context ?? GameManager.ActiveSceneManager.Context; }
        set
        {
            _context = value;
            if (value != null)
                Container = value.Container;
        }
    }

#endif

    protected Controller()
    {
        //throw new Exception("Default constructor is not allowed.  Please regenerate your diagram or create the controller with a SceneContext.");
    }

    /// <summary>
    /// Initialize this controller with a SceneContext object
    /// </summary>
    /// <param name="context"></param>
    protected Controller(SceneContext context)
    {
        Context = context;
    }


    /// <summary>
    /// Create a new ViewModel. This will generate a Unique Identifier for the VM.  If this is a specific instance use the overload and pass
    /// an identifier.
    /// </summary>
    /// <returns></returns>
    public virtual ViewModel Create()
    {
        return Create(Guid.NewGuid().ToString());
    }

    /// <summary>
    /// Creates a new ViewModel with a specific identifier.  If it already exists in the SceneContext it will return that instead
    /// </summary>
    /// <param name="identifier">The identifier that will be used to check the context to see if it already exists.</param>
    /// <returns></returns>
    public virtual ViewModel Create(string identifier)
    {
        var vm = Context[identifier];

        if (vm == null)
        {
            vm = CreateEmpty(identifier);
            vm.Controller = this;
            Initialize(vm);
        }
        return vm;
    }

    /// <summary>
    /// Create an empty view-model with the specified identifer. Note: This method does not wire up the view-model to this controller.
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns>A new View-Model or the view-model found in the context with the same identifier.</returns>
    public virtual ViewModel CreateEmpty(string identifier)
    {
        var vm = CreateEmpty();
        vm.Identifier = identifier;
        return vm;
    }

    /// <summary>
    /// Create an empty view-model . Note: This method does not wire up the view-model to this controller and only instantiates an associated view-model.
    /// </summary>
    /// <returns>A new View-Model or the view-model found in the context with the same identifier.</returns>
    public virtual ViewModel CreateEmpty()
    {
        throw new NotImplementedException("You propably need to resave you're diagram. Or you need to not call create on an abstract controller.");
    }

    public virtual void Map(IElementModel model, ViewModel viewModel)
    {
        
    }

    public abstract void Initialize(ViewModel viewModel);

    [Obsolete("WireCommands no longer lives in the controller. Regenerate your diagram.")]
    public virtual void WireCommands(ViewModel viewModel)
    {

    }

#if !TESTS
    public void ExecuteCommand(ICommand command, object argument)
    {

        if (command == null)
        {
            throw new Exception("Command is null.  This is probably cause 'Initialize View Model' has not been checked in the inspector.");
        }
        command.Parameter = argument;
        if (command.Parameter == null)
        {
            command.Parameter = argument;
        }
        IEnumerator enumerator = command.Execute();
        if (enumerator != null)
            StartCoroutine(enumerator);
    }

    public virtual void ExecuteCommand(ICommand command)
    {

        if (command == null)
        {
            throw new Exception("Command is null.  This is probably cause 'Initialize View Model' has not been checked in the inspector.");
        }
        //command.Sender = null;
        command.Parameter = null;

        IEnumerator enumerator = command.Execute();
        if (enumerator != null)
            StartCoroutine(enumerator);
    }

    public void ExecuteCommand<TArgument>(ICommandWith<TArgument> command, TArgument argument)
    {
        if (command == null)
        {
            throw new Exception("Command is null.  This is probably cause 'Initialize View Model' has not been checked in the inspector.");
        }
        command.Parameter = argument;

        IEnumerator enumerator = command.Execute();
        if (enumerator != null)
            StartCoroutine(enumerator);
    }


    /// <summary>
    /// Send an event to our game
    /// </summary>
    /// <param name="message"></param>
    /// <param name="additionalParamters"></param>
    public virtual void GameEvent(string message, params object[] additionalParamters)
    {
        Event(null, message, additionalParamters);
    }

    public UnityEngine.Coroutine StartCoroutine(IEnumerator routine)
    {
        return GameManager.ActiveSceneManager.StartCoroutine(routine);
    }

    public void StopAllCoroutines()
    {
        GameManager.ActiveSceneManager.StopAllCoroutines();
    }

    public void StopCoroutine(string name)
    {
        GameManager.ActiveSceneManager.StopCoroutine(name);
    }

    public ModelPropertyBinding SubscribeToProperty<TViewModel, TBindingType>(TViewModel source, P<TBindingType> sourceProperty, Action<TViewModel, TBindingType> changedAction) where TViewModel : ViewModel
    {
        var binding = new ModelPropertyBinding()
        {
            SetTargetValueDelegate = (o) => changedAction(source, (TBindingType)o),
            ModelPropertySelector = () => sourceProperty,
            IsImmediate = false
        };
        source.AddBinding(binding);
        return binding;
    }
#endif
    //public ModelPropertyBinding SubscribeToProperty<TBindingType>(P<TBindingType> sourceProperty, Action<TBindingType> targetSetter)
    //{
    //    var binding = new ModelPropertyBinding()
    //    {
    //        SetTargetValueDelegate = (o) => targetSetter((TBindingType)o),
    //        ModelPropertySelector = () => (ModelPropertyBase)sourceProperty,
    //        IsImmediate = false
    //    };
    //    sourceProperty().AddBinding(binding);
    //    return binding;
    //}

    protected void SubscribeToCommand(ICommand command, Action action)
    {
        command.OnCommandExecuted += () => action();

    }

#if !TESTS
    /// <summary>
    /// \brief Send an event to a game.
    /// Additional parameters shouldn't pass the view to the controller unless absolutely necessary.
    /// A warning will be issued if you try to pass a view to the controller
    /// </summary>
    /// <param name="model">The model at which the controller will accept automatically as its first parameter.</param>
    /// <param name="message">The event/method that will be sent to the controller.</param>
    /// <param name="additionalParameters">Any additional information to pass along with the event.</param>
    private void Event(ViewModel model, string message, params object[] additionalParameters)
    {
        var sceneManager = GameManager.ActiveSceneManager;
        if (sceneManager == null)
        {
            throw new Exception("SceneManager is not set.");
        }
        var method = sceneManager.GetType().GetMethod(message);

        if (method == null)
        {
            throw new Exception(string.Format("Event '{0}' was not found on {1}", message, sceneManager));
        }

        if (model != null && method.GetParameters().Length > 0)
        {
            var list = new List<object>();
            foreach (object o in Enumerable.Concat(new[] { model }, additionalParameters))
            {
                if ((o is IViewModelObserver))
                {
#if !DLL
                    UnityEngine.Debug.LogWarning("A view was passed as a parameter to an event.  This is not recommended.");
#endif
                }
                if (o == null) continue;
                list.Add(o);
            }

            method.Invoke(sceneManager, list.ToArray());
            //result.Component = controller;
            //result.Execute();
        }
        else
        {
            method.Invoke(sceneManager, additionalParameters);
        }
    }


#endif


};


/// <summary>
/// Future name of controller.
/// </summary>
public abstract class ElementService : Controller
{
}
#if DLL
}
#endif
public class UFToggleGroup : Attribute
{
    public UFToggleGroup(string checkers)
    {
        Name = checkers;
    }

    public string Name { get; set; }
}

public class UFRequireInstanceMethod : Attribute
{
    public UFRequireInstanceMethod(string canmovetochanged)
    {
        MethodName = canmovetochanged;
    }

    public string MethodName { get; set; }
}

public class UFGroup : Attribute
{
    public UFGroup(string viewModelProperties)
    {
        Name = viewModelProperties;
    }

    public string Name { get; set; }
}