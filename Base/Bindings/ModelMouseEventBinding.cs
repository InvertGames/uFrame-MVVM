public enum InputButtonEventType
{
    Button,
    ButtonDown,
    ButtonUp
}

public class ModelInputButtonBinding : ModelCommandBinding
{
    public string ButtonName { get; set; }

    public InputButtonEventType EventType { get; set; }
}

public class ModelMouseEventBinding : ModelCommandBinding
{
    public MouseEventType EventType { get; set; }
}