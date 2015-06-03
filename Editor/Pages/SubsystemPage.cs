using Invert.Core.GraphDesigner;
using uFrame.MVVM.Templates;
using UnityEngine;

namespace Invert.uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class SubsystemPage : SubsystemPageBase {
        public override Type ParentPage
        {
            get { return typeof (TheKernel); }
        }

        public override string Name
        {
            get { return "Subsystems"; }
        }

        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
            _.Paragraph("Subsystems allow you to seperate the various pieces of your project into logical and reusable parts.  Subsystems contain any number of Services, Elements, Views, StateMachines ..etc");
 
            _.ContentByPage<SystemLoaders>();
        }
    }

    public class SystemLoaders : uFrameMVVMPage<SubsystemPage>
    {
        public override bool ShowInNavigation
        {
            get { return false; }
        }

        public override void GetContent(IDocumentationBuilder _)
        {
            base.GetContent(_);
            
            _.Paragraph("System loaders are used to setup the uFrame Runtime with any dependencies it might need before the game begins.");
            _.Break();
    

            _.Title2("Custom System Loaders");
            _.Paragraph("In some cases creating a custom system loader can be very useful for different environments.  e.g. Dev Environment, Production Environment..etc");
            _.Paragraph("To create a custom system loader, derive from SystemLoader, override the load method, and add it to the kernel.");
            _.Break();
            _.Break();
            _.Title2("Generated System Loaders From Subsystems");
            _.Paragraph("All subsystem nodes inside a project will generate a 'SystemLoader'. These register an instance of every element " +
                        "controller that lives inside of it, as well as any 'Instances' defined on it.");

            _.Break();

            var graph = new ScaffoldGraph();
            InstancesReference instance;
             graph.BeginNode<SubsystemNode>(Name)
                .AddItem<InstancesReference>("MyInstance", out instance);

             var subsystem = graph.EndNode();
            graph.BeginNode<ElementNode>("Player");
            var playerNode = graph.EndNode();
            graph.PushFilter(subsystem);
            graph.SetItemLocation(playerNode, new Vector2(10f,10f));

            instance.SourceIdentifier = playerNode.Identifier;
            
            _.TemplateExample<SystemLoaderTemplate,SubsystemNode>(subsystem as SubsystemNode,true, "Load");

        }
    }

}
