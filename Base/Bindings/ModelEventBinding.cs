//public class ModelEventBinding : ModelCommandBinding
//{
//    private string _eventName;

//    public virtual string EventName
//    {
//        get { return _eventName; }
//        set { _eventName = value.ToUpper(); }
//    }

//    public ModelEventBinding(string eventName)
//    {
//        _eventName = eventName.ToUpper();
//    }

//    public override void Bind()
//    {
//        base.Bind();
//        Source.EventTriggered += SourceViewOnEventTriggered;
//    }

//    public override void Unbind()
//    {
//        base.Unbind();
//        Source.EventTriggered -= SourceViewOnEventTriggered;
//    }

//    private void SourceViewOnEventTriggered(string eventName)
//    {
//        if (eventName.ToUpper() == EventName.ToUpper())
//        {
//            ExecuteCommand();
//        }
//    }
//}