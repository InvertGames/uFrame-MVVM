
using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public class WavesFPSGameManager : WavesFPSGameManagerBase {
    
    public override IEnumerator Load(UpdateProgressDelegate progress)
    {
        // This is for the future scene serialization :) But not implemented entirely yet.
        for (int index = 0; index < RootViews.Count; index++)
        {
            var persistableView = RootViews[index];
            progress("Loading " + persistableView.name, (1f / RootViews.Count) * (index + 1));
        }
        yield break;
    }

    public override void OnLoaded()
    {
        base.OnLoaded();
        StartCoroutine(WavesFPSGameController.StartGame());
    }
}
