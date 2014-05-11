using UnityEngine;

public class InputBinding : ComponentCommandBinding
{
    public string _ButtonName;
    public InputButtonEventType _EventType;

    public void Update()
    {
        var binding = Binding as ModelInputButtonBinding;
        if (binding == null) return;

        switch (binding.EventType)
        {
            case InputButtonEventType.Button:
                if (Input.GetButton(binding.ButtonName))
                {
                    binding.ExecuteCommand();
                }
                break;

            case InputButtonEventType.ButtonDown:
                if (Input.GetButtonDown(binding.ButtonName))
                {
                    binding.ExecuteCommand();
                }
                break;

            case InputButtonEventType.ButtonUp:
                if (Input.GetButtonUp(binding.ButtonName))
                {
                    binding.ExecuteCommand();
                }
                break;
        }
    }

    protected override IBinding GetBinding()
    {
        return new ModelInputButtonBinding()
        {
            ButtonName = _ButtonName,
            Source = _SourceView,
            ModelMemberName = _ModelMemberName,
            EventType = _EventType
        };
    }
}

public class MouseEventBinding : ComponentCommandBinding
{
    public MouseEventType _EventType;

    protected override IBinding GetBinding()
    {
        return new ModelMouseEventBinding()
        {
            Source = _SourceView,
            ModelMemberName = _ModelMemberName,
            EventType = _EventType
        };
    }

    protected virtual void OnBecameInvisible()
    {
        HandleEvent(MouseEventType.OnBecameInvisible);
    }

    protected virtual void OnBecameVisible()
    {
        HandleEvent(MouseEventType.OnBecameVisible);
    }

    protected virtual void OnMouseDown()
    {
        HandleEvent(MouseEventType.OnMouseDown);
    }

    protected virtual void OnMouseDrag()
    {
        HandleEvent(MouseEventType.OnMouseDrag);
    }

    protected virtual void OnMouseEnter()
    {
        HandleEvent(MouseEventType.OnMouseEnter);
    }

    protected virtual void OnMouseExit()
    {
        HandleEvent(MouseEventType.OnMouseExit);
    }

    protected virtual void OnMouseOver()
    {
        HandleEvent(MouseEventType.OnMouseOver);
    }

    protected virtual void OnMouseUp()
    {
        HandleEvent(MouseEventType.OnMouseUp);
    }

    protected virtual void OnMouseUpAsButton()
    {
        HandleEvent(MouseEventType.OnMouseUpAsButton);
    }

    private void HandleEvent(MouseEventType eventType)
    {
        var binding = Binding as ModelMouseEventBinding;
        if (binding == null) return;
        //if (binding.SourceView == null) return;
        //if (!binding.SourceView.enabled) return;
        if (eventType == binding.EventType)
        {
            binding.ExecuteCommand();
        }
    }
}