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
 * This view serves as a base class for all the SubScreen views
 * It handles screen activation/deactivation.
 * It also handles binding for Close command. You can configure it using the inspector.
 */
public class SubScreenView : SubScreenViewBase
{

    public GameObject ScreenUIContainer;


    protected override void InitializeViewModel(uFrame.MVVM.ViewModel model) {
        base.InitializeViewModel(model);
    }

    public override void Bind() {
        base.Bind();
    }

    public override void IsActiveChanged(Boolean active)
    {

        /* 
         * Always make sure, that you cache components used in the binding handlers BEFORE you actually bind.
         * This is important, since when Binding to the viewmodel, Handlers are invoked immidiately
         * with the current values, to ensure that view state is consistant.
         * For example, you can do this in Awake or in KernelLoading/KernelLoaded.
         * However, in this example we simply use public property to get a reference to ScreenUIContainer.
         * So we do not have to cache anything.
         */
        ScreenUIContainer.gameObject.SetActive(active);
    }



}
