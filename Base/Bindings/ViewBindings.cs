using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using UniRx;
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
        binding.Bind();
        return binding;
    }

    /// <summary>
    /// Bind to a ViewModel collection.
    /// </summary>
    /// <typeparam name="TCollectionItemType">The type that the collection contains.</typeparam>
    /// <param name="t">This</param>
    /// <param name="collection">The Model Collection to bind to</param>
    /// <param name="added"></param>
    /// <param name="removed"></param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    public static IDisposable BindCollection<TCollectionItemType>(this ViewBase t, ObservableCollection<TCollectionItemType> collection, Action<TCollectionItemType> added, Action<TCollectionItemType> removed)
    {
        NotifyCollectionChangedEventHandler collectionChanged = delegate(NotifyCollectionChangedEventArgs args)
         {
             if (added != null && args.NewItems != null)
                 foreach (var item in args.NewItems)
                     added((TCollectionItemType)item);

             if (removed != null && args.OldItems != null)
                 foreach (var item in args.OldItems)
                     removed((TCollectionItemType)item);
         };

        collection.CollectionChanged += collectionChanged;
        collectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection.ToArray()));
        return t.AddBinding(Disposable.Create(() => collection.CollectionChanged -= collectionChanged));
    }




    /// <summary>
    /// Bind to a ViewModel collection.
    /// </summary>
    /// <typeparam name="TCollectionItemType">The type that the collection contains.</typeparam>
    /// <param name="t">This</param>
    /// <param name="collectionSelector">Select a model collection.</param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    [Obsolete]
    public static ModelCollectionBinding<TCollectionItemType> BindCollection<TCollectionItemType>(this ViewBase t, Func<ModelCollection<TCollectionItemType>> collectionSelector)
    {
        var binding = new ModelCollectionBinding<TCollectionItemType>()
        {
            Source = t.ViewModelObject,
            ModelPropertySelector = () => collectionSelector(),
        };
        t.AddBinding(binding);
        binding.Bind();
        return binding;
    }

    public static IDisposable BindCollision(this ViewBase t, CollisionEventType eventType, Action<Collision> action)
    {
        return t.AddBinding(OnCollisionObservable(t.gameObject, eventType).Subscribe(action));
    }


    /// <summary>
    /// Bind a Unity Collision event to a ViewModel command.
    /// </summary>
    /// <param name="t">The view that owns the binding</param>
    /// <param name="eventType">The collision event to bind to.</param>
    /// <returns>The collision binding class that allows chaining extra options.</returns>
    public static IObservable<Collision> OnCollisionObservable(this GameObject t, CollisionEventType eventType)
    {
        if (eventType == CollisionEventType.OnCollisionEnter)
        {

            return t.EnsureComponent<ObservableCollisionEnterBehaviour>().OnCollisionEnterAsObservable();
        }
        else if (eventType == CollisionEventType.OnCollisionExit)
        {
            return t.EnsureComponent<ObservableCollisionExitBehaviour>().OnCollisionExitAsObservable();
        }
        else
        {
            return t.EnsureComponent<ObservableCollisionStayBehaviour>().OnCollisionStayAsObservable();
        }
    }

    public static T EnsureComponent<T>(this ViewBase t) where T : Component
    {
        return t.GetComponent<T>() ?? t.gameObject.AddComponent<T>();
    }
    public static T EnsureComponent<T>(this GameObject t) where T : Component
    {
        return t.GetComponent<T>() ?? t.AddComponent<T>();
    }

    public static IDisposable BindViewCollision(this ViewBase t, CollisionEventType eventType, Action<ViewBase> collision)
    {
        return t.AddBinding(OnViewCollision(t.gameObject, eventType).Subscribe(collision));
    }
    public static IDisposable BindViewCollisionWith<T>(this ViewBase t, CollisionEventType eventType, Action<T> collision) where T : ViewBase
    {
        return t.AddBinding(OnViewCollisionWith<T>(t.gameObject, eventType).Subscribe(collision));
    }
    public static IObservable<ViewBase> OnViewCollision(this GameObject t, CollisionEventType eventType)
    {
        return OnCollisionObservable(t, eventType).Where(p => p.GetView() != null).Select(p => p.GetView());
    }

    public static IObservable<T> OnViewCollisionWith<T>(this GameObject t, CollisionEventType eventType) where T : ViewBase
    {
        return OnCollisionObservable(t, eventType).Where(p => p.GetView<T>() != null).Select(p => p.GetView<T>());
    }

    /// <summary>
    /// Bind a input button to a ViewModel Command
    /// </summary>
    /// <param name="t">The view that owns the binding</param>
    /// <param name="commandSelector">The command to bind the input to</param>
    /// <param name="buttonName">The name of the input button to bind to.</param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    public static IDisposable BindInputButton(this ViewBase t, Func<ICommand> commandSelector, string buttonName, InputButtonEventType buttonEventType = InputButtonEventType.ButtonDown)
    {
        if (buttonEventType == InputButtonEventType.Button)
        {
            return t.AddBinding(t.UpdateAsObservable().Where(p => Input.GetButton(buttonName)).Subscribe(_ => t.ExecuteCommand(commandSelector())));
        }
        else if (buttonEventType == InputButtonEventType.ButtonDown)
        {
            return t.AddBinding(t.UpdateAsObservable().Where(p => Input.GetButtonDown(buttonName)).Subscribe(_ => t.ExecuteCommand(commandSelector())));
        }

        return t.AddBinding(t.UpdateAsObservable().Where(p => Input.GetButtonUp(buttonName)).Subscribe(_ => t.ExecuteCommand(commandSelector())));
    }

    /// <summary>
    /// Bind a key to a ViewModel Command
    /// </summary>
    /// <param name="t">The view that owns the binding</param>
    /// <param name="commandSelector"></param>
    /// <param name="key"></param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    public static IDisposable BindKey(this ViewBase t, Func<ICommand> commandSelector, KeyCode key, object parameter = null)
    {
        return t.AddBinding(t.UpdateAsObservable().Where(p => Input.GetKey(key)).Subscribe(_ => t.ExecuteCommand(commandSelector(), parameter)));
    }

    /// <summary>
    /// Binds a mouse event to a ViewModel Command.
    /// </summary>
    /// <param name="view">The view that will own the Binding.</param>
    /// <param name="commandSelector">ICommand selector</param>
    /// <param name="eventType">The mouse event to bind to.</param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    public static IObservable<Unit> OnMouseEvent(this ViewBase view, Func<ICommand> commandSelector, MouseEventType eventType)
    {
        if (eventType == MouseEventType.OnMouseDown)
        {
            var component = view.AddComponentBinding<ObservableMouseDownBehaviour>();
            return component.OnMouseDownAsObservable();
        }
        else if (eventType == MouseEventType.OnMouseDrag)
        {
            var component = view.AddComponentBinding<ObservableMouseDragBehaviour>();
            return component.OnMouseDragAsObservable();
        }
        else if (eventType == MouseEventType.OnMouseEnter)
        {
            var component = view.AddComponentBinding<ObservableMouseEnterBehaviour>();
            return component.OnMouseEnterAsObservable();
        }
        else if (eventType == MouseEventType.OnMouseExit)
        {
            var component = view.AddComponentBinding<ObservableMouseExitBehaviour>();
            return component.OnMouseExitAsObservable();

        }
        else if (eventType == MouseEventType.OnMouseOver)
        {
            var component = view.AddComponentBinding<ObservableMouseOverBehaviour>();
            return component.OnMouseOverAsObservable();
        }
        return view.AddComponentBinding<ObservableMouseOverBehaviour>().OnMouseOverAsObservable();
    }

    public static IDisposable BindProperty<TBindingType>(this ViewBase t, P<TBindingType> sourceProperty, Action<TBindingType> targetSetter, bool onlyWhenChanged = true)
    {

        if (onlyWhenChanged)
        {
            return t.AddBinding(sourceProperty.Where(p => sourceProperty.LastValue != sourceProperty.ObjectValue).Subscribe(targetSetter));
        }

        return t.AddBinding(sourceProperty.Subscribe(targetSetter));
    }

    /// <summary>
    /// The binding class that allows chaining extra options.
    /// </summary>
    /// <typeparam name="TBindingType">The type of the model property to bind to.</typeparam>
    /// <param name="t">The view that owns the binding</param>
    /// <param name="sourceProperty">The ViewModel property to bind to. Ex. ()=>Model.MyViewModelProperty</param>
    /// <param name="targetSetter">Should set the value of the target.</param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    [Obsolete("Use other overload without function selector.")]
    public static IDisposable BindProperty<TBindingType>(this ViewBase t, Func<P<TBindingType>> sourceProperty, Action<TBindingType> targetSetter)
    {
        return t.AddBinding(sourceProperty().Subscribe(targetSetter));
    }

    /// <summary>
    /// The binding class that allows chaining extra options.
    /// </summary>
    /// <typeparam name="TBindingType">The type of the model property to bind to.</typeparam>
    /// <param name="t">The view that owns the binding</param>
    /// <param name="sourceProperty">The ViewModel property to bind to. Ex. ()=>Model.MyViewModelProperty</param>
    /// <param name="targetSetter">Should set the value of the target.</param>
    /// <returns>The binding class that allows chaining extra options.</returns>
    [Obsolete("Use other overload without function selector.")]
    public static IDisposable BindProperty<TBindingType>(this ViewBase t, Func<Computed<TBindingType>> sourceProperty, Action<TBindingType> targetSetter)
    {
        return t.AddBinding(sourceProperty().Subscribe(targetSetter));
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
    [Obsolete("User other bindings, or regenerate this code.")]
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
            ModelPropertySelector = () => sourceViewModelCollection() as IObservableProperty
        }
        .SetAddHandler(v => viewCollection.Add(v as TView))
        .SetRemoveHandler(v => viewCollection.Remove(v as TView));

        if (viewFirst)
        {
            binding.ViewFirst();
        }
        t.AddBinding(binding);
        binding.Bind();
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
    [Obsolete]
    public static ModelViewPropertyBinding BindToView<TBindingType>(this ViewBase t, Func<P<TBindingType>> sourceViewModelSelector, Action<ViewBase> setLocal = null, Func<ViewBase> getLocal = null)
    where TBindingType : ViewModel
    {
        var binding = new ModelViewPropertyBinding()
        {
            Source = t.ViewModelObject,
            SourceView = t,
            ModelPropertySelector = () => (IObservableProperty)sourceViewModelSelector(),
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
        binding.Bind();
        return binding;
    }

    public static ModelViewModelCollectionBinding BindToViewCollection<TCollectionType>(this ViewBase t,
        Func<ModelCollection<TCollectionType>> viewModelCollection, Func<ViewModel,
        ViewBase> createView,
        Action<ViewBase> added,
        Action<ViewBase> removed,
        Transform parent,
        bool viewFirst = false)
    {
        var binding = new ModelViewModelCollectionBinding()
        {
            Source = t.ViewModelObject,
            SourceView = t,
            ModelPropertySelector = () => viewModelCollection()
        };
        binding.SetParent(parent);
        binding.SetAddHandler(added);
        binding.SetRemoveHandler(added);
        binding.SetCreateHandler(createView);
        t.AddBinding(binding);
        if (viewFirst)
        {
            binding.ViewFirst();
        }
        binding.Bind();

        return binding;
    }

    public static ModelViewModelCollectionBinding BindToViewCollection<TCollectionType>(this ViewBase t,
        ModelCollection<TCollectionType> viewModelCollection, Func<ViewModel,
        ViewBase> createView,
        Action<ViewBase> added,
        Action<ViewBase> removed,
        Transform parent,
        bool viewFirst = false)
    {
        var binding = new ModelViewModelCollectionBinding()
        {
            Source = t.ViewModelObject,
            SourceView = t,
            ModelPropertySelector = () => viewModelCollection
        };
        binding.SetParent(parent);
        binding.SetAddHandler(added);
        binding.SetRemoveHandler(added);
        binding.SetCreateHandler(createView);
        if (viewFirst)
        {
            binding.ViewFirst();
        }
        t.AddBinding(binding);
        binding.Bind();

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
    [Obsolete]
    public static ModelViewModelCollectionBinding BindToViewCollection<TCollectionType>(this ViewBase t,
        Func<ModelCollection<TCollectionType>> viewModelCollection, bool viewFirst = false)
    {
        var binding = new ModelViewModelCollectionBinding()
        {
            Source = t.ViewModelObject,
            SourceView = t,
            ModelPropertySelector = () => viewModelCollection()
        };
        if (viewFirst)
        {
            binding.ViewFirst();
        }
        t.AddBinding(binding);
        return binding;
    }



}

public enum InputButtonEventType
{
    Button,
    ButtonDown,
    ButtonUp
}