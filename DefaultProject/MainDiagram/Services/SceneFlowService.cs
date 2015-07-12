using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Policy;
using uFrame.Kernel;
using uFrame.IOC;
using UniRx;
using UnityEngine;
using uFrame.MVVM;

/*
 * This service defines the scene flow in your game
 * Based on different events, it transitions from one scene to another
 * Notice that this service DOES NOT DEFINE HOW to load scenes. 
 * It DOES DEFINE WHEN to load them. Please refere to SceneManagementService
 * to check how uFrame handles scene loading.
 */
public class SceneFlowService : SceneFlowServiceBase {
    
    /*
     * This is invoked when someone publishes IntroFinishedEvent (ex. IntroSceneFlow.cs)
     * We handle this event to get the moment, when Intro is finished, so we can move to
     * AssetLoadingScene
     */
    public override void IntroFinishedEventHandler(IntroFinishedEvent data)
    {
        base.IntroFinishedEventHandler(data);

        this.Publish(new UnloadSceneCommand() //Unload Intro scene
        {
            SceneName = "IntroScene"
        });

        this.Publish(new LoadSceneCommand() // Load AssetsLoadingScene
        {
            SceneName = "AssetsLoadingScene"
        });

    }

    /*
     * We handle this event to get the moment, when asset loading is finished, so we can move to
     * Main Menu scene
     */
    public override void AssetLoadingProgressEventHandler(AssetLoadingProgressEvent data)
    {
        base.AssetLoadingProgressEventHandler(data);

        if (data.Progress != 1f) return; //This is the key part: we check that asset loading procedure is finished

        this.Publish(new UnloadSceneCommand() //Unload AssetsLoadingScene
        {
            SceneName = "AssetsLoadingScene"
        });

        this.Publish(new LoadSceneCommand() // Load MainMenuScene
        {
            SceneName = "MainMenuScene"
        });
    }

}

public class LoadScenesWithScreenCommand
{
    public List<string> ScenesToLoad;
}
