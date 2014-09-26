
using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public partial class FPSHUDView  { 

    /// Subscribes to the property and is notified anytime the value changes.
    public override void CurrentPlayerChanged(FPSPlayerViewModel value) {
        base.CurrentPlayerChanged(value);
    }
 

    public GUIText _GameOverLabel;
    public GUIText _TotalKillsLabel;

    public override void StateChanged(FPSGameState value)
    {
        _GameOverLabel.gameObject.SetActive(value == FPSGameState.Done);
    }


    public override void KillsChanged(int value)
    {
        _TotalKillsLabel.text = string.Format("Kills: {0}", value);
    }
}
