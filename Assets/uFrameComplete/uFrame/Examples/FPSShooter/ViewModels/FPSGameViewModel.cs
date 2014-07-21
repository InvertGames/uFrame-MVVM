using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class FPSGameViewModel : ViewModel
{
    public FPSGameViewModel()
    {
    }

    public FPSGameViewModel(FPSGameController controller)
    {
        Controller = controller;
    }

    //protected override void WireCommands(Controller controller)
    //{
    //    var fpsGameController = controller as FPSGameController;
    //    if (fpsGameController != null)
    //    {
            
    //        MainMenu = new Command(fpsGameController.MainMenu);
    //        QuitGame = new Command(fpsGameController.QuitGame);
    //    }
    //    else
    //    {
    //        base.WireCommands(controller);
    //    }
    //}
}