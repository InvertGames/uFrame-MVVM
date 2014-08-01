using Invert.uFrame;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner.Commands;

public class PlaymakerPlugin : DiagramPlugin
{
    public override string PackageName
    {
        get { return "PlaymakerFSM"; }
    }

    public override void Initialize(uFrameContainer container)
    {
        // Playmaker enabled element flag
        container.RegisterInstance<IDiagramNodeCommand>(new NodeFlagCommand<ElementData>("Playmaker","Use Playmaker?"), "PlayMakerCommandsFlag");
        container.RegisterInstance<IDiagramNodeCommand>(new AddVariablesToFsm(), "Sync Playmaker Events");
        container.RegisterInstance<IDiagramNodeItemCommand>(new AddVariableToFsm(), "AddVariableToFsm");
        //container.RegisterInstance<IDiagramNodeItemCommand>(new AddAsStateToFsm("Add To Current FSM", true), "AddAsState");
        container.RegisterInstance<IDiagramNodeItemCommand>(new AddAsStateToFsm("Create FSM On Selection", false), "CreateFSMOnSelection");
        container.Register<ViewBindingExtender, PlaymakerFSMBindingSyncExtender>("PlaymakerBindings");
        container.Register<DesignerGeneratorFactory, PlaymakerElementNodeGeneratorFactory>("PlaymakerElementNodeGenerator");
    }
}
public class NoesisGUIPlugin : DiagramPlugin
{
    public override string PackageName
    {
        get { return "PlaymakerFSM"; }
    }

    public override void Initialize(uFrameContainer container)
    {
        // Playmaker enabled element flag
        container.RegisterInstance<IDiagramNodeCommand>(new NodeFlagCommand<ElementData>("Noesis", "Is Noesis Compatible"), "NoesisVMFlag");

    }
}