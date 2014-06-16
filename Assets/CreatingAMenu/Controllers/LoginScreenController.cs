using System;
using System.Collections;
using System.Linq;


public class LoginScreenController : LoginScreenControllerBase {
    
    public override void InitializeLoginScreen(LoginScreenViewModel loginScreen) {
    }

    public override void Login(LoginScreenViewModel loginScreen)
    {
        base.Login(loginScreen);
        // Check the credentials.
        if (loginScreen.Username == "uframe" && loginScreen.Password == "uframe")
        {
            MainMenuSceneController.ShowWindow<MainMenuScreenViewModel>();
        }
        else
        {
            // Oops there was an error set the error message.
            loginScreen.ErrorMessage = "The username and password is \"uframe\".";
        }
        
    }
}
