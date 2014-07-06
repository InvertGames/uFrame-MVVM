using HutongGames.PlayMakerEditor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;

public class AddVariableToFsm : EditorCommand<ViewModelPropertyData>, IDiagramNodeItemCommand
{
    public override string Group
    {
        get { return "Playmaker"; }
    }

    public override string Name
    {
        get { return "Add Variable to FSM"; }
    }
    public override void Perform(ViewModelPropertyData node)
    {
        node.AddVariableToFsm();
    }

    public override string CanPerform(ViewModelPropertyData node)
    {
        if (FsmEditor.SelectedFsm == null) return "No FSM selected.";
        return null;
    }
}