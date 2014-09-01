using System.IO;
using System.Linq;
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
        var sceneManager = EnsureSceneContainerInScene(sceneManagerData);
        if (sceneManager != null)
        go.GetComponent<GameManager>()._Start = sceneManager;
        EditorApplication.SaveScene();
        AssetDatabase.Refresh();
    }


    private static SceneManager EnsureSceneContainerInScene(SceneManagerData sceneManagerData)
    {
        if (sceneManagerData.CurrentType != null)
        {
            var objs = Object.FindObjectsOfType(sceneManagerData.CurrentType);
            if (objs.Length < 1)
            {
                var go = new GameObject("_SceneManager", sceneManagerData.CurrentType)
                {
                    name = "_SceneManager"
                };
                return go.GetComponent<SceneManager>();
            }
            else
            {
                return objs.FirstOrDefault() as SceneManager;
            }
        }
        return null;
    }

    public override string CanPerform(IDiagramNode node)
    {
        if (node is SceneManagerData) return null;
        return "Must be a scene manager to perform this action.";
    }
}