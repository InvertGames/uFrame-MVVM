using System;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Graphs;
using UnityEditor;

public class AddManagerToSceneSelectionCommand : EditorCommand<SceneManagerNode>, IDiagramNodeCommand
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

    public override void Perform(SceneManagerNode node)
    {
        //var paths = EditorWindow.GetWindow<ElementsDesigner>().Diagram.Data.Settings.CodePathStrategy;
        var sceneManagerData = node as SceneManagerNode;
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

    public override string CanPerform(SceneManagerNode node)
    {
        if (Selection.activeGameObject == null) return "Make a selection first";
        return null;


    }
}