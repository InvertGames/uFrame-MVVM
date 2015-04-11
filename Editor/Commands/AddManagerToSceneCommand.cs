using System;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Graphs;
using UnityEditor;
using UnityEngine;

public class AddManagerToSceneCommand : EditorCommand<SceneTypeNode>, IDiagramNodeCommand
{
    public override string Group
    {
        get { return "Scene Manager"; }
    }

    public override string Name
    {
        get { return "Create Scene"; }
    }

    public override bool CanProcessMultiple
    {
        get { return false; }
    }

    public override string Path
    {
        get { return "Add To/Scene"; }
    }

    public override void Perform(SceneTypeNode node)
    {
      
        var sceneManagerData = node as SceneTypeNode;
        if (sceneManagerData == null) 
            return;
        var type = InvertApplication.FindType(node.FullName.AsSceneManager());
        if (type == null)
        {
            EditorUtility.DisplayDialog("Can't add to scene", "The diagram must be saved and have no compiler errors.",
                "OK");
            return;
        }

        GameObject obj = new GameObject("_SceneManager");
        obj.AddComponent(type);
    }

    public override string CanPerform(SceneTypeNode node)
    {
        return null;
    }
}