
using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public class CheckersSceneManager : CheckersSceneManagerBase {
    public TextAsset _SceneState;
    public override System.Collections.IEnumerator Load(UpdateProgressDelegate progress) {
        // Use the controllers to create the game.
        yield return new WaitForEndOfFrame();
        if (_SceneState != null)
        {
            Context.Load(new StringSerializerStorage() { Result = _SceneState.text }, new JsonStream() { UseReferences = true });
        }
        yield break;
    }
    
    public override void Setup() {
      
        base.Setup();
    }
}
