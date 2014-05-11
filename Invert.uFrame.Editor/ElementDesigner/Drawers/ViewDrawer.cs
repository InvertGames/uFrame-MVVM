using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewDrawer : DiagramItemDrawer<ViewData>
{
    private ElementDataBase _forElement;

    public override GUIStyle BackgroundStyle
    {
        get
        {

            return UFStyles.DiagramBox2;
        }
    }
    public ViewDrawer(ViewData data, ElementsDiagram diagram)
        : base(data, diagram)
    {
        Diagram = diagram;

    }

    public override bool AllowCollapsing
    {
        get { return true; }
    }


    private DiagramItemHeader _behavioursHeader;

    protected override IEnumerable<DiagramSubItemGroup> GetItemGroups()
    {

        //var vm = Data.ViewForElement;
        //UFrameBehaviours[] items = new UFrameBehaviours[] { };
        //if (vm != null)
        //{
        //    items =  UBAssetManager.Behaviours.OfType<UFrameBehaviours>()
        //        .Where(p => p != null && p.ViewModelTypeString == vm.ViewModelAssemblyQualifiedName).ToArray();    
        //}
        //yield return new DiagramSubItemGroup()
        //{
        //    Header = BehavioursHeader,
        //    Items = Data.Items.ToArray()
        //};
        yield break;
    }

    public override GUIStyle ItemStyle
    {
        get { return UFStyles.Item4; }
    }

    //public DiagramItemHeader BehavioursHeader
    //{
    //    get
    //    {
    //        if (_behavioursHeader != null)
    //            return _behavioursHeader;


    //        _behavioursHeader =
    //            new DiagramItemHeader() { HeaderType = typeof(UBSharedBehaviour), Label = "Behaviours" };
    //        _behavioursHeader.OnAddItem += BehavioursHeaderOnOnAddItem;
    //        return _behavioursHeader;
    //    }
    //    set { _behavioursHeader = value; }
    //}

    //private void BehavioursHeaderOnOnAddItem()
    //{
    //    Diagram.Repository.CreateUBehaviour(Data);
    //    Diagram.Refresh();
    //    //    var asset = UBAssetManager.CreateAsset<UFrameBehaviours>(Diagram.Repository.ViewsPath, Data.Name + "Behaviour");
    //    //    asset.ViewModelTypeString = Data.ViewForElement.ViewModelAssemblyQualifiedName;
    //    //   
    //}

    //protected override void DrawItem(IDiagramSubItem item, ElementsDiagram diagram, bool importOnly)
    //{
    //    base.DrawItem(item, diagram, importOnly);
    //    var valueItem = item as BehaviourSubItem;
    //    GUILayout.BeginArea(item.Position);
    //    //GUILayout.BeginHorizontal();

    //    //GUILayout.Label(item.Name,ItemStyle,GUILayout.Width(Data.Position.width / 2f));

    //    var type = valueItem.Property.Type;
    //    if (type == typeof (string))
    //    {
    //        valueItem.StringValue = EditorGUILayout.TextField(valueItem.Name, valueItem.StringValue);
    //    }
    //    else
    //    {
    //        EditorGUILayout.LabelField(valueItem.Name, "Not implemented", ItemStyle);
    //    }

    //    //GUILayout.EndHorizontal(); 
    //    GUILayout.EndArea();
    //}

    protected override void DrawSelectedItem(IDiagramSubItem item, ElementsDiagram diagram)
    {
        base.DrawSelectedItem(item, diagram);
    }
}