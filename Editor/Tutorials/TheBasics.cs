using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

public class TheBasics : uFrameMVVMPage<GettingStartedPage>
{
    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        _.Paragraph("The purpose of this tutorial is to teach you the high level aspects of using uFrame, this includes SubSystems, and Scenes");
        _.Break();
        _.BeginTutorial("The Basics : Systems, Scenes, and The Kernel");
        var project = DoCreateNewProjectStep(_);
        if (project == null) return;

        var graph = DoGraphStep<MVVMGraph>(_);
        if (graph == null) return;

        

        var systemA = DoNamedNodeStep<SubsystemNode>(_, "SystemA");
        if (systemA == null) return;

        var systemB = DoNamedNodeStep<SubsystemNode>(_, "SystemB");
        if (systemB == null) return;

        var sceneA = DoNamedNodeStep<SceneTypeNode>(_, "SceneA");
        if (sceneA == null) return;

        var sceneB = DoNamedNodeStep<SceneTypeNode>(_, "SceneB");
        if (sceneB == null) return;

        //var connectionA = DoCreateConnectionStep(_, systemA.ExportOutputSlot, sceneA.SubsystemInputSlot);
        //if (connectionA == null) return;

        //var connectionB = DoCreateConnectionStep(_, systemB.ExportOutputSlot, sceneB.SubsystemInputSlot);
        //if (connectionB == null) return;


        _.ShowTutorialStep(SaveAndCompile(sceneA));
        _.EndTutorial();
    }
}