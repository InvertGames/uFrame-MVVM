using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.IOC;
using uFrame.Kernel;
using uFrame.MVVM;
using uFrame.Serialization;
using UnityEngine;


public class MainMenuSceneLoader : MainMenuSceneLoaderBase
{

    protected override IEnumerator LoadScene(MainMenuScene scene, Action<float, string> progressDelegate) {
        yield break;
    }
    
    protected override IEnumerator UnloadScene(MainMenuScene scene, Action<float, string> progressDelegate) {
        yield break;
    }

}
