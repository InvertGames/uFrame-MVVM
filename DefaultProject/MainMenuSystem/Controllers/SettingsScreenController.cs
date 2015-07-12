using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.MVVM;
using UniRx;
using uFrame.Serialization;
using uFrame.IOC;
using uFrame.Kernel;
using UnityEngine;

/*
 * This example shows initializing of a viewmodel. Anytime you create a viewmodel using Contoller.Create{viewmodeltype}
 * instance goes through Initialize method, which you can use to your advantage.
 */

public class SettingsScreenController : SettingsScreenControllerBase
{

    [Inject] public SettingsService SettingsService;

    public override void InitializeSettingsScreen(SettingsScreenViewModel viewModel) {
        base.InitializeSettingsScreen(viewModel);

        /* Add known resolutions to the list */
        viewModel.AvailableResolutions.AddRange(SettingsService.AvailableResolutions);
        
        /* Setup current resolution */
        viewModel.Resolution = SettingsService.Resolution;
        
        /* Setup volume */
        viewModel.Volume = SettingsService.Volume;
    }

    public override void Apply(SettingsScreenViewModel viewModel) {
        base.Apply(viewModel);
        
        /* Pass selected values to the service */
        SettingsService.Resolution = viewModel.Resolution;
        SettingsService.Volume = viewModel.Volume;
    }


    /* 
     * Home Work: implement this command so, that settings are reverted to default values.
     * Hint: Declare default settings in the SettingsService.
     * Don't forget to update both SettingsService and SettingsScreen
     */
    public override void Default(SettingsScreenViewModel viewModel) {
        base.Default(viewModel);
    }
}
