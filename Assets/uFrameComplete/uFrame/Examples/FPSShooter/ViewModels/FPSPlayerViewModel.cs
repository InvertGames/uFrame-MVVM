using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class FPSPlayerViewModel : FPSDamageableViewModel
{
    public FPSWeaponViewModel CurrentWeapon
    {
        get { return _WeaponsProperty.Value[CurrentWeaponIndex]; }
    }
    public virtual IEnumerable<ModelPropertyBase> GetProperties()
    {
        yield return _CurrentWeaponIndexProperty;
        yield return _WeaponsProperty;

    }
}