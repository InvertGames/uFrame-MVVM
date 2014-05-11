using UnityEngine;

public enum KeyBindingEventType
{
    Key,
    KeyDown,
    KeyUp
}

/// <summary>
/// A component that will process a key binding as well as provide a key binding instance to the source view.
/// Note.  Even when adding this binding via code the component will still be added
/// because a component is needed to process a keypress
/// </summary>
public class KeyBinding : ComponentCommandBinding
{
    public bool _Alt;
    public bool _Control;
    public KeyCode _Key;
    public KeyBindingEventType _KeyEventType = KeyBindingEventType.KeyDown;
    public bool _Shift;

    /// <summary>
    /// The binding provider.  Create the binding that the component will add to the source view here.
    /// </summary>
    /// <returns>The binding that will be added to the source view.</returns>
    protected override IBinding GetBinding()
    {
        return new ModelKeyBinding(_Key)
        {
            KeyEventType = _KeyEventType,
            Alt = _Alt,
            Control = _Control,
            Shift = _Shift,
            Source = _SourceView,
            ModelMemberName = _ModelMemberName,
        };
    }

    protected virtual bool IsKey(ModelKeyBinding keyBinding)
    {
        switch (keyBinding.KeyEventType)
        {
            case KeyBindingEventType.Key:
                return Input.GetKey(keyBinding.Key);

            case KeyBindingEventType.KeyDown:
                return Input.GetKeyDown(keyBinding.Key);

            case KeyBindingEventType.KeyUp:
                return Input.GetKeyUp(keyBinding.Key);
        }
        return Input.GetKeyDown(keyBinding.Key);
    }

    protected void Update()
    {
        if (Binding == null) return;
//        if (Binding.Source.enabled)
//        {
            var keyBinding = ((ModelKeyBinding)Binding);
            if (keyBinding == null) return; // Not sure how this could ever happen but nice to be safe

            if (IsKey(keyBinding))
            {
                if (keyBinding.Shift && !(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) return;
                if (keyBinding.Alt && !(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))) return;
                if (keyBinding.Control && !(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) return;
                CommandBinding.ExecuteCommand();
            }
//        }
    }
}