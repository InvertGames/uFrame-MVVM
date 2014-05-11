using UnityEngine;

/// <summary>
/// A component that will create a command binding and requires a component for the command to work.
/// </summary>
public abstract class ComponentCommandBinding : ComponentBinding
{
    public Component _TargetComponent;

    /// <summary>
    /// Simply a wrapper of "Binding" property cast to ModelCommandBinding
    /// </summary>
    public ModelCommandBinding CommandBinding
    {
        get
        {
            return Binding as ModelCommandBinding;
        }
    }

    public Component Component { get; set; }
}