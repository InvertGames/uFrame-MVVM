using System;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Graphs;
using UnityEditor;

public class AddManagerToSceneSelectionCommand : EditorCommand<SceneTypeNode>, IDiagramNodeCommand
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

    public override void Perform(SceneTypeNode node)
    {
        //var paths = EditorWindow.GetWindow<ElementsDesigner>().Diagram.Data.Settings.CodePathStrategy;
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


        Selection.activeGameObject.AddComponent(type);
    }

    public override string CanPerform(SceneTypeNode node)
    {
        if (Selection.activeGameObject == null) return "Make a selection first";
        return null;


    }
}