using uFrame.MVVM.Templates;

namespace Invert.uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class ElementPropertiesPage : ElementPropertiesPageBase {
        
        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
            var ele = new ScaffoldGraph()
            .BeginNode<ElementNode>("Player")
            .AddItem<PropertiesChildItem>("FirstName")
            .AddItem<PropertiesChildItem>("LastName")
            .EndNode();

            _.Title2("Subscribable Properties");
            _.TemplateExample<ViewModelTemplate, ElementNode>(ele as ElementNode, true, "ViewModelProperty");

            _.Title2("Value Wrapper Properties");
            _.TemplateExample<ViewModelTemplate, ElementNode>(ele as ElementNode, true, "ViewModelValueProperty");

            _.Title2("The Bind Method");
            _.TemplateExample<ViewModelTemplate, ElementNode>(ele as ElementNode, true, "Bind");
        }
    }
    
    public class SimpleClassPropertiesPage : SimpleClassPropertiesPageBase {
        public override bool ShowInNavigation
        {
            get { return false; }
        }
        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
        }
    }
    
    public class CommandPropertiesPage : CommandPropertiesPageBase {
        public override bool ShowInNavigation
        {
            get { return false; }
        }

        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
        }
    }
}
