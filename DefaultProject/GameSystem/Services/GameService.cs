namespace uFrame.DefaultProject {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UniRx;
    
    
    public class GameService : GameServiceBase {

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
            // Queue as many scenes as you want, UIScene, LoadingScene...etc
            SceneManagement.QueueSceneLoadIfNotAlready("UIScene", null);
            SceneManagement.QueueSceneLoadIfNotAlready("GameScene", null);
            // Perform loading all queued
            SceneManagement.ExecuteLoad();
     
         
        }


    }
}
