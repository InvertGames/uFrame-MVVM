
using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public class IntroductionSceneManager : IntroductionSceneManagerBase {
    
    public override System.Collections.IEnumerator Load(UpdateProgressDelegate progress) {
        // Use the controllers to create the game.
        progress("Loading something important.", 0.2f);
        yield return new WaitForSeconds(3f);
        progress("Complete.", 1f);
        yield return new WaitForSeconds(0.5f);
    }
    
    public override void Setup() {
        base.Setup();
    }
}
