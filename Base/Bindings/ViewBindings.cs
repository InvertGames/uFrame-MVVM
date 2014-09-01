using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Binding extension method that make it easy to bind ViewModels to Views
/// </summary>
public static class ViewBindings
{
    /// <summary>
    /// Bind to a ViewModel collection.
    /// </summary>
    /// <typeparam name="TCollectionItemType">The type that the collection contains.</typeparam>
    /// <param name="t">This</param>
    /// <param name="collectionSelector">Select a model collection.</param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    public static ModelCollectionBinding<TCollectionItemType> BindCollection<TCollectionItemType>(this ViewModel t, Func<ModelCollection<TCollectionItemType>> collectionSelector)
    {
        var binding = new ModelCollectionBinding<TCollectionItemType>()
        {
            Source = t,
            ModelPropertySelector = () => collectionSelector(),
        };
      
        t.AddBinding(binding);
        return binding;
    }
    /// <summary>
    /// Bind to a ViewModel collection.
    /// </summary>
    /// <typeparam name="TCollectionItemType">The type that the collection contains.</typeparam>
    /// <param name="t">This</param>
    /// <param name="collectionSelector">Select a model collection.</param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    public static ModelCollectionBinding<TCollectionItemType> BindCollection<TCollectionItemType>(this ViewBase t, Func<ModelCollection<TCollectionItemType>> collectionSelector)
    {
        var binding = new ModelCollectionBinding<TCollectionItemType>()
        {
            Source = t.ViewModelObject,
            ModelPropertySelector = () => collectionSelector(),
        };

        t.AddBinding(binding);
        return binding;
    }
    /// <summary>
    /// Bind a Unity Collision event to a ViewModel command.
    /// </summary>
    /// <param name="t">The view that owns the binding</param>
    /// <param name="commandSelector">A function that will select the command from a ViewModel.</param>
    /// <param name="eventType">The collision event to bind to.</param>
    /// <returns>The collision binding class that allows chaining extra options.</returns>
    public static ModelCollisionEventBinding BindCollision(this ViewBase t, Func<ICommand> commandSelector, CollisionEventType eventType)
    {
        if (commandSelector == null)
            throw new ArgumentNullException("commandSelector");

        var kb = t.gameObject.AddComponent<CollisionEventBinding>();
        kb._SourceView = t;
        kb._CollisionEvent = eventType;
        kb.hideFlags = HideFlags.HideInInspector;
        var binding = kb.Binding as ModelCollisionEventBinding;

        binding.Source = t.ViewModelObject;
        binding.CommandDelegate = commandSelector;
        binding.Component = kb;
        //binding.Predicate = func;

        t.AddBinding(binding);
        return binding;
    }

    ///// <summary>
    ///// Bind a string named event.
    ///// </summary>
    ///// <param name="t">The view that owns the binding</param>
    ///// <param name="commandSelector"></param>
    ///// <param name="eventName"></param>
    ///// <returns>The event binding class that allows chaining extra options.</returns>
    //public static ModelEventBinding BindEvent(this ViewBase t, Func<ICommand> commandSelector, string eventName)
    //{
    //    var eventBinding = new ModelEventBinding(eventName)
    //    {
    //        Source = t,
    //        CommandDelegate = commandSelector
    //    };
    //    t.AddBinding(eventBinding);
    //    return eventBinding;
    //}

    /// <summary>
    /// Bind a input button to a ViewModel Command
    /// </summary>
    /// <param name="t">The view that owns the binding</param>
    /// <param name="commandSelector">The command to bind the input to</param>
    /// <param name="buttonName">The name of the input button to bind to.</param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    public static ModelInputButtonBinding BindInputButton(this ViewBase t, Func<ICommand> commandSelector, string buttonName, InputButtonEventType buttonEventType = InputButtonEventType.ButtonDown)
    {
        if (commandSelector == null) throw new ArgumentNullException("commandSelector");
        var kb = t.gameObject.AddComponent<InputBinding>();
        kb._ButtonName = buttonName;
        kb._EventType = buttonEventType;
        kb.hideFlags = HideFlags.HideInInspector;
        var binding = kb.Binding as ModelInputButtonBinding;

        binding.Source = t.ViewModelObject;
        binding.CommandDelegate = commandSelector;
        binding.Component = kb;

        t.AddBinding(binding);
        return binding;
    }

    /// <summary>
    /// Bind a key to a ViewModel Command
    /// </summary>
    /// <param name="t">The view that owns the binding</param>
    /// <param name="commandSelector"></param>
    /// <param name="key"></param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    public static ModelKeyBinding BindKey(this ViewBase t, Func<ICommand> commandSelector, KeyCode key, object parameter = null)
    {
        if (commandSelector == null) throw new ArgumentNullException("commandSelector");
        var kb = t.gameObject.AddComponent<KeyBinding>();
        kb._Key = key;
        kb.hideFlags = HideFlags.HideInInspector;
        var binding = kb.Binding as ModelKeyBinding;

        binding.Source = t.ViewModelObject;
        binding.CommandDelegate = commandSelector;
        binding.Component = kb;
        
        if (parameter != null)
        {
            binding.SetParameter(parameter);
        }

        t.AddBinding(binding);
        return binding;
    }

    /// <summary>
    /// Binds a mouse event to a ViewModel Command.
    /// </summary>
    /// <param name="view">The view that will own the Binding.</param>
    /// <param name="commandSelector">ICommand selector</param>
    /// <param name="eventType">The mouse event to bind to.</param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    public static ModelMouseEventBinding BindMouseEvent(this ViewBase view, Func<ICommand> commandSelector, MouseEventType eventType)
    {
        if (commandSelector == null) throw new ArgumentNullException("commandSelector");
        var kb = view.gameObject.AddComponent<MouseEventBinding>();
        kb._EventType = eventType;
        kb._SourceView = view;
        kb.hideFlags = HideFlags.HideInInspector;
        var binding = kb.Binding as ModelMouseEventBinding;

        binding.Source = view.ViewModelObject;
        binding.CommandDelegate = commandSelector;
        binding.Component = kb;

        view.AddBinding(binding);
        return binding;
    }
    ///// <summary>
    ///// The binding class that allows chaining extra options.
    ///// </summary>
    ///// <typeparam name="TBindingType">The type of the model property to bind to.</typeparam>
    ///// <param name="t">The view that owns the binding</param>
    ///// <param name="sourceProperty">The ViewModel property to bind to. Ex. ()=>Model.MyViewModelProperty</param>
    ///// <param name="targetSetter">Should set the value of the target.</param>
    ///// <returns>The binding class that allows chaining extra options.</returns>
    //public static ModelPropertyBinding BindProperty<TBindingType>(this ViewModel t, Func<P<TBindingType>> sourceProperty, Action<TBindingType> targetSetter)
    //{
    //    var binding = new ModelPropertyBinding()
    //    {
          
    //        SetTargetValueDelegate = (o) => targetSetter((TBindingType)o),
    //        ModelPropertySelector = () => (ModelPropertyBase)sourceProperty(),
    //        TwoWay = false
    //    };

    //    t.AddBinding(binding);

    //    return binding;
    //}



    /// <summary>
    /// The binding class that allows chaining extra options.
    /// </summary>
    /// <typeparam name="TBindingType">The type of the model property to bind to.</typeparam>
    /// <param name="t">The view that owns the binding</param>
    /// <param name="sourceProperty">The ViewModel property to bind to. Ex. ()=>Model.MyViewModelProperty</param>
    /// <param name="targetSetter">Should set the value of the target.</param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    public static ModelPropertyBinding BindProperty<TBindingType>(this ViewBase t, Func<P<TBindingType>> sourceProperty, Action<TBindingType> targetSetter)
    {
        var binding = new ModelPropertyBinding()
        {
            Source = t.ViewModelObject,
            SetTargetValueDelegate = (o) => targetSetter((TBindingType)o),
            ModelPropertySelector = () => (ModelPropertyBase)sourceProperty(),
            TwoWay = false
        };

        t.AddBinding(binding);

        return binding;
    }

    /// <summary>
    /// The binding class that allows chaining extra options.
    /// </summary>
    /// <typeparam name="TBindingType">The type of the model property to bind to.</typeparam>
    /// <param name="t">The view that owns the binding</param>
    /// <param name="sourceProperty">The ViewModel property to bind to. Ex. ()=>Model.MyViewModelProperty</param>
    /// <param name="targetSetter">Should set the value of the target.</param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    public static ModelPropertyBinding BindProperty<TBindingType>(this ViewBase t, Func<Computed<TBindingType>> sourceProperty, Action<TBindingType> targetSetter)
    {
        var binding = new ModelPropertyBinding()
        {
            Source = t.ViewModelObject,
            SetTargetValueDelegate = (o) => targetSetter((TBindingType)o),
            ModelPropertySelector = () => (ModelPropertyBase)sourceProperty(),
            TwoWay = false
        };

        t.AddBinding(binding);

        return binding;
    }

    /// <summary>
    /// A Two-Way binding to a ViewModel Property.
    /// </summary>
    /// <typeparam name="TBindingType"></typeparam>
    /// <param name="t">The view that owns the binding</param>
    /// <param name="sourceProperty">The ViewModel property to bind to. Ex. ()=>Model.MyViewModelProperty</param>
    /// <param name="targetSetter">Should set the value of the target.</param>
    /// <param name="twoWayGetter">Should return the value of the target.</param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    public static ModelPropertyBinding BindProperty<TBindingType>(this ViewBase t, Func<P<TBindingType>> sourceProperty, Action<TBindingType> targetSetter, Func<TBindingType> twoWayGetter)
    {
        var binding = new ModelPropertyBinding()
        {
            Source = t.ViewModelObject,
            SetTargetValueDelegate = (o) => targetSetter((TBindingType)o),
            ModelPropertySelector = () => (ModelPropertyBase)sourceProperty(),
            TwoWay = twoWayGetter != null
        };
        if (twoWayGetter != null)
        {
            binding.GetTargetValueDelegate = () => twoWayGetter();
        }
        t.AddBinding(binding);

        return binding;
    }

    /// <summary>
    /// Bind a ViewModel Collection to a View Collection.
    /// </summary>
    /// <typeparam name="TView">The view that owns the binding</typeparam>
    /// <typeparam name="TViewModelType"></typeparam>
    /// <param name="t"></param>
    /// <param name="sourceViewModelCollection"></param>
    /// <param name="viewCollection">The view collection is a list of ICollection that can be used to keep track of the Views created from the ViewModel Collection.</param>
    /// <param name="viewFirst"></param>
    /// <returns>The collection binding class that allows chaining extra options.</returns>
    public static ModelViewModelCollectionBinding BindToViewCollection<TView, TViewModelType>(
        this ViewBase t,
        Func<ModelCollection<TViewModelType>> sourceViewModelCollection,
        ICollection<TView> viewCollection, bool viewFirst = false
        )
        where TView : ViewBase
        where TViewModelType : ViewModel
    {
        var binding = new ModelViewModelCollectionBinding()
        {
            Source = t.ViewModelObject,
            SourceView = t,
            ModelPropertySelector = () => sourceViewModelCollection() as ModelPropertyBase
        }
        .SetAddHandler(v => viewCollection.Add(v as TView))
        .SetRemoveHandler(v => viewCollection.Remove(v as TView));

        if (viewFirst)
        {
            binding.ViewFirst();
        }
        t.AddBinding(binding);
        return binding;
    }

    /// <summary>
    /// The binding class that allows chaining extra options.
    /// </summary>
    /// <typeparam name="TBindingType">The type of the model property to bind to.</typeparam>
    /// <param name="t">The view that owns the binding</param>
    /// <param name="sourceViewModelSelector">Selector for the ViewModel Property</param>
    /// <param name="setLocal">Set a local variable on your view to store the bound view.</param>
    /// <param name="getLocal">Get the local variable on your view used in this binding.</param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    public static ModelViewPropertyBinding BindToView<TBindingType>(this ViewBase t, Func<P<TBindingType>> sourceViewModelSelector, Action<ViewBase> setLocal = null, Func<ViewBase> getLocal = null)
    where TBindingType : ViewModel
    {
        var binding = new ModelViewPropertyBinding()
        {
            Source = t.ViewModelObject,
            SourceView = t,
            ModelPropertySelector = () => (ModelPropertyBase)sourceViewModelSelector(),
            TwoWay = false
        };
        if (getLocal != null)
        {
            binding.GetTargetValueDelegate = () => getLocal();
            if (setLocal == null)
                throw new Exception("When using a BindToView you must set the setLocal parameter and getLocal parameter.");
            binding.SetTargetValueDelegate = (o) => setLocal((ViewBase)o);
        }

        t.AddBinding(binding);

        return binding;
    }

    /// <summary>
    /// Bind a ViewModel Collection
    /// </summary>
    /// <typeparam name="TCollectionType"></typeparam>
    /// <param name="t">The view that owns the binding</param>
    /// <param name="viewModelCollection">The view collection is a list of ICollection that can be used to keep track of the Views created from the ViewModel Collection.</param>
    /// <param name="viewFirst">Should the collection be initialized from the view. If false the View will be initialized to the ViewModel.</param>
    /// <returns>The Collection Binding class that allows chaining extra options.</returns>
    public static ModelViewModelCollectionBinding BindToViewCollection<TCollectionType>(this ViewBase t,
        Func<ModelCollection<TCollectionType>> viewModelCollection, bool viewFirst = false)
    {
        var binding = new ModelViewModelCollectionBinding()
        {
            Source = t.ViewModelObject,
            ModelPropertySelector = () => viewModelCollection()
        };
        if (viewFirst)
        {
            binding.ViewFirst();
        }
        t.AddBinding(binding);
        return binding;
    }

    /// <summary>
    /// Subscribes to the property and returns an action to unsubscribe.
    /// </summary>
    /// <typeparam name="TBindingType"></typeparam>
    /// <param name="modelProperty">The ViewModel Property to bind to.</param>
    /// <param name="onChange">When the property has changed.</param>
    /// <returns>An action to will unsubsribe.</returns>
    public static Action Subscribe<TBindingType>(this IViewModelObserver behaviour, P<TBindingType> modelProperty, Action<TBindingType> onChange)
    {
        var action = new ModelPropertyBase.PropertyChangedHandler(value => onChange((TBindingType)value));
        modelProperty.ValueChanged += action;
        return () => modelProperty.ValueChanged -= action;
    }

    /// <summary>
    /// Subscribes to a command execution.
    /// </summary>
    /// <param name="view">The view.</param>
    /// <param name="command">The command to subscribe to.</param>
    /// <param name="executed">When the command is executed then this will be executed.</param>
    /// <returns>An action that will unsubscribe</returns>
    public static Action Subscribe(this IViewModelObserver view, Func<ICommand> command, Action executed)
    {
        var c = command();
        var e = new CommandEvent(executed);
        c.OnCommandExecuting += e;
        return () => c.OnCommandExecuted -= e;
    }
}