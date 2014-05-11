
using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public class GUIGameSceneManager : GUIGameSceneManagerBase {
    
    public override System.Collections.IEnumerator Load(UpdateProgressDelegate progress) {
        // Use the controllers to create the game.
        progress("Loading something awesome",0.5f);
        yield return new WaitForSeconds(3f);
        progress("Complete",1f);
        yield return new WaitForSeconds(1f);
    }
    
    public override void Setup() {
        base.Setup();
    }
}
