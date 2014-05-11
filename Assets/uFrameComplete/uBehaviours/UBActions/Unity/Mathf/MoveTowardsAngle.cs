using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Mathf")]
public class MoveTowardsAngle : UBAction
{

    [UBRequired]
    public UBFloat _Current = new UBFloat();
    [UBRequired]
    public UBFloat _Target = new UBFloat();
    [UBRequired]
    public UBFloat _MaxDelta = new UBFloat();
    [UBRequireVariable]
    [UBRequired]
    public UBFloat _Result = new UBFloat();
    protected override void PerformExecute(IUBContext context)
    {
        if (_Result != null)
        {
            _Result.SetValue(context, Mathf.MoveTowardsAngle(_Current.GetValue(context), _Target.GetValue(context), _MaxDelta.GetValue(context)));
        }

    }

    public override string ToString()
    {
        return string.Format("Move {0} Towards {1} Angle ",
            _Current.ToString(RootContainer), _Target.ToString(RootContainer));
    }

    public override void WriteCode(IUBCSharpGenerator sb)
    {
        sb.AppendExpression("Mathf.MoveTowardsAngle(#_Current#, #_Target#, #_MaxDelta#)");
    }

}