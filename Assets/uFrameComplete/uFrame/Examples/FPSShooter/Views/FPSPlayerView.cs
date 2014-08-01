using System.Collections.Generic;
using UnityEngine;

public partial class FPSPlayerView : FPSPlayerViewBase
{ 
    public override void WeaponsRemoved(FPSWeaponViewBase item) {
        base.WeaponsRemoved(item);
        this._WeaponsList.Remove(item);
        if (item != null && item.gameObject != null) UnityEngine.Object.Destroy(item.gameObject);
    }
    public override void WeaponsAdded(FPSWeaponViewBase item) {
        base.WeaponsAdded(item);
        this._WeaponsList.Add(item);
    }
 
    
    //public Transform _GunsTransform;
    //public List<ViewBase> _Weapons = new List<ViewBase>();
    public override void Awake()
    {
        base.Awake();
        AddWeapon("MP5Weapon");
        AddWeapon("UMP5Weapon");
        AddWeapon("ColtWeapon");
    }

    public override ViewBase CreateWeaponsView(FPSWeaponViewModel fPSWeapon)
    {
        var prefabName = fPSWeapon.WeaponType.ToString() + "Weapon";
        return InstantiateView(prefabName,fPSWeapon);
    }

    public override void Bind()
    {
        
        base.Bind();
      

        this.BindKey(() => FPSPlayer.SelectWeapon, KeyCode.Alpha1).SetParameter(0);
        this.BindKey(() => FPSPlayer.SelectWeapon, KeyCode.Alpha2).SetParameter(1);
        this.BindKey(() => FPSPlayer.SelectWeapon, KeyCode.Alpha3).SetParameter(2);
        //this.BindKey(() => FPSPlayer.SelectWeapon, KeyCode.Alpha4).SetParameter(3);
        //this.BindKey(() => FPSPlayer.SelectWeapon, KeyCode.Alpha5).SetParameter(4);

        this.BindKey(() => FPSPlayer.NextWeapon, KeyCode.RightArrow);
        this.BindKey(() => FPSPlayer.PreviousWeapon, KeyCode.LeftArrow);
       
    }

    public override void AfterBind()
    {
        base.AfterBind();
        ExecuteSelectWeapon(0);
    }

    public override void CurrentWeaponIndexChanged(int value)
    {
        base.CurrentWeaponIndexChanged(value);
        Debug.Log("WeaponIndexChanged");
        for (var i = 0; i < this._WeaponsList.Count; i++)
            _WeaponsList[i].gameObject.SetActive(i == value);
    }

    public void AddWeapon(string weaponName)
    {
       
        var weapon = InstantiateView(weaponName, weaponName) as FPSWeaponViewBase;
        weapon.transform.parent = _WeaponsContainer;
        weapon.transform.localEulerAngles = new Vector3(0f,0f,0f);
        weapon.transform.localPosition = new Vector3(0f,0f,0f);
        
    }
}