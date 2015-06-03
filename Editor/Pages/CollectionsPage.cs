using uFrame.MVVM.Templates;

namespace Invert.uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class ElementCollectionsPage : ElementCollectionsPageBase {
        
        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
            _.Paragraph("Collections are a bindable list on a view model that allow anything to be notified when a change occurs to the collection.");

            var ele = new ScaffoldGraph()
                .BeginNode<ElementNode>("Player")
                .AddItem<CollectionsChildItem>("Children")
            .EndNode();

            _.Title2("Generated Model Collections");
            _.TemplateExample<ViewModelTemplate, ElementNode>(ele as ElementNode, true, "CollectionProperty", "Bind");

        }
    }
    
    public class SimpleClassCollectionsPage : SimpleClassCollectionsPageBase {
        public override bool ShowInNavigation
        {
            get { return false; }
        }

        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
        }
    }
    
    public class CommandCollectionsPage : CommandCollectionsPageBase {
        public override bool ShowInNavigation
        {
            get { return false; }
        }

        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
        }
    }
}
