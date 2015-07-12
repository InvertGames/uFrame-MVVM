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
using UnityEngine.UI;

/*
 * This view only serves to hold a collection binding for SubScreens in the base class (MainMenuRootViewBase).
 * In the editor, we set "Scene First" to true. Due to this, when scene is loaded,
 * all existings subscreens will be added to the MainMenuRoot.Screens collection.
 */
public class MainMenuRootView : MainMenuRootViewBase
{
    
    protected override void InitializeViewModel(uFrame.MVVM.ViewModel model) {
        base.InitializeViewModel(model);
    }
    
    public override void Bind() {
        base.Bind();
    }

    public override uFrame.MVVM.ViewBase ScreensCreateView(uFrame.MVVM.ViewModel viewModel) {
        return InstantiateView(viewModel);
    }
    
    public override void ScreensAdded(uFrame.MVVM.ViewBase view) {
    }
    
    public override void ScreensRemoved(uFrame.MVVM.ViewBase view) {
    }

}
