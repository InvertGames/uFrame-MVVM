using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.IOC;
using uFrame.Kernel;
using uFrame.MVVM;
using uFrame.Serialization;
using UnityEngine;


public class LevelSceneLoader : LevelSceneLoaderBase
{


    /*
     *  Example of Scene Loader which sets up different ViewModel instances of the game in this scene
     *  Here we assume that every LevelScene has a LevelRoot somewhere around (scene.LevelRoot in this case).
     *  We want to make sure, that once scene is loaded, LevelRoot knows what the current level is.
     *  
     *  We do it specifically in the LevelSceneLoader, because this should happen when we load LevelScene.
     *  With such a setup, it does not matter if we start from MainMenuScene, LevelScene or IntroScene.
     */

    [Inject] public LevelManagementService LevelManagementService;
    protected override IEnumerator LoadScene(LevelScene scene, Action<float, string> progressDelegate)
    {
        /* 
         * Old Chinese trick: When loading scene, some of the components in this scene may not be initialized by unity
         * In this situation, object.GetComponent<>() will not work on some game objects. 
         * That is why we can just skip one frame and let unity finally do it's job.
         */
        yield return null;

        /*
         * When loading scene, make sure LevelRoot knows about what scene it is currently in.
         * Find LevelDescriptor with the corresponding ID and assign it to the CurrentLevel property of the LevelRoot
         */
        scene.LevelRoot.CurrentLevel = LevelManagementService.Levels.FirstOrDefault(level => level.Id == scene.Id);
        yield break;
    }
    
    protected override IEnumerator UnloadScene(LevelScene scene, Action<float, string> progressDelegate) {
        yield break;
    }
}
