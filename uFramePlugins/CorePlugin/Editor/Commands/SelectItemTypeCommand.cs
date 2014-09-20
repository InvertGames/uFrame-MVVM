using System;
using System.Collections.Generic;
using System.Linq;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using Invert.uFrame.Editor.ViewModels;
using UnityEditor;
using UnityEngine;

public class SelectItemTypeCommand : EditorCommand<DiagramViewModel>
{
    public bool AllowNone { get; set; }
    public bool PrimitiveOnly { get; set; }
    public bool IncludeUnityEngine { get; set; }
    public override void Perform(DiagramViewModel node)
    {
        var typesList = GetRelatedTypes(node);

        var viewModelItem = node.SelectedNodeItem as TypedItemViewModel;
        ITypeDiagramItem viewModelItemData;
        if (viewModelItem == null)
        {

            viewModelItemData = node.SelectedNode.DataObject as ITypeDiagramItem;
            if (viewModelItemData == null)
                return;
        }
        else
        {
            viewModelItemData = viewModelItem.Data;
        }
        ElementItemTypesWindow.InitTypeListWindow("Choose Type", typesList.ToArray(), (selected) =>
        {
            uFrameEditor.ExecuteCommand((diagram) =>
            {
                viewModelItemData.RelatedType = selected.AssemblyQualifiedName;
            });
            EditorWindow.GetWindow<ElementItemTypesWindow>().Close();
        });
    }

    public virtual IEnumerable<ElementItemType> GetRelatedTypes(DiagramViewModel diagramData)
    {
        if (AllowNone)
        {
            yield return new ElementItemType() { AssemblyQualifiedName = null, Group = "", Label = "[ None ]" };
        }

        if (!PrimitiveOnly)
        {
            foreach (var viewModel in diagramData.Data.GetElements())
            {
                yield return new ElementItemType()
                {
                    AssemblyQualifiedName = viewModel.Identifier,
                    Label = viewModel.Name,
                    Group = diagramData.Title
                };
            }
        }
        yield return new ElementItemType() { Type = typeof(int), Group = "", Label = "int" };
        yield return new ElementItemType() { Type = typeof(string), Group = "", Label = "string" };
        yield return new ElementItemType() { Type = typeof(decimal), Group = "", Label = "decimal" };
        yield return new ElementItemType() { Type = typeof(float), Group = "", Label = "float" };
        yield return new ElementItemType() { Type = typeof(bool), Group = "", Label = "bool" };
        yield return new ElementItemType() { Type = typeof(char), Group = "", Label = "char" };
        yield return new ElementItemType() { Type = typeof(DateTime), Group = "", Label = "date" };
        yield return new ElementItemType() { Type = typeof(Vector2), Group = "", Label = "Vector2" };
        yield return new ElementItemType() { Type = typeof(Vector3), Group = "", Label = "Vector3" };

        if (PrimitiveOnly) yield break;
        //if (IncludeUnityEngine)
        //{
        //    yield return new ElementItemType() { Type = typeof(UnityEngine.MonoBehaviour), Group = "UnityEngine", Label = "MonoBehaviour" };
        //    yield return new ElementItemType() { Type = typeof(UnityEngine.Component), Group = "UnityEngine", Label = "Component" };


        //}
        var projectAssembly = typeof(ViewModel).Assembly;
        foreach (var type in projectAssembly.GetTypes())
        {
            if (IncludeUnityEngine && typeof (UnityEngine.Object).IsAssignableFrom(type))
            {
                yield return new ElementItemType() { Type = type, Group = "Components", Label = type.Name };;
                continue;
            }
            if (!typeof(Component).IsAssignableFrom(type) && type.IsClass && !type.Name.Contains("<") && !typeof(ViewModel).IsAssignableFrom(type) && !typeof(Controller).IsAssignableFrom(type) && !typeof(ViewBase).IsAssignableFrom(type))
            {
                if (!type.ContainsGenericParameters)
                yield return new ElementItemType() { Type = type, Group = "Project", Label = type.Name };
            }
        }
    }

    public override string CanPerform(DiagramViewModel node)
    {
        
        if (node == null) return "No element data.";
        if (node.SelectedNode == null) return "No selection";
        //if (node.SelectedNodeItem as TypedItemViewModel == null) return "Must be an element item";
        return null;
    }
}


