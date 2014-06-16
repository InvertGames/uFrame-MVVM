using System;
using System.Collections;
using System.Linq;


public class MainMenuScreenController : MainMenuScreenControllerBase {
    
    public override void InitializeMainMenuScreen(MainMenuScreenViewModel mainMenuScreen) {

    }

    public override void Play(MainMenuScreenViewModel mainMenuScreen)
    {
        base.Play(mainMenuScreen);
        // Call our root scene playgame command
        MainMenuSceneController.PlayGame();
    }

    public override void LogOff(MainMenuScreenViewModel mainMenuScreen)
    {
        base.LogOff(mainMenuScreen);
        MainMenuSceneController.ShowWindow<LoginScreenViewModel>();
    }
}
