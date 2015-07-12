using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.MVVM;
using UniRx;
using uFrame.Serialization;
using uFrame.IOC;
using uFrame.Kernel;


public class MenuScreenController : MenuScreenControllerBase {
    
    public override void InitializeMenuScreen(MenuScreenViewModel viewModel) {
        base.InitializeMenuScreen(viewModel);
        // This is called when a MenuScreenViewModel is created
    }
}
