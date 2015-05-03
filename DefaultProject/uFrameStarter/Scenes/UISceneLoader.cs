namespace uFrame.DefaultProject {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    
    
    public class UISceneLoader : UISceneLoaderBase {
        
        protected override IEnumerator LoadScene(UIScene scene, Action<float, string> progressDelegate) {
            yield break;
        }
        
        protected override IEnumerator UnloadScene(UIScene scene, Action<float, string> progressDelegate) {
            yield break;
        }
    }
}
