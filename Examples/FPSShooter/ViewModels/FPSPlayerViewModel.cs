using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class FPSPlayerViewModel : FPSDamageableViewModel
{
    public FPSWeaponViewModel CurrentWeapon
    {
        get { return _WeaponsProperty[CurrentWeaponIndex]; }
    }
 
}