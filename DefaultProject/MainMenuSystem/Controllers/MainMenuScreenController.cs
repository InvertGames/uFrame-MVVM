namespace uFrame.DefaultProject {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UniRx;
    
    
    public class MainMenuScreenController : MainMenuScreenControllerBase {
        
        public override void Setup() {
            base.Setup();
            // This is called when the controller is created
        }
        
        public override void InitializeMainMenuScreen(MainMenuScreenViewModel viewModel) {
            base.InitializeMainMenuScreen(viewModel);
            // This is called when a MainMenuScreenViewModel is created
        }
        
        public override void PlayGame(MainMenuScreenViewModel viewModel) {
            base.PlayGame(viewModel);
            this.Publish(new BeginGameCommand()
            {
                
                // Add options here
            });
        }
    }
}
