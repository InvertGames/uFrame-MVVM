using System;
using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Graphs;

//public class ServicesTutorial : MVVMPage {
//    public override string Name
//    {
//        get { return "Services Tutorial"; }
//    }

//    public override Type ParentPage
//    {
//        get { return typeof (GettingStartedPage); }
//    }

//    public override void GetContent(IDocumentationBuilder builder)
//    {
//        base.GetContent(builder);
//        builder.Paragraph("This tutorial will guide you through Services. To learn more about services, refer to this page: {0}", "PLEASE IMPLEMENT HYPER LINK OR SMTH");
//        builder.BeginTutorial(this.Name);
//        DoTutorial(builder);
//        DoFinalStep(builder);
//        builder.EndTutorial();
//    }

//    private void DoTutorial(IDocumentationBuilder builder)
//    {
//        var project = DoCreateNewProjectStep(builder, _ =>
//        {
//        });

//        if (project == null) return;

//        var graph = DoGraphStep<MVVMGraph>(builder, _ =>
//        {
//        });

//        if (graph == null) return;


//        var subsystem = DoNamedNodeStep<SubsystemNode>(builder, "ServiceTutorialSystem", null, _ =>
//        {
//        });

//        if (subsystem== null) return;

//        var sceneManager = DoNamedNodeStep<SceneTypeNode>(builder, "ServiceTutorialSceneManager", null, _ =>
//        {
//        });

//        if (sceneManager == null) return;

//        //var connection = DoCreateConnectionStep(builder, subsystem.ExportOutputSlot,sceneManager.SubsystemInputSlot, _ =>
//        //{
//        //});

//       // if (connection == null) return;

//        var serviceNode = DoNamedNodeStep<ServiceNode>(builder, "MyService", subsystem, _ =>
//        {
//        });

//        if (serviceNode == null) return;

//        builder.ShowTutorialStep(SaveAndCompile(serviceNode));

//        var elementNode = DoNamedNodeStep<ElementNode>(builder, "ExampleElement", subsystem, _ =>
//        {
//            _.Paragraph("At this point you have a class called MyService with it's own designer and editable files.");
//            _.Paragraph("Services can subscribe to events, publish events and expose some API using methods.");
//            _.Paragraph("The next part will show how to subscribe to events.");
//        });

//        if (elementNode == null) return;

//        var commandItem = DoNamedItemStep<CommandsChildItem>(builder, "ExampleElementCommand", elementNode, "a command" ,_ =>
//        {
//            _.Paragraph("Create a command for the ElementA ");
//        });

//        if (commandItem == null) return;

//        var viewNode = DoNamedNodeStep<ViewNode>(builder, elementNode.Name.AsView(), elementNode, _ =>
//        {

//        });

//        if (viewNode == null) return;

//        var elementToViewConnection = DoCreateConnectionStep(builder, elementNode, viewNode.ElementInputSlot);

//        if (elementToViewConnection == null) return;

//        var handlerItemForElementCommand = DoNamedItemStep<HandlersReference>(builder,commandItem.Name,serviceNode,"a command handler",
//            _ =>
//            {
//               _.Paragraph("Create handler for ElementACommand"); 
//            });

//        if (handlerItemForElementCommand == null) return;

//        builder.ShowTutorialStep(SaveAndCompile(viewNode));

//        builder.ShowTutorialStep(
//          new TutorialStep(
//              "Implement ExampleElementCommand handler in MyService.cs",
//              () => EnsureCodeInEditableFile(serviceNode, serviceNode.Name + ".cs", "Debug.Log")),
//            _ =>
//            {
//                _.Paragraph("Open MyService.cs file. ");
//                _.Paragraph("Find ExampleElementCommandHandler method. If you do not see it, override the base one. ");
//                _.Paragraph("After the base invocation add \"Debug.Log(\"ExampleElement command was handled!\");\"");
//            }
//          );


//        builder.ShowTutorialStep(CreateSceneCommand(sceneManager),
//            _ =>
//            {
//                _.Paragraph("At this point we have handler implemented. We only need to prepare our scene.");
//            });

//        var viewBase = EnsureComponentInSceneStep<ViewBase>(builder, viewNode, _ =>
//        {
//        });

//        if (viewBase == null) return;

//        //implement command handler step

//    }

//    private void DoFinalStep(IDocumentationBuilder builder)
//    {
//        builder.ShowTutorialStep(new TutorialStep("Congratulations!", () =>
//        {
//            return "Yup Yup Yup";
//        }), _ =>
//        {
//            _.Paragraph("Start you game, select ExampleElementView and click ExampleElementCommand! You should see your Debug.Log message!");
//        });
//    }

//}
