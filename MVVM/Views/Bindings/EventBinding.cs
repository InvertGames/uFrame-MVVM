///// <summary>
///// The event binding component that will add an event binding to a source view.
///// </summary>
//public class EventBinding : ComponentCommandBinding
//{
//    public string _EventName;

//    protected override IBinding GetBinding()
//    {
//        return new ModelEventBinding(_EventName)
//        {
//            EventName = _EventName,
//            ModelMemberName = _ModelMemberName,
//            Source = _SourceView.ViewModelObject,
//        };
//    }

//    protected override void Awake()
//    {
//        base.Awake();
//    }
//}