using System;
using System.Collections.Generic;
using System.Linq;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ViewModels;
using UnityEditor;
using UnityEngine;

public class SelectItemTypeCommand : EditorCommand<ElementsDiagram>
{
    public bool AllowNone { get; set; }
    public bool PrimitiveOnly { get; set; }

    public override void Perform(ElementsDiagram node)
    {
        var typesList = GetRelatedTypes(node.Data);

        var viewModelItem = node.SelectedItem.NodeItem as IViewModelItem;
        if (viewModelItem == null)
        {
            return;
        }
        ElementItemTypesWindow.InitTypeListWindow("Choose Type", typesList.ToArray(), (selected) =>
        {
            node.ExecuteCommand((diagram) =>
            {
                viewModelItem.RelatedType = selected.AssemblyQualifiedName;
            });
            EditorWindow.GetWindow<ElementItemTypesWindow>().Close();
        });
    }

    public virtual IEnumerable<ElementItemType> GetRelatedTypes(IElementDesignerData diagramData)
    {
        if (AllowNone)
        {
            yield return new ElementItemType() { AssemblyQualifiedName = null, Group = "", Label = "[ None ]" };
        }

        if (!PrimitiveOnly)
        {
            foreach (var viewModel in diagramData.GetElements())
            {
                yield return new ElementItemType()
                {
                    AssemblyQualifiedName = viewModel.AssemblyQualifiedName,
                    Label = viewModel.Name,
                    Group = diagramData.Name
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
        yield return new ElementItemType() { Type = typeof(FPSPlayerController), Group = "", Label = "FPSPlayerController" };

        if (PrimitiveOnly) yield break;

        var projectAssembly = typeof(ViewModel).Assembly;
        foreach (var type in projectAssembly.GetTypes())
        {
            if (typeof(ViewModel).IsAssignableFrom(type) || typeof(ICommand).IsAssignableFrom(type))
            {
                yield return new ElementItemType() { Type = type, Group = "Project", Label = type.Name };
            }
        }
    }

    public override string CanPerform(ElementsDiagram node)
    {
        
        if (node == null) return "No element data.";
        if (node.SelectedItem == null) return "No selection";
        if (node.SelectedItem.NodeItem as IViewModelItem == null) return "Must be an element item";
        return null;
    }
}