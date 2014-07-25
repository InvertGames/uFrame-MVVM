using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using UnityEngine;

/// <summary>
/// The base class for a View that binds to a ViewModel
/// </summary>
public abstract class ViewBase : ViewContainer,IViewModelObserver
{
    
    /// <summary>
    /// The View Event delegate that takes a string for the event name.
    /// </summary>
    /// <param name="eventName">The event that has occured.</param>
    public delegate void ViewEvent(string eventName);

    /// <summary>
    /// An event that is invoked whe calling Event("MyEvent")
    /// </summary>
    public event ViewEvent EventTriggered;

    /// <summary>
    /// Should we log an event for each View event that occurs.
    /// </summary>
    [HideInInspector]
    public bool _LogEvents;

    private List<IBindingProvider> _bindingProviders;

    private bool _bound = false;

    ///// <summary>
    ///// Where should the viewmodel come from or how should it be instantiated.
    ///// </summary>
    //[HideInInspector]
    //public ViewModelRegistryType _ViewModelFrom = ViewModelRegistryType.ResolveInstance;
    private List<ViewBase> _children;

    [SerializeField, HideInInspector, UFGroup("View Model Properties")]
    private bool _forceResolveViewModel = false;

    //[HideInInspector]
    //public string _ViewModelControllerType;
    [HideInInspector]
    private ViewModel _Model;

    [SerializeField, HideInInspector, UFGroup("View Model Properties")]
    private bool _overrideViewModel;

    //[HideInInspector]
    //public string _ViewModelControllerMethod;
    private ViewBase _parentView;

    [SerializeField, HideInInspector, UFGroup("View Model Properties")]
    private string _resolveName = null;

    [NonSerialized]
    private bool _shouldRebindOnEnable = false;

    public List<IBindingProvider> BindingProviders
    {
        get { return _bindingProviders ?? (_bindingProviders = new List<IBindingProvider>()); }
        set { _bindingProviders = value; }
    }

    public IEnumerable<ViewModel> ChildViewModels
    {
        get
        {
            return ChildViews.Select(p => p.ViewModelObject);
        }
    }

    public List<ViewBase> ChildViews
    {
        get { return _children ?? (_children = new List<ViewBase>()); }
        set { _children = value; }
    }

    public bool ForceResolveViewModel
    {
        get { return _forceResolveViewModel; }
        set { _forceResolveViewModel = value; }
    }

    public bool Instantiated { get; set; }

    public virtual bool IsMultiInstance
    {
        get
        {
            return true;
        }
    }

    public bool OverrideViewModel
    {
        get { return _overrideViewModel; }
        set { _overrideViewModel = value; }
    }

    public ViewBase ParentView
    {
        get
        {
            if (_parentView == null)
            {
                if (transform == null) return null;
                if (transform.parent == null) return null;
                _parentView = transform.parent.GetView();
            }
            return _parentView;
        }
        set
        {
            _parentView = value;
        }
    }

    public ViewModel ParentViewModel
    {
        get
        {
            var pv = ParentView;
            if (pv == null) return null;
            return pv.ViewModelObject;
        }
    }

    public virtual ViewModel ViewModelObject
    {
        get
        {
            return _Model ?? (_Model = CreateModel());
        }
        set
        {
            // Skip if its that same
            if (_Model == value) return;

            if (value == null) return;

            _Model = value;
            // Should we rebind?
            if (_bound)
            {
                Unbind();
                SetupBindings();
            }
            //Bind();
        }
    }

    public abstract Type ViewModelType { get; }


    public virtual string DefaultIdentifier
    {
        get
        {
            return ViewModelType.Name;
        }
    }
    /// <summary>
    /// The name of the prefab that created this view
    /// </summary>
    public string ViewName { get; set; }

    //public void AddBinding(IBinding binding)
    //{

    //    base.AddBinding(binding);
    //    // If this view has already been binded invoke the bind method
    //    if (_bound)
    //    {
    //        binding.Bind();
    //    }
    //}

    public virtual void AfterBind()
    {
    }

    public virtual void Awake()
    {
        if (GameManager.ActiveSceneManager != null)
            GameManager.ActiveSceneManager.RegisterRootView(this);
    }

    public abstract void Bind();

    public abstract ViewModel CreateModel();

    /// <summary>
    /// Invoke a .NET event on this view.  This is a convinience method for Event Bindings.
    /// </summary>
    /// <param name="eventname">The name of the event that occured</param>
    public virtual void Event(string eventname)
    {
        if (_LogEvents)
        {
            Debug.Log(string.Format("Event: {0} occured.", eventname), this.gameObject);
        }
        ViewEvent handler = EventTriggered;
        if (handler != null)
            handler(eventname);
    }

    public void ExecuteCommand(ICommand command, object argument)
    {
        if (command == null) return;

        command.Parameter = argument;
        if (command.Parameter == null)
        {
            command.Parameter = this.ViewModelObject;
        }
        IEnumerator enumerator = command.Execute();
        if (enumerator != null)
            StartCoroutine(enumerator);
    }

    public virtual void ExecuteCommand(ICommand command)
    {
        if (command == null) return;

        if (command.Parameter == null)
            command.Parameter = this.ViewModelObject;

        IEnumerator enumerator = command.Execute();
        if (enumerator != null)
            StartCoroutine(enumerator);
    }

    public void ExecuteCommand<TArgument>(ICommandWith<TArgument> command, ViewModel sender, TArgument argument)
    {
        if (command == null) return;
        command.Parameter = argument;
        if (command.Parameter == null)
        {
            command.Parameter = this.ViewModelObject;
        }
        IEnumerator enumerator = command.Execute();
        if (enumerator != null)
            StartCoroutine(enumerator);
    }

    public void ExecuteCommand<TArgument>(ICommandWith<TArgument> command, TArgument argument)
    {
        if (command == null) return;
        command.Parameter = argument;
        if (command.Parameter == null)
        {
            command.Parameter = this.ViewModelObject;
        }
        IEnumerator enumerator = command.Execute();
        if (enumerator != null)
            StartCoroutine(enumerator);
    }

    public void InitializeData(ViewModel model)
    {
        InitializeViewModel(model);
    }

    public virtual void OnDestroy()
    {
        if (ViewModelObject != null)
        {
            ViewModelObject.References--;
            if (ViewModelObject.References < 1)
            {
                ViewModelObject.Unbind();
            }
        }

        var pv = ParentView;
        if (pv != null)
        {
            pv.ChildViews.Remove(this);
        }
        Unbind();
   
        //Bindings.Clear();
    }

    //                return vm;
    //            }
    //            catch (TargetException)
    //            {
    //                Debug.LogError(string.Format("ViewModel can't be created.  You need to add the {0} controller to the scene in order to properly invoke its {1} method. ", _ViewModelControllerType, _ViewModelControllerMethod), this.gameObject);
    //                return null;
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogError(string.Format("The controller method {0} on type {1} was not found either it was renamed or removed. Please fix the view model from property", _ViewModelControllerMethod, _ViewModelControllerType), this.gameObject);
    //            return null;
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError(string.Format("The controller type {0} was not found either it was renamed or removed. Please fix the view model from property", _ViewModelControllerType), this.gameObject);
    //        return null;
    //    }
    //}
    //return Activator.CreateInstance(ViewModelType) as ViewModel;
    //}
    public virtual void OnDisable()
    {
        //var pv = ParentView;
        //if (pv != null)
        //{
        //    pv.ChildViews.Remove(this);
        //}
        //Unbind();
        //_shouldRebindOnEnable = true;
    }

    public virtual void OnEnable()
    {
        if (_shouldRebindOnEnable)
            SetupBindings();
    }

    /// <summary>
    /// This method will setup all bindings on this view.  Bindings don't actually occur on a view until this method is called.
    /// In the bind method it will simply add to the collection of bindings.  You should never have to call this method manually.
    /// </summary>
    public void SetupBindings()
    {
        if (ViewModelObject == null)
        {
            _Model = CreateModel();
        }

        if (_bound)
            return;
        // Initialize the model
        if (ViewModelObject != null) ViewModelObject.References++;
        // Loop through and binding providers and let them add bindings
        foreach (var bindingProvider in BindingProviders)
            bindingProvider.Bind(this);

        // Add any programming bindings
        Bind();
        // Initialize the bindings
        for (var i = 0; i < Bindings.Count; i++)
            Bindings[i].Bind();

        for (var i = 0; i < transform.childCount; i++)
        {
            var view = transform.GetChild(i).GetView();
            if (view == null) continue;
            view.SetupBindings();
        }
        foreach (var childView in ChildViews)
        {
            if (childView.transform != this.transform)
            {
                childView.SetupBindings();
            }
        }

        // Mark this view as bound
        _bound = true;

        AfterBind();
    }

    //{
    public virtual void Start()
    {
        var pv = ParentView;
        if (pv != null)
        {
            pv.ChildViews.Add(this);
        }

        if (ViewModelObject == null)
            _Model = CreateModel();
        // If its instantiated then we dont want to recall
        // the bindings init because instantiate does that already
        if (Instantiated) return;
        if (ParentView == null || ParentView._bound)
        {
            SetupBindings();
        }
    }

    public List<IBinding> Bindings
    {
        get
        {
            if (!ViewModelObject.Bindings.ContainsKey(InstanceId))
            {
                ViewModelObject.Bindings.Add(InstanceId, new List<IBinding>());
            }

            return ViewModelObject.Bindings[InstanceId];
        }
    }

    public int InstanceId
    {
        get
        {
            if (_instanceId == 0)
            {
                _instanceId = this.gameObject.GetInstanceID();
            }
            return _instanceId;
        }
    }
    public void AddBinding(IBinding binding)
    {
        Bindings.Add(binding);
        if (_bound)
        {
            binding.Bind();
        }
        
    }

    public void RemoveBinding(IBinding binding)
    {
        Bindings.Remove(binding);
    }

    /// <summary>
    /// Unbind the current bindings.
    /// </summary>
    public virtual void Unbind()
    {
        //base.Unbind();

        // Loop through and binding providers and let them add bindings
        foreach (var bindingProvider in BindingProviders)
            bindingProvider.Unbind(this);

        _bound = false;
    }

    /// <summary>
    /// This method should be overriden to Initialize the ViewModel
    /// with any options specified in a unity component inspector.
    /// </summary>
    /// <param name="model">The model to initialize.</param>
    protected abstract void InitializeViewModel(ViewModel model);

    protected virtual void LateUpdate()
    {
        Apply();
    }

    protected virtual void Apply()
    {
        
    }
    [SerializeField, HideInInspector]
    private string _identifier;

    private int _instanceId;
    

    public virtual string Identifier
    {
        get
        {
            if (IsMultiInstance && ForceResolveViewModel)
            {
                return _resolveName;
            } else if (!IsMultiInstance && !string.IsNullOrEmpty(_resolveName))
            {
                return _resolveName;
            }
            else if (!IsMultiInstance)
            {
                return DefaultIdentifier;
            }

            //if (!IsMultiInstance)
            //{
            //    return string.Empty;
            //}
            if (string.IsNullOrEmpty(_identifier))
            {
                _identifier = Guid.NewGuid().ToString();
            }
            return _identifier;
        }
        set { _identifier = value; }
    }

    public SceneManager SceneManager
    {
        get { return GameManager.ActiveSceneManager; }
    }

    protected ViewModel RequestViewModel(Controller controller)
    {
        return SceneManager.RequestViewModel(this, controller, Identifier);
        //if (OverrideViewModel)
        //{
        //    if (!IsMultiInstance && string.IsNullOrEmpty(_resolveName))
        //    {
        //        return controller.GetByType(ViewModelType);
        //    }
        //    var result = controller.Create(Identifier, InitializeViewModel);
        //    return result;
        //}
        //else
        //{
        //    if (!IsMultiInstance && string.IsNullOrEmpty(_resolveName))
        //        return controller.GetByType(ViewModelType);

        //    return controller.Create(Identifier);
        //}

        //controller.Create(Identifier, InitializeViewModel);
        if (ForceResolveViewModel || !IsMultiInstance)
        {
            if (string.IsNullOrEmpty(_resolveName))
            {
                if (OverrideViewModel)
                    return controller.GetByType(Identifier, ViewModelType, true, InitializeViewModel);

                return controller.GetByType(Identifier, ViewModelType, false);
            }
            if (OverrideViewModel)
                return controller.Create(Identifier, InitializeViewModel); //controller.Create(InitializeViewModel);
            else
                return controller.Create(Identifier); //controller.Create(InitializeViewModel);
            if (OverrideViewModel)
                return controller.GetByName(_resolveName, true, InitializeViewModel);

            return controller.GetByName(_resolveName, false);
        }

        if (IsMultiInstance)
        {
            //if (ForceResolveViewModel)
            //{
            //    return controller.GetByType(Identifier, ViewModelType, OverrideViewModel, InitializeViewModel);
            //}
            if (OverrideViewModel)
                return controller.Create(Identifier, InitializeViewModel); //controller.Create(InitializeViewModel);
            else
                return controller.Create(Identifier); //controller.Create(InitializeViewModel);
        }
        return controller.Create(Identifier);
    }

    protected ViewModel ResolveViewModel(Controller controller = null)
    {
        var vm = !string.IsNullOrEmpty(_resolveName) ?
              GameManager.Container.Resolve<ViewModel>(_resolveName) :
              GameManager.Container.Resolve(ViewModelType) as ViewModel;
        return vm;
    }
}