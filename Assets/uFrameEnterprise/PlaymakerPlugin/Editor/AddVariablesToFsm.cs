using HutongGames.PlayMakerEditor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;

public class AddVariablesToFsm : EditorCommand<ElementData>, IDiagramNodeCommand
{
    public override string Name
    {
        get { return base.Name; }
    }
    public override string Group
    {
        get { return "Playmaker"; }
    }
    public override bool ShowAsDiabled
    {
        get { return true; }
    }

    public override string Path
    {
        get { return "Plamaker/" + "Add All Variables"; }
    }

    public override void Perform(ElementData node)
    {
        //var fsm = Selection.activeGameObject.GetComponent<PlayMakerFSM>();
        foreach (var viewModelPropertyData in node.Properties)
        {
            viewModelPropertyData.AddBindingEventToFsm();
            viewModelPropertyData.AddVariableToFsm();
        }
    }

    public override string CanPerform(ElementData node)
    {
        if (node == null) return "Invalid Node.";
        if (!node["Playmaker"]) return "You must turn on the playmaker flag.";
        if (FsmEditor.SelectedFsm == null) return "No FSM Selected";
        return null;
    }
}