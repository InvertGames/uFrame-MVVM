using System;
using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;


public abstract partial class DamageableView {
    
    public override void Bind() {
        base.Bind();
    }

    //protected override IObservable<Vector3> GetPositionObservable()
    //{
    //    return base.GetPositionObservable();
    //}

    //protected override Vector3 GetPosition()
    //{
    //    return this.transform.position;
    //}
}
