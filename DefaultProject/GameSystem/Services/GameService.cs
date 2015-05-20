namespace uFrame.DefaultProject {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UniRx;
    
    
    public class GameService : GameServiceBase {

        // Grab the uFrame SceneManagementService
        [Inject]
        public SceneManagementService SceneManagement { get; set; }

        // Get a reference to our defined instance
        [Inject("LoadingScreen")]
        public LoadingScreenViewModel LoadingScreen { get; set; }

        public override void SceneLoaderEventHandler(SceneLoaderEvent data)
        {
            base.SceneLoaderEventHandler(data);
            if (data.State == SceneState.Loaded || data.State == SceneState.Unloaded)
            {
                LoadingScreen.Active = false;
            }
            else
            {
                LoadingScreen.Active = true;
                LoadingScreen.Progress = data.Progress;
                LoadingScreen.Message = data.ProgressMessage;
            }
        }

        public override void GameReadyEventHandler(GameReadyEvent data)
        {
            base.GameReadyEventHandler(data);
     
            // We don't want to load the UIScene if we enter play-mode from the gamescene
            if (Application.loadedLevelName != "GameScene")
            {       
                // This is an example of how to queue multiple scenes by using the
                // Scene managemenet service
                // Queue as many scenes as you want, UIScene, HudScene...etc
                SceneManagement.QueueSceneLoadIfNotAlready("UIScene", null);
            }
            
            // Perform loading all queued
            SceneManagement.ExecuteLoad();
     
         
        }

        public override void BeginGameCommandHandler(BeginGameCommand data)
        {
            base.BeginGameCommandHandler(data);
            // Unloading a scene
            this.Publish(new UnloadSceneCommand()
            {
                SceneName = "UIScene"
            });
            // Loading a scene with publish
            this.Publish(new LoadSceneCommand() 
            { 
                SceneName = "GameScene", 
                Settings = new GameSceneSettings() 
            });
        }
    }
}
