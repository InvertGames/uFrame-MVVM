using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public class MainMenuSceneManager : MainMenuSceneManagerBase {
    
    public override System.Collections.IEnumerator Load(UpdateProgressDelegate progress) {
        // Use the controllers to create the game.
        yield break;
    }
    
    public override void Setup() {
        base.Setup();
    }
}
