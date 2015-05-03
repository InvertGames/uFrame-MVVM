namespace uFrame.DefaultProject {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    
    
    public class GameSceneLoader : GameSceneLoaderBase {
        
        protected override IEnumerator LoadScene(GameScene scene, Action<float, string> progressDelegate) {
            if (scene.Settings == null)
            {
                // Here is a good place to create some default settings
                scene.Settings = new GameSceneSettings()
                {
                    // etc
                };
            }

            // Some long running progress
            for (var i = 0; i < 100; i++)
            {
                progressDelegate(i*0.01f, string.Format("Loaded {0} %", i *0.01f));
                yield return new WaitForSeconds(0.05f);
            }
            
        }
        
        protected override IEnumerator UnloadScene(GameScene scene, Action<float, string> progressDelegate) {
            yield break;
        }
    }
}
