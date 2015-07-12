using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.IOC;
using uFrame.Kernel;
using uFrame.MVVM;
using uFrame.Serialization;
using UnityEngine;

/*
 * This class defines loading procedure for AssetsLoadingScene.
 * When you load/unload any scene marked as AssetsLoadingScene, this loader will be used
 * and corresponding action (Load/Unload) will be invoked, passing you the SceneType instance and progress delegate,
 * which you can use to report progress.
 * Notice that both methods are Coroutines. Use it to your advantage.
 */
public class AssetsLoadingSceneLoader : AssetsLoadingSceneLoaderBase {
    
    protected override IEnumerator LoadScene(AssetsLoadingScene scene, Action<float, string> progressDelegate) {
        //We publish this event, so AssetsLoadingService can handle it and start loading assets.
        Publish(new StartAssetLoadingCommand());
        yield break;
    }
    
    protected override IEnumerator UnloadScene(AssetsLoadingScene scene, Action<float, string> progressDelegate) {
        yield break;
    }
}
