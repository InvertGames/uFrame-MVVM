using HutongGames.PlayMakerEditor;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using UnityEditor;

public class AddAsStateToFsm : EditorCommand<ViewModelPropertyData>, IDiagramNodeItemCommand
{
    private string _name = "Create Fsm";
    public override string Group
    {
        get { return "Playmaker"; }
    }
    public AddAsStateToFsm(string name)
    {
        _name = name;
    }

    public AddAsStateToFsm(string name, bool isAddToCurrent)
    {
        _name = name;
        IsAddToCurrent = isAddToCurrent;
    }

    public override string Name
    {
        get { return _name; }
    }

    public override bool ShowAsDiabled
    {
        get { return true; }
    }

    public bool IsAddToCurrent { get; set; }
    public override void Perform(ViewModelPropertyData node)
    {
        if (IsAddToCurrent)
        {
            if (node.IsEnum(node.Node.Data))
                node.AddEnumPropertyFsm();
            else
            {
                node.AddPropertyFsm();
            }
        }
        else
        {
            if (node.IsEnum(node.Node.Data))
                node.CreateEnumPropertyFsm();
            else
                node.CreatePropertyFsm();
        }
    }

    public override string CanPerform(ViewModelPropertyData node)
    {
        if (node == null) return "Invalid node.";
        if (Selection.activeGameObject == null) return "Select a GameObject first.";
        //var relatedNode = node.RelatedNode();
        
        if (IsAddToCurrent)
        {
          
            if (FsmEditor.SelectedFsm == null)
            {
                return "Select an object with an FSM first.";
            }
        }
        //if (relatedNode != null)
        //{
        var type = uFrameEditor.FindType(node.Node.Name + "ViewModel");
        if (type == null)
        {
            return "Save the diagram first.";
        }
        return null;
        // }
        // return "Property must be an enum type.";
    }
}