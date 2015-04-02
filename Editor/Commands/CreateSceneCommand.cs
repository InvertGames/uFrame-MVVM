using System.IO;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class CreateSceneCommand : EditorCommand<SceneManagerNode>, IDiagramNodeCommand
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

    public override void Perform(SceneManagerNode node)
    {
        if (!EditorApplication.SaveCurrentSceneIfUserWantsTo()) return;

        var paths = node.Graph.CodePathStrategy;
        var sceneManagerData = node as SceneManagerNode;

        if (!Directory.Exists(paths.ScenesPath))
        {
            Directory.CreateDirectory(paths.ScenesPath);
        }
        EditorApplication.NewScene();
        var go = new GameObject(string.Format("_{0}Root", node.Name));
        var type = InvertApplication.FindType(node.Name);
        if(type!=null) go.AddComponent(type);
        EditorUtility.SetDirty(go);
        
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

    public override string CanPerform(SceneManagerNode node)
    {

        return null;
    }
}

