
using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public class FPSDamageableController : FPSDamageableControllerBase
{

    public override void InitializeFPSDamageable(FPSDamageableViewModel fPSDamageable)
    {
    }

    public override void ApplyDamage(FPSDamageableViewModel fPSDamageable, int arg)
    {
        base.ApplyDamage(fPSDamageable, arg);
        fPSDamageable.Health -= arg;
        if (fPSDamageable.Health <= 0)
        {
            fPSDamageable.State = FPSPlayerState.Dead;
        }

    }

  


}
