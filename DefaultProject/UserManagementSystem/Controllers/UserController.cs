using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.MVVM;
using UniRx;
using uFrame.Serialization;
using uFrame.IOC;
using uFrame.Kernel;


public class UserController : UserControllerBase {
    
    public override void InitializeUser(UserViewModel viewModel) {
        base.InitializeUser(viewModel);
        // This is called when a UserViewModel is created
    }
}
