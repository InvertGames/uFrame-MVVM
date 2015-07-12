using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using uFrame.Kernel;
using uFrame.MVVM.Bindings;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace uFrame.MVVM
{
    /// <summary>
    /// The base class for a View that binds to a ViewModel
    /// </summary>
    public abstract partial class ViewBase : MVVMComponent, IUFSerializable, IBindable
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


        #region Props
        private List<IBindingProvider> _bindingProviders;

        private bool _bound = false;

        [SerializeField, HideInInspector]
        [FormerlySerializedAs("_resolveName")]
        private string _viewModelId = "";

        [SerializeField, HideInInspector]
        private bool _InjectView = false;

        private int _ViewId;

        [SerializeField, HideInInspector]
        private bool _BindOnStart = true;
        [SerializeField, HideInInspector]
        private bool _DisposeOnDestroy;


        [HideInInspector]
        private ViewModel _Model;

        [SerializeField, HideInInspector, UFGroup("View Model Properties")]
        private bool _overrideViewModel;

        #endregion

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
                if (!ViewModelObject.Bindings.ContainsKey(ViewId))
                {
                    ViewModelObject.Bindings.Add(ViewId, new List<IDisposable>());
                }

                return ViewModelObject.Bindings[ViewId];
            }
        }

        /// <summary>
        /// This is the default identifier to use when "ResolveName" is not specified and it's a single instance.
        /// This field is automatically overriden by the uFrame designer.
        /// </summary>
        public virtual string DefaultIdentifier
        {
            get
            {
                return null;
            }
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
                if (String.IsNullOrEmpty(_viewModelId))
                {
                    _viewModelId = Guid.NewGuid().ToString();
                }
                return _viewModelId;
            }
            set
            {
                if (ViewModelObject != null && ViewModelObject.Identifier == value) return;
                _viewModelId = value;
                //ViewModelObject = FetchViewModel(Identifier,ViewModelType);
            }
        }

        public virtual void SetIdentifierSilently(string id)
        {
            _viewModelId = id;
        }

        public bool InjectView
        {
            get { return _InjectView; }
            set { _InjectView = value; }
        }

        public bool BindOnStart
        {
            get { return _BindOnStart; }
            set { _BindOnStart = value; }
        }


        /// <summary>
        /// A lazy loaded property for "GetInstanceId" on the game-object.
        /// </summary>
        public int ViewId
        {
            get
            {
                if (_ViewId == 0)
                {
                    _ViewId = this.gameObject.GetInstanceID();
                }
                return _ViewId;
            }
        }

        /// <summary>
        /// Is this view currently bound to a view-model?
        /// </summary>
        public bool IsBound
        {
            get { return _bound; }
            set { _bound = value; }
        }

        public void SetViewModelObjectSilently(ViewModel vm)
        {
            _Model = vm;
        }
        public virtual ViewModel ViewModelObject
        {
            get
            {
                return _Model;
            }
            set
            {

                if (_Model == value) return;
                if (value == null || IsBound) Unbind();

                _Model = value;

                if (_Model == null)
                {
                    return;
                }

                SetIdentifierSilently(_Model.Identifier);

                //if (OverrideViewModel)
                //{
                //    InitializeData(_Model);
                //}

                SetupBindings(); //Star binding procedure

                //Bind();
            }
        }

        private void Reset()
        {

        }

        public abstract Type ViewModelType { get; }



        /// <summary>
        /// The name of the prefab that created this view
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// 	<para>This method adds a binding directly onto the view-model.  It will be registered with a key of this object instance id, this allows any disposable to be
        /// properly disposed when this view is destroyed.</para>
        /// </summary>
        /// <param name="binding">The IDisposable that will be invoked when a view or view-model is un-bound.</param>
        /// <returns>The same disposable you pass in order to store a local destruction of the binding if needed.</returns>
        /// <example>
        /// 	<code title="Adding a simple binding." description="This example will register the HealthChange subscription so that when the view is destroyed, it will no longer be invoked when the health changes." groupname="Views" lang="CS">
        /// this.AddBinding(MyViewModel.HealthProperty.Subscribe(HealthChanged));</code>
        /// </example>
        public IDisposable AddBinding(IDisposable binding)
        {
            Bindings.Add(binding);
            return binding;
        }

        /// <summary>
        /// 	<para>This method is invoked right after the view-model has been bound.</para>
        /// </summary>

        public virtual void AfterBind()
        {
        }

        public ViewCreatedEvent CreateEventData { get; set; }


        public override void KernelLoaded()
        {
            base.KernelLoaded();
            if (this.InjectView)
            {
                uFrameKernel.Container.Inject(this);
            }
            this.Publish(CreateEventData ?? (CreateEventData = new ViewCreatedEvent()
            {
                IsInstantiated = false,
                Scene = ParentScene,
                View = this
            }));
        }



        /// <summary>
        /// When this view is destroy it will decrememnt the ViewModel's reference count.  If the reference count reaches 0
        /// it will call "Unbind" on the viewmodel properly unbinding anything subscribed to it.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (IsBound) Unbind();
            if (!uFrameKernel.IsKernelLoaded || CreateEventData == null) return;

            this.Publish(new ViewDestroyedEvent()
            {
                IsInstantiated = CreateEventData.IsInstantiated,
                Scene = CreateEventData.Scene,
                View = this
            });
        }

        protected virtual void OnDisable()
        {

        }

        protected virtual void OnEnable()
        {

        }
        public IScene ParentScene
        {
            get
            {



                return _parentScene ?? (_parentScene = (GetComponentInParentRecursive(this.transform, typeof(IScene)) as IScene));
            }
            set { _parentScene = value; }
        }

        private object GetComponentInParentRecursive(Transform parent, Type type)
        {
            if (parent == null) return null;
            var theComponent = GetComponentInParent(type);
            if (theComponent != null)
            {
                return theComponent;
            }

            var result = GetComponentInParentRecursive(parent.parent, type);
            return result;
        }
        /// <summary>
        /// 	<para>This method is the primary method of a view.  It's purpose is to provide a safe place to create subscriptions/bindings to it's view-model.  When
        /// this method is invoked it will allways have an instance to a view-model.</para>
        /// 	<para>In this method you should subscribe to it's owned view-models properties, collections, and execute commands.</para>
        /// </summary>
        /// <example>
        /// 	<code title="Example" description="" lang="CS">
        /// var viewModel = ViewModelObject as FPSWeaponViewModel; // &lt;-- for clarity, use property instead
        /// this.BindProperty(viewModel.AmmoProperty, ammo=&gt; { _AmmoLabel.text = ammo.ToString(); });</code>
        /// </example>
        public virtual void Bind()
        {
        }

        /// <summary>
        /// This method is called in order to create a model for this view.  In a uFrame Designer generated
        /// view it will implement this method and call the "FetchViewModel" on the scene manager.
        /// </summary>
        [Obsolete]
        public virtual ViewModel CreateModel()
        {
            return null;
        }

        /// <summary>
        /// 	<para>A wrapper for "InitializeViewModel" which takes the information supplied in the inspector and applies it to the view-model.</para>
        /// 	<innovasys:widget type="Note Box" layout="block" xmlns:innovasys="http://www.innovasys.com/widgets">
        /// 		<innovasys:widgetproperty layout="block" name="Content">If your viewmodel is a composite view-model containing properties of other view-models, a view can
        ///     be referenced, and this method will use that view's view-model.</innovasys:widgetproperty>
        /// 	</innovasys:widget>
        /// </summary>
        /// <param name="model"></param>
        /// <example>
        /// 	<code title="Initialize Data Example" description="Demonstrates using initializedata to pull use the values from the inspector on an instantiated view." groupname="Views" lang="CS">
        /// var myView = InstantiateView(new FPSWeaponViewModel() { Ammo=60 });
        /// myView.InitializeData();
        /// Debug.Log(myView.FPSWeapon.Ammo);
        /// // output will not be 60, it will be the value specified on the views insepctor.</code>
        /// </example>
        public void InitializeData(ViewModel model)
        {
            InitializeViewModel(model);
        }


        private IScene _parentScene;


        /// <summary>This method will ensure that a view-model exists and then call the bind method when it's appropriate.</summary>
        public void SetupBindings()
        {
            if (ViewModelObject != null)
            {
                ViewModelObject.References++;
            }

            foreach (var bindingProvider in BindingProviders)
                bindingProvider.Bind(this);

            PreBind();
            Bind();
            IsBound = true;
            AfterBind();

        }

        /// <summary>Unbind any binding or disposable that has been added via the "AddBinding" method.</summary>
        public virtual void Unbind()
        {
            DisposeBindings();
            IsBound = false;
        }

        public virtual void DisposeBindings()
        {
            if (_Model != null)
            {
                _Model.References--;
                foreach (var binding in Bindings)
                {
                    binding.Dispose();
                }
            }

            for (int index = 0; index < BindingProviders.Count; index++)
            {
                var bindingProvider = BindingProviders[index];
                bindingProvider.Unbind(this);
            }
        }

        public virtual void Write(ISerializerStream stream)
        {

            //ViewModelObject.Write(stream);
            stream.SerializeString("Identifier", this.Identifier);
            stream.SerializeString("ViewType", this.GetType().FullName);
        }

        /// <summary>Will deserialize this view directly from a stream.</summary>
        public virtual void Read(ISerializerStream stream)
        {
            this.Identifier = stream.DeserializeString("Identifier");
        }

        /// <summary>
        /// This method should be overriden to Initialize the ViewModel
        /// with any options specified in a unity component inspector.
        /// </summary>
        /// <buildflag>Exclude from Online</buildflag>
        /// <buildflag>Exclude from Booklet</buildflag>
        /// <param name="model">The model to initialize.</param>
        protected virtual void InitializeViewModel(ViewModel model) { }

        /// <summary>
        /// This method is called immediately before "Bind".  This method is used
        /// by uFrames designer generated code to set-up defined bindings.
        /// </summary>
        protected virtual void PreBind()
        {
        }

        public bool OverrideViewModel
        {
            get
            {
                return _overrideViewModel;
            }
        }

        public bool DisposeOnDestroy
        {
            get { return _DisposeOnDestroy; }
            set { _DisposeOnDestroy = value; }
        }
    }



    // Observable Stuff
    public partial class ViewBase
    {
        #region Observables

        /// <summary>
        /// Observable that notifies its subscribers only when the transform has changed.
        /// </summary>
        public Subject<Transform> TransformChangedObservable
        {
            get { return _transformObservable ?? (_transformObservable = new Subject<Transform>()); }
            set { _transformObservable = value; }
        }

        public IObservable<Vector3> PositionAsObservable
        {
            get
            {
                return TransformChangedObservable.Select(p => p.transform.position).DistinctUntilChanged();
            }
        }
        public IObservable<Vector3> LocalPositionAsObservable
        {
            get
            {
                return TransformChangedObservable.Select(p => p.transform.localPosition).DistinctUntilChanged();
            }
        }
        public IObservable<Quaternion> LocalRotationAsObservable
        {
            get
            {
                return TransformChangedObservable.Select(p => p.transform.localRotation).DistinctUntilChanged();
            }
        }

        public IObservable<Quaternion> RotationAsObservable
        {
            get
            {
                return TransformChangedObservable.Select(p => p.transform.rotation).DistinctUntilChanged();
            }
        }
        public IObservable<Vector3> ScaleAsObservable
        {
            get
            {
                return TransformChangedObservable.Select(p => p.transform.localScale).DistinctUntilChanged();
            }
        }

        #endregion
        private IObservable<Vector3> _positionObservable;

        private IObservable<Quaternion> _rotationObservable;
        private IObservable<Vector3> _scaleObservable;
        private IObservable<Transform> _transformChangedObservable;
        private Subject<Transform> _transformObservable;
        Subject<Unit> _updateObservable;
        /// <summary>This Observable allows you to use the Update method of a monobehaviour as an observable.</summary>
        /// <example>
        /// 	<code title="Simple Update Observable" description="In this example we subscribe to the update observable, and print out a message every frame." groupname="Views" lang="CS">
        /// this.UpdateAsObservable().Subscribe(_=&gt;Debug.Log("Output every frame"));</code>
        /// </example>
        public IObservable<Unit> UpdateAsObservable()
        {
            return _updateObservable ?? (_updateObservable = new Subject<Unit>());
        }

        /// <summary>
        /// 	<para>Update is called every frame, if the MonoBehaviour is enabled.  It is important to make sure that you override this method instead of just creating
        /// it.  uFrame uses this method as an observable, and that observable is used for all default Scene Property implementations.</para>
        /// </summary>
        public virtual void Update()
        {
            if (_updateObservable != null)
                _updateObservable.OnNext(Unit.Default);

            if (_transformObservable != null && transform.hasChanged)
            {
                TransformChangedObservable.OnNext(transform);
                transform.hasChanged = false;
            }
        }
    }

    // Obsolete Stuff
    public partial class ViewBase
    {
        [Obsolete]
        protected ViewModel RequestViewModel(object dummy)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public void ExecuteCommand(ICommand command)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public void ExecuteCommand(ICommand command, object selector)
        {
            throw new NotImplementedException();
        }
    }

    public class MVVMComponent : uFrameComponent
    {
        public ViewBase InstantiateView(ViewModel model)
        {
            return InstantiateView(model, Vector3.zero);
        }

        public ViewBase InstantiateView(ViewModel model, Vector3 position)
        {
            return InstantiateView(model, position, Quaternion.identity);
        }

        public ViewBase InstantiateView(ViewModel model, Vector3 position, Quaternion rotation)
        {
            return transform.InstantiateView(model, position, rotation);
        }

        public ViewBase InstantiateView(GameObject prefab, ViewModel model)
        {
            return InstantiateView(prefab, model, Vector3.zero);
        }

        public ViewBase InstantiateView(GameObject prefab, ViewModel model, Vector3 position)
        {
            return InstantiateView(prefab, model, position, Quaternion.identity);
        }

        public ViewBase InstantiateView(string viewName, string identifier = null)
        {
            return InstantiateView(viewName, null, identifier);
        }

        /// <summary>
        /// Instantiates a view.
        /// </summary>
        /// <param name="viewName">The name of the prefab/view to instantiate</param>
        /// <param name="model">The model that will be passed to the view.</param>
        /// <returns>The instantiated view</returns>
        public ViewBase InstantiateView(string viewName, ViewModel model, string identifier = null)
        {
            return InstantiateView(viewName, model, Vector3.zero, identifier);
        }
        /// <summary>
        /// Instantiates a view.
        /// </summary>
        /// <param name="viewName">The name of the prefab/view to instantiate</param>

        /// <param name="position">The position to instantiate the view.</param>
        /// <returns>The instantiated view</returns>
        public ViewBase InstantiateView(string viewName, Vector3 position, string identifier = null)
        {
            return InstantiateView(viewName, null, position, Quaternion.identity, identifier);
        }

        /// <summary>
        /// Instantiates a view.
        /// </summary>
        /// <param name="viewName">The name of the prefab/view to instantiate</param>
        /// <param name="model">The model that will be passed to the view.</param>
        /// <param name="position">The position to instantiate the view.</param>
        /// <returns>The instantiated view</returns>
        public ViewBase InstantiateView(string viewName, ViewModel model, Vector3 position, string identifier = null)
        {
            return InstantiateView(viewName, model, position, Quaternion.identity, identifier);
        }

        /// <summary>
        /// Instantiates a view.
        /// </summary>
        /// <param name="viewName">The name of the prefab/view to instantiate</param>
        /// <param name="model">The model that will be passed to the view.</param>
        /// <param name="position">The position to instantiate the view.</param>
        /// <param name="rotation">The rotation to instantiate the view with.</param>
        /// <returns>The instantiated view</returns>
        public ViewBase InstantiateView(string viewName, ViewModel model, Vector3 position,
            Quaternion rotation, string identifier = null)
        {
            return transform.InstantiateView(viewName, model, position, rotation, identifier);
        }

        /// <summary>
        /// Instantiates a view.
        /// </summary>
        /// <param name="prefab">The prefab/view to instantiate</param>
        /// <param name="model">The model that will be passed to the view.</param>
        /// <param name="position">The position to instantiate the view.</param>
        /// <param name="rotation">The rotation to instantiate the view with.</param>
        /// <returns>The instantiated view</returns>
        public ViewBase InstantiateView(GameObject prefab, ViewModel model, Vector3 position,
            Quaternion rotation, string identifier = null)
        {
            return transform.InstantiateView(prefab, model, position, rotation, identifier);
        }
    }

}
