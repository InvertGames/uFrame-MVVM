[UBCategory("Vector3")]
public class Vector3CompareInt : UBConditionAction
{
    public UBInt _Lhs = new UBInt();
    public UBInt _Rhs = new UBInt();
    public override bool PerformCondition(IUBContext context)
    {
        return _Lhs.GetValue(context) == _Rhs.GetValue(context);

    }
}