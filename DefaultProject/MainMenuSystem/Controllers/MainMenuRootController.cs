using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.MVVM;
using uFrame.Serialization;
using uFrame.IOC;
using uFrame.Kernel;
using UniRx;



/*
 * You usually want to use controller for handling logic, which is only based
 * on the view model of related type.
 * It turns out, that most of the time controller is used to publish different events to services.
 */
public class MainMenuRootController : MainMenuRootControllerBase
{

    public override void InitializeMainMenuRoot(MainMenuRootViewModel viewModel) {
        base.InitializeMainMenuRoot(viewModel);
    }

}
