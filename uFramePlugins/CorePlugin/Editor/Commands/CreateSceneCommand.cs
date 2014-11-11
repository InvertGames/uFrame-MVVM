using System.IO;
using System.Linq;
using Invert.Core.GraphDesigner;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using Invert.uFrame.Editor.ViewModels;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class CreateSceneCommand : EditorCommand<DiagramNodeViewModel>, IDiagramNodeCommand
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

    public override void Perform(DiagramNodeViewModel node)
    {
        if (!EditorApplication.SaveCurrentSceneIfUserWantsTo()) return;

        var paths = InvertGraphEditor.CurrentProject.CurrentGraph.CodePathStrategy;
        var sceneManagerData = node as SceneManagerViewModel;

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

        
        if (!File.Exists(System.IO.Path.Combine(paths.ScenesPath, node.Name + ".unity")))
        {
            EditorApplication.SaveScene(System.IO.Path.Combine(paths.ScenesPath, node.Name + ".unity"));
            AssetDatabase.Refresh();
        }
        else
        {
            EditorApplication.SaveScene();
        }
    }


    private static SceneManager EnsureSceneContainerInScene(SceneManagerViewModel sceneManagerData)
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

    public override string CanPerform(DiagramNodeViewModel node)
    {
        if (node is SceneManagerViewModel) return null;
        return "Must be a scene manager to perform this action.";
    }
}