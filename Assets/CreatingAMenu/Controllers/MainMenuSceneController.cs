using System;
using System.Collections;
using System.Linq;


public class MainMenuSceneController : MainMenuSceneControllerBase {

    public override void InitializeMainMenuScene(MainMenuSceneViewModel mainMenuScene)
    {
        // Show our initial screen
        ShowWindow(mainMenuScene.StartScreen);
    }
    
    public void ShowWindow(MenuScreenViewModel screen)
    {
        // Set all the windows to false.  If it is already false no bindings will be triggered.
        foreach (var menuScreen in MainMenuScene.MenuScreens)
        {
            menuScreen.Active = false;
        }
        // Finally set the screen we want to be active
        screen.Active = true;
    }

    public void ShowWindow<TMenuScreen>() where TMenuScreen : MenuScreenViewModel
    {
        // Find the menu screen in the screens collection
        var menuScreen = MainMenuScene.MenuScreens.OfType<TMenuScreen>().FirstOrDefault();
        // If we've found the menu screen
        if (menuScreen != null)
        {
            // Show the screen
            ShowWindow(menuScreen);
        }
        else
        {
            throw new Exception("Problem finding menu screen.");
        }
    }
}
