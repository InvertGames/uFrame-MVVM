using System;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using UnityEditor;
using UnityEngine;

public class AddManagerToSceneCommand : EditorCommand<IDiagramNode>, IDiagramNodeCommand
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
        get { return "Add To/Scene"; }
    }

    public override void Perform(IDiagramNode node)
    {
      
        var sceneManagerData = node as SceneManagerData;
        if (sceneManagerData == null) 
            return;
        var sceneManagerAssemblyName = uFrameEditor.uFrameTypes.ViewModel.AssemblyQualifiedName.Replace("ViewModel",
            sceneManagerData.NameAsSceneManager);
        var type = Type.GetType(sceneManagerAssemblyName);
        if (type == null)
        {
            EditorUtility.DisplayDialog("Can't add to scene", "The diagram must be saved and have no compiler errors.",
                "OK");
            return;
        }

        GameObject obj = new GameObject("_SceneManager");
        obj.AddComponent(type);
    }

    public override string CanPerform(IDiagramNode node)
    {
        if (node is SceneManagerData) return null;
        return "Must be a scene manager to perform this action.";
    }
}