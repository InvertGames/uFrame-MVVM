using System;
using System.Collections.Generic;
using UnityEngine;

public partial class FPSPlayerView
{
    public List<FPSWeaponViewBase> _WeaponsList = new List<FPSWeaponViewBase>();
    
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

    public override void WeaponsAdded(ViewBase item)
    {
        base.WeaponsAdded(item);
        _WeaponsList.Add(item as FPSWeaponViewBase);
    }

    public override void WeaponsRemoved(ViewBase item)
    {
        base.WeaponsRemoved(item);
        _WeaponsList.Remove(item as FPSWeaponViewBase);
    }

    public override void Bind()
    {
        
        base.Bind();

        this.BindKey(() => FPSPlayer.SelectWeapon, KeyCode.Alpha1, 0);
        this.BindKey(() => FPSPlayer.SelectWeapon, KeyCode.Alpha2, 1);
        this.BindKey(() => FPSPlayer.SelectWeapon, KeyCode.Alpha3, 2);
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