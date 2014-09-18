//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class HUDPlayerView : FPSPlayerViewBase
//{
//    public GUIText _AmmoLabel;
//    public GUIText _ReloadLabel;
//    public List<Texture2D> _HudTextures;


//    public override ViewBase CreateWeaponsView(FPSWeaponViewModel fPSWeapon)
//    {
//        _HudTextures.Add(Resources.Load<Texture2D>("Textures/" + fPSWeapon.WeaponType.ToString() + "WeaponGUI"));
//        return null;
//    }

//    public override void WeaponsAdded(FPSWeaponViewBase fPSWeapon)
//    {
        
//    }

//    public override void WeaponsRemoved(FPSWeaponViewBase fPSWeapon)
//    {
       
      
//    }

//    public void OnGUI()
//    {
//        if (FPSPlayer == null) return;
//        if (FPSPlayer.CurrentWeapon.Ammo < 10)
//            _ReloadLabel.gameObject.SetActive(true);
//        else
//            _ReloadLabel.gameObject.SetActive(false);

//        _AmmoLabel.text = "Ammo: " + FPSPlayer.CurrentWeapon.Ammo;

//        GUIWeapons();
//    }

//    private void GUIWeapons()
//    {
//        var offset = 10f;
//        for (int index = 0; index < _HudTextures.Count; index++)
//        {
//            var weaponTexture = _HudTextures[index];

//            var rect = new Rect(offset, Screen.height - 32 - 10, 64, 32);
//            rect.width += 5;
//            rect.height += 5;

//            if (FPSPlayer.CurrentWeaponIndex == index)
//            {
//                GUI.Box(rect, "");
//            }
//            rect.x += 5f;
//            rect.y += 5f;
//            rect.width -= 10f;
//            rect.height -= 10f;
//            GUI.DrawTexture(rect, weaponTexture,
//                ScaleMode.ScaleToFit, true);

//            offset += (64) + 10f;
//        }
//    }
//}