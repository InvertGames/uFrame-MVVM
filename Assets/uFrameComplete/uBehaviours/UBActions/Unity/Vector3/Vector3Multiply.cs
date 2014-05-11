using System;
using System.Collections.Generic;
using UnityEngine;

[UBCategory("Vector3")]
public class Vector3Multiply : UBAction
{

    public UBVector3 _A = new UBVector3();
    public UBFloat _D = new UBFloat();
    public UBVector3 _Result = new UBVector3();
    protected override void PerformExecute(IUBContext context)
    {
        if (_Result != null)
        {
            _Result.SetValue(context, _A.GetValue(context) * _D.GetValue(context));
        }

    }

    public override string ToString()
    {
        return string.Format("{0} * {1}", _A.ToString(RootContainer), _D.ToString(RootContainer));
    }
}