using System;
using UniRx;
using UnityEngine;

namespace uFrame.MVVM.Bindings
{
    /// <summary>
    /// The base class for all bindings.
    /// </summary>
    public abstract class Binding : IBinding, IDisposable
    {
        private IObservableProperty _modelProperty;
        private Func<IObservableProperty> _modelPropertySelector;

        /// <summary>
        /// Does this instance type implement ITwoWayBinding?
        /// </summary>
        public bool CanTwoWayBind
        {
            get { return this is ITwoWayBinding; }
        }

        /// <summary>
        /// A delegate for Getting the target value and is required for a two-way binding.
        /// </summary>
        public Func<object> GetTargetValueDelegate { get; set; }

        public bool IsBound { get; set; }

        /// <summary>
        /// Was this loaded from a component in the Unity Inspector?
        /// </summary>
        public bool IsComponent { get; set; }

        /// <summary>
        /// The source ViewModel member name that is being bound to.
        /// </summary>
        public string ModelMemberName
        {
            get { return ModelProperty.PropertyName; }
            set { }
        }

        /// <summary>
        /// The Model Property that is being bound to. Will call the ModelPropertySelector if null.
        /// </summary>
        public IObservableProperty ModelProperty
        {
            get
            {
                if (_modelProperty != null)
                    return _modelProperty;
                else return _modelProperty = ModelPropertySelector();
            }
            set { _modelProperty = value; }
        }

        /// <summary>
        /// A selector that will select the model property.
        /// This should be set manually if reflection shouldn't be used.
        /// </summary>
        public Func<IObservableProperty> ModelPropertySelector
        {
            get { return _modelPropertySelector; }
            set { _modelPropertySelector = value; }
        }

        /// <summary>
        /// A delegate to set the value of the target member(s).
        /// </summary>
        public Action<object> SetTargetValueDelegate { get; set; }

        ///// <summary>
        ///// The owner view that this Binding belongs to
        ///// </summary>
        //public ViewModel Source { get; set; }

        /// <summary>
        /// The value of the ViewModel Member
        /// </summary>
        public object SourceValue
        {
            get { return ModelProperty.ObjectValue; }
        }

        /// <summary>
        /// Is this a two-way binding.
        /// </summary>
        public bool TwoWay { get; set; }

        protected Binding()
        {
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="sourceView">The View that will own this binding.</param>
        ///// <param name="modelMemberName">The member of the ViewModel.</param>
        //protected Binding(ViewBase sourceView, string modelMemberName)
        //{
        //    Source = sourceView;
        //    ModelMemberName = modelMemberName;
        //}

        /// <summary>
        /// Set-up the binding. This should almost always be implemented in a deriving class.
        /// </summary>
        public virtual void Bind()
        {
            IsBound = true;
            _modelProperty = null;
        }

        /// <summary>
        /// Unbind this binding
        /// </summary>
        public virtual void Unbind()
        {
            IsBound = false;
            _modelProperty = null;
        }

        public void Dispose()
        {
            Unbind();
        }
    }

}