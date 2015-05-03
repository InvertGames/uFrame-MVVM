namespace uFrame.DefaultProject
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UniRx;
    using UnityEngine;


    public class LoadingScreenView : LoadingScreenViewBase
    {

        protected override void InitializeViewModel(ViewModel model)
        {
            base.InitializeViewModel(model);
            // NOTE: this method is only invoked if the 'Initialize ViewModel' is checked in the inspector.
            // var vm = model as LoadingScreenViewModel;
            // This method is invoked when applying the data from the inspector to the viewmodel.  Add any view-specific customizations here.
        }

        public override void Bind()
        {
            base.Bind();
            // Use this.LoadingScreen to access the viewmodel.
            // Use this method to subscribe to the view-model.
            // Any designer bindings are created in the base implementation.
        }

        public override void ActiveChanged(Boolean arg1)
        {
            this.transform.GetChild(0).gameObject.SetActive(arg1);
        }
    }
}
