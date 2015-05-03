namespace uFrame.DefaultProject {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    
    
    public class GameSceneLoader : GameSceneLoaderBase {
        
        protected override IEnumerator LoadScene(GameScene scene, Action<float, string> progressDelegate) {
            for (var i = 0; i < 100; i++)
            {
                progressDelegate(i*0.01f, string.Format("Loaded {0} %", i *0.01f));
                yield return new WaitForSeconds(0.05f);
            }
            yield break;
        }
        
        protected override IEnumerator UnloadScene(GameScene scene, Action<float, string> progressDelegate) {
            yield break;
        }
    }
}
