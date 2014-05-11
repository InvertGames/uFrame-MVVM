using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;

public class ElementDrawer : DiagramItemDrawer<ElementDataBase>
{
    //public float Height
    //{
    //    get { return (Data.EnumItems.Sum(p=>p.Position.height)) + (HeaderSize * 4) + (Padding * 2); }
    //}

    public override GUIStyle BackgroundStyle
    {
        get { return UFStyles.DiagramBox3; }
    }

    public override GUIStyle ItemStyle
    {
        get { return UFStyles.Item4; }
    }

    public ElementDrawer(ElementDataBase data, ElementsDiagram diagram)
        : base(data, diagram)
    {
    }

    public DiagramItemHeader ComponentsHeader
    {
        get { return _componentsHeader ?? (_componentsHeader = new DiagramItemHeader() { HeaderType = typeof(ViewComponentData), Label = "Components" }); }
        set { _componentsHeader = value; }
    }

    public DiagramItemHeader ViewsHeader
    {
        get { return _viewsHeader ?? (_viewsHeader = new DiagramItemHeader() { HeaderType = typeof(ViewData), Label = "Views" }); }
        set { _viewsHeader = value; }
    }

    public DiagramItemHeader PropertiesHeader
    {
        get { return _propertiesHeader; }
        set { _propertiesHeader = value; }
    }

    public DiagramItemHeader CollectionsHeader
    {
        get { return _collectionsHeader; }
        set { _collectionsHeader = value; }
    }

    public DiagramItemHeader CommandsHeader
    {
        get { return _commandsHeader; }
        set { _commandsHeader = value; }
    }

    public DiagramItemHeader BehavioursHeader
    {
        get { return _behavioursHeader; }
        set { _behavioursHeader = value; }
    }

    private DiagramItemHeader _propertiesHeader = new DiagramItemHeader() { Label = "Properties", HeaderType = typeof(ViewModelPropertyData) };
    private DiagramItemHeader _collectionsHeader = new DiagramItemHeader() { Label = "Collections", HeaderType = typeof(ViewModelCollectionData) };
    private DiagramItemHeader _commandsHeader = new DiagramItemHeader() { Label = "Commands", HeaderType = typeof(ViewModelCommandData) };
    private DiagramItemHeader _behavioursHeader = new DiagramItemHeader() { Label = "Behaviours" };
    
    private DiagramItemHeader _componentsHeader;
    private DiagramItemHeader _viewsHeader;


    protected override GUIStyle GetHighlighter()
    {
        if (!Data.IsMultiInstance)
        {
            return UFStyles.BoxHighlighter4;
        }
        return base.GetHighlighter();
    }

    public override float Width
    {
        get
        {

            return Math.Max(EditorStyles.largeLabel.CalcSize(new GUIContent(Data.FullLabel)).x + 50, MaxNameWidth(EditorStyles.label)+MaxTypeWidth(EditorStyles.label));
        }
    }
    public virtual float MaxTypeWidth(GUIStyle style)
    {
     
            var maxLengthItem = Vector2.zero;
            if (AllowCollapsing && !Data.IsCollapsed)
            {
                foreach (var item in Data.ViewModelItems)
                {
                    var newSize = style.CalcSize(new GUIContent(item.RelatedTypeName));

                    if (maxLengthItem.x < newSize.x)
                    {
                        maxLengthItem = newSize;
                    }
                }
            }


            return maxLengthItem.x + 5;
      
    }
    public float MaxNameWidth(GUIStyle style)
    {
     
            var maxLengthItem = Vector2.zero;
            if (AllowCollapsing && !Data.IsCollapsed)
            {
                foreach (var item in Data.ViewModelItems)
                {
                    var newSize = style.CalcSize(new GUIContent(item.Name));

                    if (maxLengthItem.x < newSize.x)
                    {
                        maxLengthItem = newSize;
                    }
                }
            }


            return  maxLengthItem.x + 2;
        
    }
    protected override void DrawSelectedItem(IDiagramSubItem subItem, ElementsDiagram diagram)
    {
        var item = subItem as IViewModelItem;
        if (item == null)
        {
            base.DrawSelectedItem(subItem, diagram);
            return;
        }
        GUILayout.Space(7);
        var rtn = item.RelatedTypeName ?? "[None]";

        if (ElementDataBase.TypeNameAliases.ContainsKey(rtn))
        {
            rtn = ElementDataBase.TypeNameAliases[rtn];
        }
        if (GUILayout.Button(rtn, UFStyles.ClearItemStyle))
        {
            var typesList = item.GetAvailableRelatedTypes(Diagram.Repository);
            ElementItemTypesWindow.InitTypeListWindow("Choose Type", typesList.ToArray(), (selected) =>
            {
                Undo.RecordObject(diagram.Data, "Set Type");
                item.RelatedType = selected.AssemblyQualifiedName;
                diagram.Data.UpdateLinks();
                EditorUtility.SetDirty(diagram.Data);
                EditorWindow.GetWindow<ElementItemTypesWindow>().Close();
            });
        }
        base.DrawSelectedItem(subItem, diagram);
    }

    protected override void DrawItemLabel(IDiagramSubItem item)
    {
        var vmItem = item as IViewModelItem;
        if (vmItem == null)
        {
            base.DrawItemLabel(item);
        }
        else
        {
            GUILayout.BeginArea(item.Position.Scale(Scale));
            GUILayout.BeginHorizontal();
            GUILayout.Space(7);

            var style = new GUIStyle(UFStyles.ClearItemStyle);
           // style.fontSize = Mathf.RoundToInt(style.fontSize * Scale);
            style.fontStyle = FontStyle.Normal;
            style.alignment = TextAnchor.MiddleLeft;
            style.normal.textColor = BackgroundStyle.normal.textColor;
            var rtn = vmItem.RelatedTypeName ?? string.Empty;
            if (ElementDataBase.TypeNameAliases.ContainsKey(rtn))
            {
                rtn = ElementDataBase.TypeNameAliases[rtn];
            }
            GUILayout.Label(rtn, style, GUILayout.Width(MaxTypeWidth(ItemStyle)));
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleLeft;
            GUILayout.Label(vmItem.Name, style, GUILayout.Width(MaxNameWidth(ItemStyle)));
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        //base.DrawItemLabel(item);

    }

    protected override IEnumerable<DiagramSubItemGroup> GetItemGroups()
    {
        var elementData = Data as ElementData;
        if (elementData != null && Diagram.Data.CurrentFilter == Data)
        {
            //yield return new DiagramSubItemGroup()
            //{
            //    Header = ViewsHeader,
            //    Items = elementData.IncludedViews.Cast<IDiagramSubItem>().ToArray()
            //};
            //yield return new DiagramSubItemGroup()
            //{
            //    Header = ComponentsHeader,
            //    Items = elementData.IncludedComponents.Cast<IDiagramSubItem>().ToArray()
            //};
          
     
        }
        yield return new DiagramSubItemGroup()
        {
            Header = PropertiesHeader,
            Items = Data.Properties.Cast<IDiagramSubItem>().ToArray()
        };
        yield return new DiagramSubItemGroup()
        {
            Header = CollectionsHeader,
            Items = Data.Collections.Cast<IDiagramSubItem>().ToArray()
        };
        yield return new DiagramSubItemGroup()
        {
            Header = CommandsHeader,
            Items = Data.Commands.Cast<IDiagramSubItem>().ToArray()
        };
    }
}