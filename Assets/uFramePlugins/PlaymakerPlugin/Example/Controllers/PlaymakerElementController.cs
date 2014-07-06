using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlaymakerElementController : PlaymakerElementControllerBase
{
    protected Dictionary<int, TimeSpan> UpgradeTable = new Dictionary<int, TimeSpan>();

    public PlaymakerElementController()
    {
        UpgradeTable.Add(0,new TimeSpan(0,0,0,2));
        UpgradeTable.Add(1,new TimeSpan(0,0,0,4));
        UpgradeTable.Add(2,new TimeSpan(0,0,0,8));
        UpgradeTable.Add(3,new TimeSpan(0,0,0,12));
    }


    public override void InitializePlaymakerElement(PlaymakerElementViewModel playmakerElement) {
        // Ensure when loading that tick is called and applies any data changes.
        Tick(playmakerElement);
    }



    public override void Kill(PlaymakerElementViewModel playmakerElement)
    {
        base.Kill(playmakerElement);
        // Simply set the state to dead
        playmakerElement.State = PlaymakerElementState.Dead;
    }

    public override void Tick(PlaymakerElementViewModel playmakerElement)
    {
        base.Tick(playmakerElement);
        if (DateTime.UtcNow >= playmakerElement.UpgradeCompleteTime)
        {
            playmakerElement.State = PlaymakerElementState.Idle;
            playmakerElement.CurrentLevel++;
        }
    }

    public override void Upgrade(PlaymakerElementViewModel playmakerElement)
    {
        base.Upgrade(playmakerElement);
        playmakerElement.State = PlaymakerElementState.Upgrading;
        playmakerElement.UpgradeCompleteTime = DateTime.UtcNow.Add(UpgradeTable[playmakerElement.CurrentLevel]);
    }
}
