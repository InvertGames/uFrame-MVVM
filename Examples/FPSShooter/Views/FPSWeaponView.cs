using System.Globalization;
using UniRx;
using UnityEngine;

public partial class FPSWeaponView
{ 

    public float _CrossHairScale = 0.5f;
 
    public Transform _ZoomPositions;
    public Texture2D _HudTexture;
    private Vector3 _StartPosition;
    private Quaternion _StartRotation;
    public Transform _ModelTransform;
    
    public override void AmmoChanged(int value)
    {
        base.AmmoChanged(value);
        
        if (value <= 0 && FPSWeapon.State == FPSWeaponState.Firing)
        {
            // Executes End Fire on the controller
            ExecuteEndFire();
        }
    }

    public override void ZoomIndexChanged(int zoomIndex)
    {
        base.ZoomIndexChanged(zoomIndex);
        FPSCrosshair.ResetCrosshair();
        var zoomTransform = transform.FindChild(zoomIndex.ToString(CultureInfo.InvariantCulture));
        if (zoomTransform != null)
        {
            _StartPosition = zoomTransform.localPosition;
            _StartRotation = zoomTransform.localRotation;
        }
    }

    public override void StateChanged(FPSWeaponState value)
    {
        if (!this.gameObject.activeInHierarchy) return;
        if (!this.gameObject.activeSelf) return;
        // If we are reloading hide the gun
        _ModelTransform.gameObject.SetActive(value != FPSWeaponState.Reloading);
        
        if (value == FPSWeaponState.Firing)
        {
            // Tell the Weapon Fire View Component to start firing
            FPSWeaponFire.Fire();
        }
        else if (value != FPSWeaponState.InActive)
        {
            // Tell the Weapon Fire View Component to stop firing
            FPSWeaponFire.StopFiring();
        }
    }

    public override void Bind()
    {
     
        base.Bind();

        this.BindKey(() => FPSWeapon.NextZoom, KeyCode.LeftShift);
        this.BindKey(() => FPSWeapon.Reload, KeyCode.R);
        FPSCrosshair.ResetCrosshair();
    }

    public void Update()
    {
        if (_ModelTransform == null) return;
        _ModelTransform.localPosition = Vector3.Lerp(_ModelTransform.localPosition, _StartPosition, FPSWeapon.RecoilSpeed);
        _ModelTransform.localRotation = Quaternion.Lerp(_ModelTransform.localRotation, _StartRotation, FPSWeapon.RecoilSpeed);

        if (gameObject.activeSelf && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftControl)))
        {
            if (FPSWeapon.State == FPSWeaponState.Active)
            {
                ExecuteBeginFire();
            
            }
        }
        if (gameObject.activeSelf && (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.LeftControl)))
        {
            ExecuteEndFire();
        }
    
    }

}