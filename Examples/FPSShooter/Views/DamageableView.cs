using System;
using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;


public abstract partial class DamageableView {
    public override void Bind() {
        base.Bind();
    }
    
    protected override IObservable<Vector3> GetPositionObservable()
    {
        return PositionAsObservable;
    }

    public override void PositionChanged(Vector3 value)
    {
        base.PositionChanged(value);
        Debug.Log("Position has changed", this);
    }
}
