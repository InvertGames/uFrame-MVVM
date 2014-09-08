using System;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using UnityEditor;

public class AddManagerToSceneSelectionCommand : EditorCommand<IDiagramNode>, IDiagramNodeCommand
{
    public override string Group
    {
        get { return "Scene Manager"; }
    }

    public override string Name
    {
        get { return "Create Scene"; }
    }

    public override string Path
    {
        get { return "Add To/Selection"; }
    }

    public override void Perform(IDiagramNode node)
    {
        //var paths = EditorWindow.GetWindow<ElementsDesigner>().Diagram.Data.Settings.CodePathStrategy;
        var sceneManagerData = node as SceneManagerData;
        if (sceneManagerData == null)
            return;
        var sceneManagerAssemblyName = uFrameEditor.UFrameTypes.ViewModel.AssemblyQualifiedName.Replace("ViewModel",
            sceneManagerData.NameAsSceneManager);
        var type = Type.GetType(sceneManagerAssemblyName);
        if (type == null)
        {
            EditorUtility.DisplayDialog("Can't add to scene", "The diagram must be saved and have no compiler errors.",
                "OK");
            return;
        }

        
        Selection.activeGameObject.AddComponent(type);
    }

    public override string CanPerform(IDiagramNode node)
    {
        if (Selection.activeGameObject == null) return "Make a selection first";
        if (node is SceneManagerData) return null;
        
        return "Must be a scene manager to perform this action.";
    }
}