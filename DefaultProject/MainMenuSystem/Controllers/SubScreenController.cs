using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.MVVM;
using UniRx;
using uFrame.Serialization;
using uFrame.IOC;
using uFrame.Kernel;



/*
 * You usually want to use controller for handling logic, which is only based
 * on the view model of related type.
 * It turns out, that most of the time controller is used to publish different events to services.
 */
public class SubScreenController : SubScreenControllerBase {
    
    public override void InitializeSubScreen(SubScreenViewModel viewModel) {
        base.InitializeSubScreen(viewModel);
    }

    /*
     * Here we handle Close command by requesting MenuScreenViewModel.
     * Indeed, all the SubScreens are tightly coupled with MenuScreen
     * However, it is not difficult at all to introduce more generic solution.
     * Or you can move this logic to the MainMenuService, to avoid this coupling.
     * You'll need to add CloseCommand handler to the MainMenuService and implement it.
     */
    public override void Close(SubScreenViewModel viewModel) {
        base.Close(viewModel);
        Publish(new RequestMainMenuScreenCommand()
        {
            ScreenType = typeof(MenuScreenViewModel)
        });
    }
}
