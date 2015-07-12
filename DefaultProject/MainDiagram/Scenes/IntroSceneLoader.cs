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
 * This class defines loading procedure for IntroScene.
 * When you load/unload any scene marked as IntroScene, this loader will be used
 * and corresponding action (Load/Unload) will be invoked, passing you the SceneType instance and progress delegate,
 * which you can use to report progress.
 * Notice that both methods are Coroutines. Use it to your advantage.
 */
public class IntroSceneLoader : IntroSceneLoaderBase {
    
    protected override IEnumerator LoadScene(IntroScene scene, Action<float, string> progressDelegate) {
        yield break;
    }
    
    protected override IEnumerator UnloadScene(IntroScene scene, Action<float, string> progressDelegate) {
        yield break;
    }
}
