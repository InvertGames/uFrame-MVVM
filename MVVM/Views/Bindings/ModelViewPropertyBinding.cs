using System;
using UniRx;
using UnityEngine;

namespace uFrame.MVVM.Bindings
{
    public class ModelViewPropertyBinding : Binding, IDisposable
    {
        public Transform Parent { get; set; }

        public string ViewName { get; set; }

        public ViewBase SourceView { get; set; }

        public Func<ModelViewModelCollectionBinding, ViewModel, ViewBase> OnCreateView { get; set; }

        public override void Bind()
        {
            base.Bind();

            Disposer = ModelProperty.SubscribeInternal(PropertyChanged);
            PropertyChanged(ModelProperty.ObjectValue);
        }

        public IDisposable Disposer { get; set; }

        private void PropertyChanged(object objectValue)
        {
            var target = GetTargetValueDelegate() as ViewBase;

            // If we have a previous view destroy it
            if (target != null && target.ViewModelObject != objectValue)
            {
                UnityEngine.Object.Destroy(target.gameObject);
            }

            // If the viewmodel is null
            if (objectValue == null)
            {
                if (SetTargetValueDelegate != null)
                    SetTargetValueDelegate(null);
                return;
            }

            // If the target local variable is empty or the viewmodel doesn't match the target
            if (target == null || target.ViewModelObject != objectValue)
            {
                // Instantiate the view
                var view = string.IsNullOrEmpty(ViewName)
                    ? SourceView.InstantiateView(objectValue as ViewModel)
                    : SourceView.InstantiateView(ViewName, objectValue as ViewModel);

                // Set the local variable of the binder
                if (SetTargetValueDelegate != null)
                    SetTargetValueDelegate(view);



                // Parent it defaulting to the view
                view.transform.parent = Parent ?? view.transform;
            }
        }

        public ModelViewPropertyBinding SetView(string viewName)
        {
            ViewName = viewName;
            return this;
        }

        public ModelViewPropertyBinding SetParent(Transform parent)
        {
            Parent = parent;
            return this;
        }

        public override void Unbind()
        {
            Disposer.Dispose();
            base.Unbind();
        }

    }
}