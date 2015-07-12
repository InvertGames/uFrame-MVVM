using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.Kernel;
using uFrame.MVVM;
using uFrame.MVVM.Services;
using uFrame.MVVM.Bindings;
using uFrame.Serialization;
using UniRx;
using UnityEngine;

/*
 * This view holds different bindings in the base class (LoginScreenViewBase)
 * For example LoginCommand is bound to a specific button. You can set this button in the inspector,
 * or you can turn the binding off completely.
 */
public class LoginScreenView : LoginScreenViewBase {
    
    protected override void InitializeViewModel(uFrame.MVVM.ViewModel model) {
        base.InitializeViewModel(model);
    }
    
    public override void Bind() {
        base.Bind();
    }

}
