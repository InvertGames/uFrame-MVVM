using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//using UnityEngine;
//using Debug = UnityEngine.Debug;

/// <summary>
/// A controller is a integral part of uFrame and is used for an extra layer connecting services and "Elements" of a
/// game together.
/// </summary>
public abstract class Controller : IViewModelObserver
{
    private List<IBinding> _bindings;
    private string _typeName;
    private SceneContext _context;

  

    public List<IBinding> Bindings
    {
        get { return _bindings ?? (_bindings = new List<IBinding>()); }
        set { _bindings = value; }
    }

    public IGameContainer Container
    {
        get
        {
            return GameManager.Container;
        }
    }

    /// <summary>
    /// The friendly name of the controller.
    /// If this' type name ends with controller it will be removed.
    /// </summary>
    public string ControllerName
    {
        get
        {
            if (_typeName == null)
            {
                var typeName = this.GetType().Name;
                if (typeName.Length > 10 && typeName.ToLower().EndsWith("controller"))
                {
                    _typeName = typeName.Substring(0, typeName.Length - 10);
                }
            }
            return _typeName;
        }
    }

    public void AddBinding(IBinding binding)
    {
        Bindings.Add(binding);
        // This is key for controllers.  We want to bind it immediately
        binding.Bind();
    }

    //public virtual ViewModel Create(string identifier)
    //{
    //    var vm = CreateEmpty();
    //    WireCommands(vm);
    //    LoadByIdentifier(identifier, vm);
    //    return vm;
    //}

    protected Controller()
    {
        //throw new Exception("Default constructor is not allowed.  Please regenerate your diagram or create the controller with a SceneContext.");
    }

    protected Controller(SceneContext context)
    {
        Context = context;
        Context.Container.RegisterInstance(this.GetType(),this,false);
    }

    public SceneContext Context
    {
        get { return _context ?? (_context = GameManager.ActiveSceneManager.Context); }
        set { _context = value; }
    }

    public virtual ViewModel Create()
    {
        return Create(Guid.NewGuid().ToString());
    }
    public virtual ViewModel CreateEmpty(string identifier)
    {
        var vm = CreateEmpty();
        vm.Identifier = identifier;
        Context[identifier] = vm;
        return vm;
        return new FPSMenuViewModel() { Identifier = identifier };
    }
    public virtual ViewModel Create(string identifier)
    {
        var vm = Context[identifier];

        if (vm == null)
        {
            vm = CreateEmpty(identifier);
            InitializeInternal(vm);
        }
        return vm;
    }

    public virtual ViewModel Create(string identifier, Action<ViewModel> preInitializer)
    {
        var vm = Context[identifier];
        
        if (vm == null)
        {
            vm = CreateEmpty(identifier);
            InitializeInternal(vm, preInitializer);
            return vm;
        }
        if (preInitializer != null)
        {
            InitializeInternal(vm,preInitializer);
        }
        return vm;
    }

    public virtual ViewModel CreateEmpty()
    {
        throw new NotImplementedException("You propably need to resave you're diagram. Or you need to not call create on an abstract controller.");
    }

    public void ExecuteCommand(ICommand command, object argument)
    {
        if (command == null) return;
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
        if (command == null) return;
        //command.Sender = null;
        command.Parameter = null;

        IEnumerator enumerator = command.Execute();
        if (enumerator != null)
            StartCoroutine(enumerator);
    }

    public void ExecuteCommand<TArgument>(ICommandWith<TArgument> command, TArgument argument)
    {
        if (command == null) return;
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

    public virtual ViewModel GetByName(string resolveName, bool initialize = true, Action<ViewModel> preInitializer = null)
    {
        if (string.IsNullOrEmpty(resolveName))
            throw new Exception("GetByName on a controller can't be called with a null or empty resolve name.");

        var viewModel = ResolveByName(resolveName);
        if (viewModel == null)
        {
            viewModel = CreateEmpty();
            Container.RegisterInstance(viewModel, resolveName);
            Container.RegisterInstance(viewModel.GetType(), viewModel, resolveName);
        }

        if (initialize)
            InitializeInternal(viewModel, preInitializer);

        return viewModel;
    }

    public virtual ViewModel GetByType(string identifier, Type viewModelType, bool initialize = true, Action<ViewModel> preInitializer = null)
    {
        var viewModel = GameManager.Container.Resolve(viewModelType, null, true) as ViewModel;

        if (viewModel == null)
        {
            var contextViewModel = Context[identifier];
            if (contextViewModel == null)
            {
                contextViewModel = CreateEmpty();
                contextViewModel.Identifier = identifier;
                Context[identifier] = contextViewModel;
            }
            
            Container.RegisterInstance(viewModelType, contextViewModel);

            if (initialize)
            {
                InitializeInternal(contextViewModel,preInitializer);
            }

            return contextViewModel;
        }
        else
        {
            var contextViewModel = Context[identifier];
            // Ensure the view model exists in the context
            if (contextViewModel == null)
            {
                Context[identifier] = viewModel;
            }
            viewModel.Identifier = identifier;
        }
        if (initialize)
        {
            InitializeInternal(viewModel, preInitializer);
        }
        
        return viewModel;
    }

    public abstract void Initialize(ViewModel viewModel);

    public virtual void LoadByIdentifier(string identifier, ViewModel viewModel)
    {
    }

    public void RemoveBinding(IBinding binding)
    {
        Bindings.Remove(binding);
    }

    [Obsolete("No longer needed.  Use inject")]
    public virtual void Setup(IGameContainer container)
    {
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

    public ModelPropertyBinding SubscribeToProperty<TViewModel, TBindingType>(TViewModel source, P<TBindingType> sourceProperty, Action<TViewModel, TBindingType> changedAction)
    {
        var binding = new ModelPropertyBinding()
        {
            SetTargetValueDelegate = (o) => changedAction(source, (TBindingType)o),
            ModelPropertySelector = () => sourceProperty,
            IsImmediate = false
        };
        AddBinding(binding);
        return binding;
    }

    public ModelPropertyBinding SubscribeToProperty<TBindingType>(P<TBindingType> sourceProperty, Action<TBindingType> targetSetter)
    {
        var binding = new ModelPropertyBinding()
        {
            SetTargetValueDelegate = (o) => targetSetter((TBindingType)o),
            ModelPropertySelector = () => (ModelPropertyBase)sourceProperty,
            IsImmediate = false
        };
        AddBinding(binding);
        return binding;
    }

    public void Unbind()
    {
        foreach (var binding in Bindings)
        {
            binding.Unbind();
        }

        // Remove all the bindings that are not from a component
        Bindings.RemoveAll(p => !p.IsComponent);
    }

    public abstract void WireCommands(ViewModel viewModel);

    protected virtual ViewModel ResolveByName(string resolveName)
    {
        return GameManager.Container.Resolve<ViewModel>(resolveName);
    }

    protected void SubscribeToCommand(ICommand command, Action action)
    {
        command.OnCommandExecuted += () => action();
    }

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
        var controller = GameManager.ActiveSceneManager;
        if (controller == null)
        {
            throw new Exception("Controller is not set.");
        }
        var method = controller.GetType().GetMethod(message);

        if (method == null)
        {
            throw new Exception(string.Format("Event '{0}' was not found on {1}", message, controller));
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

            method.Invoke(controller, list.ToArray());
            //result.Component = controller;
            //result.Execute();
        }
        else
        {
            method.Invoke(controller, additionalParameters);
        }
    }

#if !DLL
#endif

    private void InitializeInternal(ViewModel viewModel, Action<ViewModel> preInitializer = null)
    {
        if (preInitializer != null)
        {
            preInitializer(viewModel);
        }
        WireCommands(viewModel);
        Initialize(viewModel);
    }
};