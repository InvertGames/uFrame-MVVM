using System;
using UnityEngine;

/// <summary>
/// A class that contains a binding from a ViewModel to a Target
/// </summary>
public class ModelPropertyBinding : Binding, ITwoWayBinding
{
    private object _lastValue;
    private bool _isImmediate = true;

    public bool IsImmediate
    {
        get { return _isImmediate; }
        set { _isImmediate = value; }
    }

    public override void Bind()
    {
        base.Bind();
        if (TwoWay)
            _lastValue = GetTargetValueDelegate();

        ModelProperty.PropertyChanged += PropertyChanged;
        if (IsImmediate)
        PropertyChanged(ModelProperty.ObjectValue);
    }

    /// <summary>
    /// If the value has changed apply the value to the property without reinvoking the SetTargetDelegate.
    /// It's important to not reinvoke the SetTargetDelegate because it will create a stack overflow. But only
    /// the SetTargetDelegate should be ignored because there may be other bindings to this property and when it changes
    /// they should definately know about it.
    /// </summary>
    public void BindReverse()
    {
        var currentValue = GetTargetValueDelegate();

        if (_lastValue != currentValue)
        {
            // Remove this property change event for a moment while updating the value so it doesn't do it again
            ModelProperty.PropertyChanged -= PropertyChanged;
            ModelProperty.ObjectValue = currentValue;
            ModelProperty.PropertyChanged += PropertyChanged;
            // Store the last value
            _lastValue = currentValue;
        }
    }

    /// <summary>
    /// Unbind remove the property changed event handler and the sets the model property
    /// to null so it can be refreshed if a new model is set
    /// </summary>
    public override void Unbind()
    {
        ModelProperty.PropertyChanged -= PropertyChanged;
        base.Unbind();
    }

    ///
    /// <summary>
    /// The property changed event handler.
    /// </summary>
    /// <param name="value"></param>
    private void PropertyChanged(object value)
    {
        _lastValue = value;
        SetTargetValueDelegate(value);
    }
}

public class ModelViewPropertyBinding : Binding
{
    public Transform Parent { get; set; }

    public string ViewName { get; set; }

    public Func<ModelViewModelCollectionBinding, ViewModel, ViewBase> OnCreateView { get; set; }

    public override void Bind()
    {
        base.Bind();

        ModelProperty.PropertyChanged += PropertyChanged;

        PropertyChanged(ModelProperty.ObjectValue);
    }

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
                ? Source.InstantiateView(objectValue as ViewModel)
                : Source.InstantiateView(ViewName, objectValue as ViewModel);

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
        ModelProperty.PropertyChanged -= PropertyChanged;
        base.Unbind();
    }
}