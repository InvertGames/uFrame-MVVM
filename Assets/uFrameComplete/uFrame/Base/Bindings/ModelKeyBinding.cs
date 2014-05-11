using UnityEngine;

/// <summary>
/// Binds a key to a ViewModel command.
/// </summary>
public class ModelKeyBinding : ModelCommandBinding
{
    private KeyBindingEventType _keyEventType = KeyBindingEventType.KeyDown;

    public bool Alt { get; set; }

    public bool Control { get; set; }

    public KeyCode Key { get; set; }

    public KeyBindingEventType KeyEventType
    {
        get { return _keyEventType; }
        set { _keyEventType = value; }
    }

    public bool Shift { get; set; }

    public ModelKeyBinding(KeyCode key)
        : base()
    {
        Key = key;
    }

    public ModelKeyBinding On(KeyBindingEventType eventType)
    {
        var keyBinding = this.Component as KeyBinding;
        if (keyBinding != null)
        {
            
        }
        KeyEventType = eventType;
        return this;
    }

    /// <summary>
    /// When invoked Alt must be pressed along with 'Key' for the command to be invoked
    /// </summary>
    /// <returns>This to respect chaining.</returns>
    public ModelKeyBinding RequireAlt()
    {
        Alt = true;
        return this;
    }

    /// <summary>
    /// When invoked Control must be pressed along with 'Key' for the command to be invoked
    /// </summary>
    /// <returns>This to respect chaining.</returns>
    public ModelKeyBinding RequireControl()
    {
        Control = true;
        return this;
    }

    /// <summary>
    /// When invoked Shift must be pressed along with 'Key' for the command to be invoked
    /// </summary>
    /// <returns>This to respect chaining.</returns>
    public ModelKeyBinding RequireShift()
    {
        Shift = true;
        return this;
    }
}