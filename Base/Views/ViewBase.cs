using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

/// <summary>
/// The base class for a View that binds to a ViewModel
/// </summary>
public abstract class ViewBase : ViewContainer, ICommandHandler
{
    public T AddComponentBinding<T>() where T : ObservableComponent
    {
        var component = gameObject.AddComponent<T>();
        AddBinding(component);
        return component;
    }
    public IDisposable AddComponentBinding(ObservableComponent component) 
    {
        return AddBinding(component);
    }
    Subject<Unit> _updateObservable;

    /// <summary>Update is called every frame, if the MonoBehaviour is enabled.</summary>
    public virtual void Update()
    {
        if (_updateObservable != null) _updateObservable.OnNext(Unit.Default);
    }

    /// <summary>Update is called every frame, if the MonoBehaviour is enabled.</summary>
    public IObservable<Unit> UpdateAsObservable()
    {
        return _updateObservable ?? (_updateObservable = new Subject<Unit>());
    }
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

    [HideInInspector]
    private string _id;

    [SerializeField, HideInInspector]
    private bool _InjectView = false;

    private int _instanceId;

    //[HideInInspector]
    //public string _ViewModelControllerType;
    [HideInInspector]
    private ViewModel _Model;

    [SerializeField, HideInInspector, UFGroup("View Model Properties")]
    private bool _overrideViewModel;

    //[HideInInspector]
    //public string _ViewModelControllerMethod;
    private ViewBase _parentView;

    private IObservable<Vector3> _positionObservable;

    [SerializeField, HideInInspector, UFGroup("View Model Properties")]
    private string _resolveName = null;

    private IObservable<Quaternion> _rotationObservable;

    [SerializeField, HideInInspector]
    private bool _Save = false;

    private IObservable<Vector3> _scaleObservable;

    [NonSerialized]
    private bool _shouldRebindOnEnable = false;

    public List<IBindingProvider> BindingProviders
    {
        get { return _bindingProviders ?? (_bindingProviders = new List<IBindingProvider>()); }
        set { _bindingProviders = value; }
    }

    /// <summary>
    /// A wrapper for this view's viewmodel bindings.  It is a wrapper
    /// for ViewModel.Bindings[gameObject.GetInstanceId()]
    /// </summary>
    public List<IDisposable> Bindings
    {
        get
        {
            if (!ViewModelObject.Bindings.ContainsKey(InstanceId))
            {
                ViewModelObject.Bindings.Add(InstanceId, new List<IDisposable>());
            }

            return ViewModelObject.Bindings[InstanceId];
        }
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

    public virtual ICommandHandler CommandHandler
    {
        get { return this; }
    }

    /// <summary>
    /// This is the default identifier to use when "ResolveName" is not specified and it's a single instance.
    /// This field is automatically overriden by the uFrame designer.
    /// </summary>
    public virtual string DefaultIdentifier
    {
        get
        {
            return ViewModelType.Name;
        }
    }

    public bool ForceResolveViewModel
    {
        get { return _forceResolveViewModel; }
        set { _forceResolveViewModel = value; }
    }

    /// <summary>
    /// The identifier used for requesting a view-model.
    /// Implementation Details:
    /// -If its not a multiinstance viewmodel and the "ResolveName" is empty it will use "DefaultIdentifier" property otherwise it will use
    /// the resolve name.
    /// - If it's a multiinstance viewmodel and the resolvename is specified it will use that.
    /// - If the Use Hashcode as identifier is checked it will use this views hashcode.
    ///
    /// Note: If using a prefab that is placed in the Unity editor in various places around a scene and it still needs to be unique every
    /// scene load (for scene loading and saving) you will want to override this property and supply a identifier that makes it unique.
    /// </summary>
    public virtual string Identifier
    {
        get
        {
            if (ForceResolveViewModel)
            {
                if (string.IsNullOrEmpty(_resolveName)) return null;
                return _resolveName;
            }
            return _id = (this.transform.position.GetHashCode()).ToString() + this.ViewModelType.Name;
        }
        set { _id = value; }
    }

    public bool InjectView
    {
        get { return _InjectView; }
        set { _InjectView = value; }
    }

    /// <summary>
    /// A lazy loaded property for "GetInstanceId" on the game-object.
    /// </summary>
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

    public bool Instantiated { get; set; }

    //public virtual bool IsMultiInstance
    //{
    //    get
    //    {
    //        return true;
    //    }
    //}

    public bool IsBound
    {
        get { return _bound; }
        set { _bound = value; }
    }

    [Obsolete]
    public virtual bool IsMultiInstance
    {
        get { return true; }
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
                //var parentView = transform.parent;
                //while (parentView != null)
                //{
                    
                //    if (parentView == null)
                //        parentView = parentView.parent;
                //}
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

    public IObservable<Vector3> PositionObservable
    {
        get

        {

            return _positionObservable ?? (_positionObservable = UpdateAsObservable().Where(p => this.transform.hasChanged).Select(_ => this.transform.position));
        }
    }

    public IObservable<Quaternion> RotationAsObservable
    {
        get
        {
            return _rotationObservable ?? (_rotationObservable = UpdateAsObservable().Where(p => this.transform.hasChanged).Select(_ => this.transform.rotation));
        }
    }

    public IObservable<Vector3> ScaleAsObservable
    {
        get
        {
            return _scaleObservable ?? (_scaleObservable = UpdateAsObservable().Where(p => this.transform.hasChanged).Select(_ => this.transform.localScale));
        }
    }


    /// <summary>
    /// Should this view be saved in the "SceneContext"
    /// </summary>
    public bool Save
    {
        get { return _Save; }
        set { _Save = value; }
    }


    public SceneManager SceneManager
    {
        get { return GameManager.ActiveSceneManager; }
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
            if (IsBound)
            {
                Unbind();
                SetupBindings();
            }
            //Bind();
        }
    }

    public abstract Type ViewModelType { get; }

    /// <summary>
    /// The name of the prefab that created this view
    /// </summary>
    public string ViewName { get; set; }

    /// <summary>
    /// Adds a binding to the view-model's binding dictionary for this view.
    /// </summary>
    /// <param name="binding"></param>
    public IDisposable AddBinding(IDisposable binding)
    {
        Bindings.Add(binding);
        return binding;
    }

    /// <summary>
    /// This method is invoked right after it has been bound
    /// </summary>
    public virtual void AfterBind()
    {
    }

    public virtual void Awake()
    {
        
    }

    /// <summary>
    /// This method is called in order to subscribe to properties, commands, and collections.
    /// </summary>
    public virtual void Bind()
    {
    }

    /// <summary>
    /// This method is called in order to create a model for this view.  In a uFrame Designer generated
    /// view it will implement this method and call the "RequestViewModel" on the scene manager.
    /// </summary>
    /// <returns>A view model for this view to bind to</returns>
    public abstract ViewModel CreateModel();

    /// <summary>
    /// All of the designer generated "Execute{CommandName}" ultimately use this method.  So when
    /// need to execute a command on an outside view-model(meaning not the view-model of this view) this
    /// method can be used. e.g. ExecuteCommand(command, argument)
    /// </summary>
    /// <param name="command">The command to execute e.g. MyGameViewModel.MainMenuCommand</param>
    /// <param name="argument">The argument to pass along if needed.</param>
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

    /// <summary>
    /// All of the designer generated "Execute{CommandName}" ultimately use this method.  So when
    /// need to execute a command on an outside view-model(meaning not the view-model of this view) this
    /// method can be used. e.g. ExecuteCommand(MyGameViewModel.MainMenuCommand)
    /// </summary>
    /// <param name="command">The command to execute e.g. MyGameViewModel.MainMenuCommand</param>
    public virtual void ExecuteCommand(ICommand command)
    {
        if (command == null) return;

        if (command.Parameter == null)
            command.Parameter = this.ViewModelObject;

        IEnumerator enumerator = command.Execute();
        if (enumerator != null)
            StartCoroutine(enumerator);
    }

    /// <summary>
    /// Executes a command of type ICommand.
    /// </summary>
    /// <typeparam name="TArgument"></typeparam>
    /// <param name="command">The command instance to execute.</param>
    /// <param name="sender">The sender of the command.</param>
    /// <param name="argument">The argument required by the command.</param>
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

    /// <summary>
    /// Executes a command of type ICommand.
    /// </summary>
    /// <typeparam name="TArgument"></typeparam>
    /// <param name="command">The command instance to execute.</param>
    /// <param name="argument">The argument required by the command.</param>
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

    /// <summary>
    /// A wrapper for "InitializeViewModel".
    /// </summary>
    /// <param name="model"></param>
    public void InitializeData(ViewModel model)
    {
        InitializeViewModel(model);
        model.Dirty = false;
    }

    /// <summary>
    /// When this view is destroy it will decrememnt the ViewModel's reference count.  If the reference count reaches 0
    /// it will call "Unbind" on the viewmodel properly unbinding anything subscribed to it.
    /// </summary>
    public virtual void OnDestroy()
    {
       
        Unbind();
        var pv = ParentView;
        if (pv != null)
        {
            pv.ChildViews.Remove(this);
        }
    }

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

    public virtual void Read(ISerializerStream stream)
    {
        ViewModelObject.Read(stream);
        stream.SerializeString("ViewType", this.GetType().FullName);
    }

    ///// <summary>
    ///// Removes a binding from the view-models binding dictionary for this view.
    ///// </summary>
    ///// <param name="binding"></param>
    //public void RemoveBinding(IBinding binding)
    //{
    //    Bindings.Remove(binding);
    //}

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

        if (IsBound)
            return;
        // Initialize the model
        if (ViewModelObject != null)
        {
            ViewModelObject.References++;

            if (ViewModelObject.CommandHandler == null)
                ViewModelObject.CommandHandler = CommandHandler;
        }
        // Loop through and binding providers and let them add bindings
        foreach (var bindingProvider in BindingProviders)
            bindingProvider.Bind(this);

        // Add any programming bindings
        PreBind();
        Bind();
        //// Initialize the bindings
        //for (var i = 0; i < Bindings.Count; i++)
        //    Bindings[i].Bind();

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
        IsBound = true;

        AfterBind();
    }

    /// <summary>
    /// The start method approparitely initializes the "ChildViews" collection.
    /// </summary>
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
        if (ParentView == null || ParentView.IsBound)
        {
            SetupBindings();
        }
    }

    /// <summary>
    /// Unbind the current bindings.
    /// </summary>
    public virtual void Unbind()
    {
        if (_Model != null)
        {
            _Model.References--;
            if (_Model.References < 1)
            {
                _Model.Unbind();
            }
            else
            {
                foreach (var binding in Bindings)
                {
                    binding.Dispose();
                }
            }
        }

        foreach (var bindingProvider in BindingProviders)
            bindingProvider.Unbind(this);

        IsBound = false;
    }

    public virtual void Write(ISerializerStream stream)
    {
        ViewModelObject.Write(stream);
        stream.SerializeString("ViewType", this.GetType().FullName);
    }

    /// <summary>
    /// Overriden by the the uFrame designer to apply any two-way/reverse properties.
    /// </summary>
    protected virtual void Apply()
    {
    }

    /// <summary>
    /// This method should be overriden to Initialize the ViewModel
    /// with any options specified in a unity component inspector.
    /// </summary>
    /// <param name="model">The model to initialize.</param>
    protected abstract void InitializeViewModel(ViewModel model);

    /// <summary>
    /// Just calls the apply method.
    /// </summary>
    public virtual void LateUpdate()
    {
     
        if (IsBound)
            Apply();
    }

    /// <summary>
    /// This method is called immediately before "Bind".  This method is used
    /// by uFrames designer generated code to set-up defined bindings.
    /// </summary>
    protected virtual void PreBind()
    {
    }

    protected virtual ViewBase ReplaceView(ViewBase current, ViewModel value, GameObject prefab)
    {
        if (value == null && current != null && current.gameObject != null)
        {
            Destroy(current.gameObject);
        }
        if (prefab == null)
        {
            return ((this.InstantiateView(value)));
        }
        else
        {
            return ((this.InstantiateView(prefab, value)));
        }
    }

    /// <summary>
    /// Request a view-model with a given controller.
    /// </summary>
    /// <param name="controller"></param>
    /// <returns></returns>
    protected ViewModel RequestViewModel(Controller controller)
    {
        return SceneManager.RequestViewModel(this, controller, Identifier);
    }
}