using UnityEngine;

public class ViewModelCollectionBinding : ComponentBinding
{
    public bool _Immediate;
    public Transform _Parent;
    public Component _TargetComponent;
    public string _ViewName;

    protected override IBinding GetBinding()
    {
        return new ModelViewModelCollectionBinding()
        {
            ModelMemberName = _ModelMemberName,
            Source = _SourceView,
            Parent = _Parent,
            IsImmediate = _Immediate,
            ViewName = _ViewName,
            OnAddView = OnAddView,
            OnRemoveView = OnRemoveView
        };
    }

    private void OnAddView(ViewBase obj)
    {
    }

    private void OnRemoveView(ViewBase obj)
    {
    }
}