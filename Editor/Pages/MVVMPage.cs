namespace Invert.uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class MVVMPage : MVVMPageBase {
        public override decimal Order
        {
            get { return -3; }
        }

       
        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);

        }
    }
}
