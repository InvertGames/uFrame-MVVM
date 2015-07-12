using System;
using System.Collections.Generic;
using System.Linq;
using uFrame.IOC;
using uFrame.Kernel;
using UniRx;
using UnityEngine;

namespace uFrame.MVVM.Services
{
    public class ViewService : SystemServiceMonoBehavior
    {
        private List<ViewBase> _views;
        private static IViewResolver _viewResolver;

        [Inject]
        public static IViewResolver ViewResolver
        {
            get { return _viewResolver; }
            set { _viewResolver = value; }
        }

        public override void Setup()
        {
            if (ViewResolver == null)
            {
                ViewResolver = new ViewResolver();
            }

            this.OnEvent<InstantiateViewCommand>()
                .Subscribe(InstantiateView);

            this.OnEvent<ViewCreatedEvent>()
                .Subscribe(ViewCreated);

            this.OnEvent<ViewDestroyedEvent>()
                .Subscribe(ViewDestroyed);
        }

        public List<ViewBase> Views
        {
            get { return _views ?? (_views = new List<ViewBase>()); }
            set { _views = value; }
        }

        protected virtual void InstantiateView(InstantiateViewCommand data)
        {
            var scene = data.Scene as Scene;

            GameObject viewPrefab = null;

            if (data.Prefab != null)
            {
                viewPrefab = data.Prefab;
            }
            else if (data.ViewModelObject != null)
            {
                viewPrefab = ViewResolver.FindView(data.ViewModelObject);
            }
            else
            {
                throw new Exception("No Prefab or Viewmodel were passed in.");
                //viewPrefab = uFrameMVVMKernel.ViewResolver.FindView(data.Identifier);
            }

            var result = Instantiate(viewPrefab) as GameObject;
            var resultView = result.GetComponent<ViewBase>();

            if (data.ViewModelObject != null)
            {
                resultView.Identifier = data.ViewModelObject.Identifier;
                resultView.ViewModelObject = data.ViewModelObject;
            }
            else if (!string.IsNullOrEmpty(data.Identifier))
            {
                resultView.Identifier = data.Identifier;
            }

            resultView.CreateEventData = new ViewCreatedEvent()
            {
                IsInstantiated = true,
                View = resultView,
                Scene = data.Scene,
            };

            if (scene != null)
                resultView.transform.parent = scene.transform;

            data.Result = resultView;
        }

        protected virtual void ViewCreated(ViewCreatedEvent viewCreatedEvent)
        {

            var view = viewCreatedEvent.View;
            if (view.ViewModelObject == null && view.BindOnStart)
            {
                var viewModel = FetchViewModel(viewCreatedEvent.View);
                
            }
            Views.Add(view);
        }

        protected virtual void ViewDestroyed(ViewDestroyedEvent data)
        {
            if (data.View.DisposeOnDestroy)
            {
                var vm = data.View.ViewModelObject;
                vm.Dispose();
                Publish(new ViewModelDestroyedEvent()
                {
                    ViewModel = vm,
                });
            }
        }

        /// <summary>
        /// This is method is called by each view in order to get it's view-model as well as place it in
        /// the SceneContext if the "Save & Load" option is checked in it's inspector
        /// </summary>
        /// <param name="viewBase">The view that is requesting it's view-model.</param>
        /// <returns>A new view model or the view-model with the identifier specified found in the scene context.</returns>
        public static ViewModel FetchViewModel(ViewBase viewBase)
        {
            if (viewBase.ViewModelObject != null)
            {
                return viewBase.ViewModelObject;
            }
            
            // Attempt to resolve it by the identifier 
            //var contextViewModel = uFrameMVVMKernel.Container.Resolve<ViewModel>(viewBase.Identifier);
            // It now only registers under the viewmodeltype to allow multip different view-models with the same identifier
            var contextViewModel =
                uFrameKernel.Container.Resolve(viewBase.ViewModelType, viewBase.Identifier) as ViewModel;

            // If it doesn't resolve by the identifier we need to create it
            if (contextViewModel == null)
            {
                // Either use the controller to create it or create it ourselves
                contextViewModel = MVVMKernelExtensions.CreateViewModel(viewBase.ViewModelType);
                contextViewModel.Identifier = viewBase.Identifier;

                // Register it, this is usually when a non registered element is treated like a single-instance anways
                uFrameKernel.Container.RegisterInstance(viewBase.ViewModelType, contextViewModel,
                    string.IsNullOrEmpty(viewBase.Identifier) ? null : viewBase.Identifier);

                // Register it under the generic view-model type
                //uFrameMVVMKernel.Container.RegisterInstance<ViewModel>(contextViewModel, viewBase.Identifier);

                uFrameKernel.EventAggregator.Publish(new ViewModelCreatedEvent()
                {
                    ViewModel = contextViewModel
                });
            }
            // If we found a view-model
            if (contextViewModel != null)
            {
                // If the view needs to be overriden it will initialize with the inspector values
                if (viewBase.OverrideViewModel)
                {
                    viewBase.InitializeData(contextViewModel);
                }
                return viewBase.ViewModelObject = contextViewModel;
            }
          
            return contextViewModel;
        }


    }
}