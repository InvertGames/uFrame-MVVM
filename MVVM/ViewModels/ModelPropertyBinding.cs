
namespace uFrame.MVVM.Bindings
{

    using System;

    /// <summary>
    /// A class that contains a binding from a ViewModel to a Target
    /// </summary>
    public class ModelPropertyBinding : Binding, ITwoWayBinding
    {

        private bool _isImmediate = true;

        public bool IsImmediate
        {
            get { return _isImmediate; }
            set { _isImmediate = value; }
        }

        public override void Bind()
        {
            base.Bind();
            Disposer = ModelProperty.SubscribeInternal(PropertyChanged);
            // ModelProperty.ValueChanged += PropertyChanged;
            if (IsImmediate)
                PropertyChanged(ModelProperty.ObjectValue);
        }

        public IDisposable Disposer { get; set; }

        /// <summary>
        /// If the value has changed apply the value to the property without reinvoking the SetTargetDelegate.
        /// It's important to not reinvoke the SetTargetDelegate because it will create a stack overflow. But only
        /// the SetTargetDelegate should be ignored because there may be other bindings to this property and when it changes
        /// they should definately know about it.
        /// </summary>
        public void BindReverse()
        {

        }

        /// <summary>
        /// Unbind remove the property changed event handler and the sets the model property
        /// to null so it can be refreshed if a new model is set
        /// </summary>
        public override void Unbind()
        {
            Disposer.Dispose();
            base.Unbind();
        }

        ///
        /// <summary>
        /// The property changed event handler.
        /// </summary>
        /// <param name="value"></param>
        private void PropertyChanged(object value)
        {
            SetTargetValueDelegate(value);
        }
    }
}