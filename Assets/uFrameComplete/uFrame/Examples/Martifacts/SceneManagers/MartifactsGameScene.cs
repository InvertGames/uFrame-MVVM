using System;
using System.Collections;
using System.Linq;


public class MartifactsGameScene : MartifactsGameSceneBase {
    
    public override System.Collections.IEnumerator Load(UpdateProgressDelegate progress) {
        // Use the controllers to create the game.
        yield break;
    }

    public override void OnLoaded()
    {
        base.OnLoaded();
        MartifactsGameController.RoverMoved(MartifactsGameController.MartifactsGame.Rover);
    }

    public override void Setup() {
        base.Setup();
    }
}
