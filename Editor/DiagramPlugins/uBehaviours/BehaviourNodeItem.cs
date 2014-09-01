using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Invert.uFrame.Editor;
using JSONData = Invert.uFrame.Editor.JSONData;
using JSONNode = Invert.uFrame.Editor.JSONNode;

public class BehaviourNodeItem : IDiagramNodeItem
{
    private UBSharedBehaviour _Behaviour;
    private bool _isSelected;
    public Rect Position { get; set; }

    public string Label
    {
        get { return Name; }
    }

    public void CreateLink(IDiagramNode container, IGraphItem target)
    {

    }

    public bool CanCreateLink(IGraphItem target)
    {
        return false;
    }

    public IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] nodes)
    {
        yield break;
    }

    public bool IsSelected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
            if (value == false)
            {
                if (!string.IsNullOrEmpty(_renameTo))
                {
                    AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(Behaviour), Name);
                    UBAssetManager.RefreshBehaviours();
                    _renameTo = null;
                }
            }
            else
            {

                Selection.activeObject = Behaviour;
            }

        }
    }

    public void RemoveLink(IDiagramNode target)
    {

    }

    public Vector2[] ConnectionPoints { get; set; }

    public void Serialize(Invert.uFrame.Editor.JSONClass cls)
    {
       
    }

    public void Deserialize(Invert.uFrame.Editor.JSONClass cls)
    {
        
    }

    public string Name
    {
        get
        {
            if (Behaviour == null) return "??";
            return Behaviour.name;
        }
        set
        {
            Behaviour.name = value;
            EditorUtility.SetDirty(Behaviour);
        }
    }

    public string Highlighter { get; private set; }
    public string FullLabel { get { return Name; } }
    public string Identifier { get; set; }
    public bool IsSelectable { get { return true; } }
    public DiagramNode Node { get; set; }

    public DataBag DataBag
    {
        get { return _dataBag; }
        set { _dataBag = value; }
    }

    public bool IsEditing { get; set; }

    public UBSharedBehaviour Behaviour
    {
        get { return _Behaviour; }
        set { _Behaviour = value; }
    }

    public void Remove(IDiagramNode diagramNode)
    {
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(Behaviour));
    }

    private string _renameTo = null;
    private DataBag _dataBag = new DataBag();

    [DiagramContextMenu("Add to/Selection", 1)]
    public void AddToSelection()
    {
        var selection = Selection.activeObject as GameObject;

        if (selection != null)
        {
            selection.AddComponent<UBComponent>().Behaviour = Behaviour;    
        }
        
        

    }
    public void Rename(IDiagramNode data, string name)
    {
        _renameTo = name;
        Behaviour.name = name;
    }

 
}