using System.IO;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class CreateSceneCommand : EditorCommand<IDiagramNode>, IDiagramNodeCommand
{
    public override string Group
    {
        get { return "Scene Manager"; }
    }

    public override decimal Order
    {
        get { return 1; }
    }

    public override string Name
    {
        get { return "Create Scene"; }
    }

    public override void Perform(IDiagramNode node)
    {
        var paths = EditorWindow.GetWindow<ElementsDesigner>().Diagram.Data.Settings.CodePathStrategy;
        var sceneManagerData = node as SceneManagerData;

        if (!Directory.Exists(paths.ScenesPath))
        {
            Directory.CreateDirectory(paths.ScenesPath);
        }
        EditorApplication.NewScene();
        var go = new GameObject("_GameManager");
        go.AddComponent<GameManager>()._LoadingLevel = "Loading";
        EditorUtility.SetDirty(go);
        EnsureSceneContainerInScene(sceneManagerData);
        EditorApplication.SaveScene();
        AssetDatabase.Refresh();
    }

  
    private static void EnsureSceneContainerInScene(SceneManagerData sceneManagerData)
    {
        if (sceneManagerData.CurrentType != null)
        {
            if (Object.FindObjectsOfType(sceneManagerData.CurrentType).Length < 1)
            {
                var go = new GameObject("_SceneManager", sceneManagerData.CurrentType);
                go.name = "_SceneManager";
            }
        }
    }

    public override string CanPerform(IDiagramNode node)
    {
        if (node is SceneManagerData) return null;
        return "Must be a scene manager to perform this action.";
    }
}