using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Unity3d Component that will provide a binding to a specified View
/// </summary>
public abstract class ComponentBinding : MonoBehaviour
{
    public string _ModelMemberName;
    private ViewBase _SourceView;
    private IBinding _binding;

    /// <summary>
    /// The binding that has been created for this component.
    /// </summary>
    public IBinding Binding
    {
        get { return _binding ?? (_binding = GetBinding()); }
        set { _binding = value; }
    }

    public ViewBase SourceView
    {
        get { return _SourceView; }
        set { _SourceView = value; }
    }

    /// <summary>
    /// Override this method to filter the list of properties that are displayed in the Binding Inspector
    /// </summary>
    /// <param name="modelProperties"></param>
    /// <returns></returns>
    public virtual IEnumerable<KeyValuePair<string, IObservableProperty>> FilterBindableProperties(Dictionary<string, IObservableProperty> modelProperties)
    {
        return modelProperties;//.Where(p => !typeof(ICollection).IsAssignableFrom(p.Value.ValueType));
    }

    protected virtual void Awake()
    {
        this.hideFlags = HideFlags.HideInInspector;
        // Check if it is loading from a scene.
        // The only way _SourceView could be set is if Unity loaded it from a scene.
        if (_SourceView != null)
        {
            // Mark the binding as a component binding so its not removed when rebinding.
            Binding.IsComponent = true;
            // Add the binding to the source view.
            _SourceView.AddBinding(Binding);
        }
    }

    /// <summary>
    /// The binding provider.  Create the binding that the component will add to the source view here.
    /// </summary>
    /// <returns>The binding that will be added to the source view.</returns>
    protected abstract IBinding GetBinding();
}