namespace uFrame.DefaultProject {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UniRx;
    
    
    public class LoadingScreenController : LoadingScreenControllerBase {
        
        public override void Setup() {
            base.Setup();
            // This is called when the controller is created
        }
        
        public override void InitializeLoadingScreen(LoadingScreenViewModel viewModel) {
            base.InitializeLoadingScreen(viewModel);
            // This is called when a LoadingScreenViewModel is created

        }
        
    }
}
