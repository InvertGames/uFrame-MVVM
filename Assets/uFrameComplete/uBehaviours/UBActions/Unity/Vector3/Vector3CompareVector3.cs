using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("Vector3")]
public class Vector3CompareVector3 : UBConditionAction
{
    public UBVector3 _Lhs = new UBVector3();
    public UBVector3 _Rhs = new UBVector3();
    public override bool PerformCondition(IUBContext context)
    {
        return _Lhs.GetValue(context) == _Rhs.GetValue(context);

    }
}