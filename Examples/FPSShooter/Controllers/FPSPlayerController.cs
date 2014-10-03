using System.Collections;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class FPSPlayerController : FPSPlayerControllerBase
{
    
    public override void NextWeapon(FPSPlayerViewModel fPsPlayer)
    {
        fPsPlayer.CurrentWeapon.State = FPSWeaponState.Active;
        if (fPsPlayer.CurrentWeaponIndex == fPsPlayer.Weapons.Count - 1)
        {
            fPsPlayer.CurrentWeaponIndex = 0;
        }
        else
        {
            fPsPlayer.CurrentWeaponIndex++;
        }
        
    }

    public override void PickupWeapon( FPSPlayerViewModel fPsPlayer, FPSWeaponViewModel fpsWeaponViewModel)
    {
        fPsPlayer.Weapons.Add(fpsWeaponViewModel);
        
    }

    public override void PreviousWeapon(FPSPlayerViewModel fPsPlayer)
    {
        if (fPsPlayer.CurrentWeaponIndex == 0)
        {
            fPsPlayer.CurrentWeaponIndex = fPsPlayer.Weapons.Count - 1;
        }
        else
        {
            fPsPlayer.CurrentWeaponIndex--;
        }
    }

    public override void InitializeFPSPlayer(FPSPlayerViewModel player)
    {
        
    }

    public override void SelectWeapon( FPSPlayerViewModel fPsPlayer, int index)
    {
        if (index > fPsPlayer.Weapons.Count) return;
        fPsPlayer.CurrentWeaponIndex = index;
    }
} 