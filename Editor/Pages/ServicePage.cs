using Invert.Core.GraphDesigner;
using uFrame.MVVM.Templates;

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

            _.Paragraph("While services can serve for almost any purpose, they can be used to seperate various features of uFrame, and your application. " +
                        "Examples might include, FacebookService, NetworkingService, AchievementsService...etc");
            _.Break();
            _.Paragraph("As a matter of fact, at the time of this writing, uFrame ships with two default services, The 'ViewService', and the 'SceneManagementService'.");
            
            _.Break();
            

            _.Paragraph("There is really only one general rule of thumb when implementing services, " +
                        "they should only be listening to events, processing them, and publishing its own " +
                        "events that might be useful to other services.  While you can inject other services " +
                        "and use them directly, it's highly reommended to use events as the means of communication.");
            _.Break();

            _.Title2("Accesing ViewModels in services.");
            _.Paragraph("To access a running list of a specific viewmodel just add this property to any service, and make sure you specify the viewmodel type you need.");
            _.CodeSnippet("[Inject] IViewModelManager<PlayerViewModel> AllPlayers { get;set; }");
            _.LinkToPage<ViewModelManagers>();

            var service = ServiceNode();
            _.Title2("Designer File Implementation");
            _.TemplateExample<ServiceTemplate, ServiceNode>(service);
            _.Break();
            _.Title2("Editable File Implementation");
            _.TemplateExample<ServiceTemplate, ServiceNode>(service,false);
        }

        private static ServiceNode ServiceNode()
        {
            var graph = new ScaffoldGraph();
            var typeRef = graph.BeginNode<TypeReferenceNode>("ViewCreatedEvent").EndNode();
            graph.BeginNode<ServiceNode>("MyService");
            HandlersReference handler;
            graph.AddItem<HandlersReference>("ViewCreatedEvent", out handler);
            handler.SourceIdentifier = typeRef.Identifier;
            var service = graph.EndNode() as ServiceNode;
            return service;
        }
    }
}
