using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using uFrame.MVVM;
using UniRx;
using uFrame.Serialization;
using uFrame.IOC;
using uFrame.Kernel;
using Debug = UnityEngine.Debug;


/*
 * This controller shows how to use direct service reference, instead of events.
 * 
 */
public class LoginScreenController : LoginScreenControllerBase
{

    /*
     * Directly inject desired service. Notice that you introduce some coupling this way.
     */
    [Inject] public UserManagementService UserManagementService;

    public override void InitializeLoginScreen(LoginScreenViewModel viewModel) {
        base.InitializeLoginScreen(viewModel);
        // This is called when a LoginScreenViewModel is created
    }

    public override void Login(LoginScreenViewModel viewModel) {
        base.Login(viewModel);
        /* Direct call to the service. */
        UserManagementService.AuthorizeLocalUser(viewModel.Username,viewModel.Password);
    }

}
