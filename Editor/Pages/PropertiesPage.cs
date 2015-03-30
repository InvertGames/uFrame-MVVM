namespace Invert.uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class ElementPropertiesPage : ElementPropertiesPageBase {
        
        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
            
        }
    }
    
    public class SimpleClassPropertiesPage : SimpleClassPropertiesPageBase {
        
        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
        }
    }
    
    public class CommandPropertiesPage : CommandPropertiesPageBase {
        
        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
        }
    }
}
