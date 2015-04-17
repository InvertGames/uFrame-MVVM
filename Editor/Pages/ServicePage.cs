using Invert.Core.GraphDesigner;

namespace Invert.uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class ServicePage : ServicePageBase {
        public override Type ParentPage
        {
            get { return typeof (TheKernel); }
        }

        public override string Name
        {
            get { return "Services"; }
        }

        public override decimal Order
        {
            get { return 1; }
        }

        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
            var graph = new ScaffoldGraph();
            var typeRef = graph.BeginNode<TypeReferenceNode>("ViewCreatedEvent").EndNode();
            graph.BeginNode<ServiceNode>("MyService");
            HandlersReference handler;
            graph.AddItem<HandlersReference>("ViewCreatedEvent", out handler);
            handler.SourceIdentifier = typeRef.Identifier;
            var service = graph.EndNode() as ServiceNode;
            _.Title2("Designer File Implementation");
            _.TemplateExample<ServiceTemplate, ServiceNode>(service);
            _.Break();
            _.Title2("Editable File Implementation");
            _.TemplateExample<ServiceTemplate, ServiceNode>(service,false);
        }
    }
}
