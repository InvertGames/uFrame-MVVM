using System;
using System.Collections.Generic;
using uFrame.IOC;
using uFrame.Kernel;

namespace uFrame.MVVM
{
    /// <summary>
    /// A controller is ultimately a special service that acts as a factory for a view-model.  It will properly initialize a view-model 
    /// by setting up any command signals and additionally will invoke any initial setup required (e.g. Loading Data, Additional subscriptions.. etc)
    /// </summary>
    public abstract partial class Controller : SystemService
    {
        private bool _setupInvoked = false;
        private List<ViewModel> _instanceVMs = new List<ViewModel>();

        /// <summary>
        /// The dependency container that this controller will use
        /// </summary>
        public IUFrameContainer Container { get; set; }

        /// <summary>
        /// Create a new ViewModel. This will generate a Unique Identifier for the VM.  If this is a specific instance use the overload and pass
        /// an identifier.
        /// </summary>
        /// <returns></returns>
        public virtual ViewModel Create()
        {
            return Create(Guid.NewGuid().ToString());
            ;
        }

        /// <summary>
        /// Creates a new ViewModel with a specific identifier.  If it already exists in the SceneContext it will return that instead
        /// </summary>
        /// <param name="identifier">The identifier that will be used to check the context to see if it already exists.</param>
        /// <returns></returns>
        public virtual ViewModel Create(string identifier)
        {

            var vm = CreateEmpty(identifier);
            vm.Identifier = identifier;
            if (_setupInvoked)
            {
                Initialize(vm);
                EventAggregator.Publish(new ViewModelCreatedEvent() {ViewModel = vm});
            }
            else
            {
                _instanceVMs.Add(vm);
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
            vm.Disposer = this.DisposingViewModel;
            vm.Identifier = identifier;
            uFrameKernel.Container.RegisterViewModel(vm, identifier);
            return vm;
        }

        /// <summary>
        /// Create an empty view-model . Note: This method does not wire up the view-model to this controller and only instantiates an associated view-model.
        /// </summary>
        /// <returns>A new View-Model or the view-model found in the context with the same identifier.</returns>
        public virtual ViewModel CreateEmpty()
        {
            throw new NotImplementedException(
                "You propably need to resave you're diagram. Or you need to not call create on an abstract controller.");
        }

        /// <summary>
        /// The setup method is called when the controller is first created and has been injected.  Use this
        /// to subscribe to any events on the EventAggregator
        /// </summary>
        public override void Setup()
        {
            foreach (var item in _instanceVMs)
            {
                Initialize(item);
                EventAggregator.Publish(new ViewModelCreatedEvent() {ViewModel = item});
            }
            _instanceVMs.Clear();
            _instanceVMs = null;
            _setupInvoked = true;
        }


        /// <summary>
        /// Override this method to initialize the view-model this controller is made for. 
        /// 
        /// When you invoke thisController.Create() it will invoke this method allowing you to give some 
        /// additional options.
        /// </summary>
        /// <param name="viewModel"></param>
        public virtual void Initialize(ViewModel viewModel)
        {

        }

        /// <summary>
        /// This method is invoked when a view-model is disposing.
        /// </summary>
        /// <param name="viewModel"></param>
        public virtual void DisposingViewModel(ViewModel viewModel)
        {

        }
    };

}