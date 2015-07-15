using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.MVVM;
using UniRx;
using uFrame.Serialization;
using uFrame.IOC;
using uFrame.Kernel;


public class LevelSelectScreenController : LevelSelectScreenControllerBase
{

    /* Inject service for direct communication */
    [Inject] public LevelManagementService LevelManagementService;

    public override void InitializeLevelSelectScreen(LevelSelectScreenViewModel viewModel) {
        base.InitializeLevelSelectScreen(viewModel);

        /*
         * Add available levels to the LevelSelectScreen viewmodel
         */
        foreach (var levelDescriptor in LevelManagementService.Levels)
        {
            viewModel.AvailableLevels.Add(levelDescriptor);
        }

    }

    public override void SelectLevel(LevelSelectScreenViewModel viewModel, LevelDescriptor arg) {
        base.SelectLevel(viewModel, arg);
        
        // This can be extracted into a service to load levels from other places
        // without duplicated code. Also external service could handle rules,
        // under which certain level can/cannot be loaded

        if (arg.IsLocked) return; //Level is locked, do not switch scenes

        Publish(new UnloadSceneCommand()
        {
            SceneName = "MainMenuScene" // Unload  main menu scene
        });

        Publish(new LoadSceneCommand()
        {
            SceneName = arg.LevelScene // Load level scene
        });
            

    }
}
