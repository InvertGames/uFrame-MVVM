using UnityEngine;

public class M16WeaponViewModel : FPSWeaponViewModel
{
    public readonly P<bool> _Visible = new P<bool>(true);

    public bool Visible
    {
        get { return _Visible.Value; }
        set { _Visible.Value = value; }
    }

}