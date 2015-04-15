namespace Invert.uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class ViewComponentPage : ViewComponentPageBase {
        public override Type ParentPage
        {
            get { return typeof (ViewPage); }
        }

        public override decimal Order
        {
            get { return 1; }
        }

        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
        }
    }
}
