namespace Invert.uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class StateMachineStartStatePage : StateMachineStartStatePageBase {
        public override bool ShowInNavigation
        {
            get { return false; }
        }

        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
        }
    }
}
