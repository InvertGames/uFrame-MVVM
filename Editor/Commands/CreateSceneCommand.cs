using System.IO;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class CreateSceneCommand : EditorCommand<SceneTypeNode>, IDiagramNodeCommand
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

    public override void Perform(SceneTypeNode node)
    {
        if (!EditorApplication.SaveCurrentSceneIfUserWantsTo()) return;

        var paths = node.Graph.CodePathStrategy;

        if (!Directory.Exists(paths.ScenesPath))
        {
            Directory.CreateDirectory(paths.ScenesPath);
        }
        EditorApplication.NewScene();
        var go = new GameObject(string.Format("_{0}Root", node.Name));
        var type = InvertApplication.FindType(node.Name);
        if (type != null) go.AddComponent(type);
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

    public override string CanPerform(SceneTypeNode node)
    {

        return null;
    }
}

public class ScaffoldOrUpdateKernelCommand : ToolbarCommand<DiagramViewModel>
{
    public override ToolbarPosition Position
    {
        get
        {
            return ToolbarPosition.Right;
        }
    }

    public override string Name
    {
        get { return "Scaffold/Update Kernel"; }
    }

    public override void Perform(DiagramViewModel node)
    {
        if (!EditorApplication.SaveCurrentSceneIfUserWantsTo()) return;
        if (!EditorUtility.DisplayDialog("ACHTUNG!", "Before scaffolding the core, make sure you saved and compiled!",
            "Yes, I saved and compiled!", "Cancel")) return;

        var paths = node.GraphData.CodePathStrategy;

        var sceneName = node.GraphData.Project.Name + "KernelScene.unity";
        var sceneNameWithPath = System.IO.Path.Combine(paths.ScenesPath, sceneName);

        var prefabName = node.GraphData.Project.Name + "Kernel.prefab";
        var project = node.GraphData.Project as UnityEngine.Object;
        var path = AssetDatabase.GetAssetPath(project);

        var prefabNameWithPath =  path.Replace(project.name + ".asset", prefabName);


        uFrameMVVMKernel uFrameMVVMKernel = null;

        if (File.Exists(sceneNameWithPath))
        {
            //EditorApplication.OpenScene(sceneNameWithPath);
            var gameObject = (GameObject)AssetDatabase.LoadAssetAtPath(prefabNameWithPath, typeof(GameObject));
            uFrameMVVMKernel = gameObject.GetComponent<uFrameMVVMKernel>();
            SyncKernel(node, uFrameMVVMKernel);

        }
        else
        {        
            EditorApplication.NewEmptyScene();
            if (!Directory.Exists(paths.ScenesPath))
            {
                Directory.CreateDirectory(paths.ScenesPath);
            }
            uFrameMVVMKernel = FindComponentInScene<uFrameMVVMKernel>() ??
                                new GameObject("Kernel").AddComponent<uFrameMVVMKernel>();
            SyncKernel(node, uFrameMVVMKernel);
        //    var pref ab : Object = PrefabUtility.CreateEmptyPrefab(localPath);
        //PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
            PrefabUtility.CreatePrefab(prefabNameWithPath, uFrameMVVMKernel.gameObject, ReplacePrefabOptions.ConnectToPrefab);
            EditorApplication.SaveScene(sceneNameWithPath);
        }

        AssetDatabase.Refresh();
    }

    private static void SyncKernel(DiagramViewModel node, uFrameMVVMKernel uFrameMVVMKernel)
    {
        var servicesContainer = uFrameMVVMKernel.transform.FindChild("Services");
        if (servicesContainer == null)
        {
            servicesContainer = new GameObject("Services").transform;
            servicesContainer.SetParent(uFrameMVVMKernel.transform);
        }

        var systemLoadersContainer = uFrameMVVMKernel.transform.FindChild("SystemLoaders");
        if (systemLoadersContainer == null)
        {
            systemLoadersContainer = new GameObject("SystemLoaders").transform;
            systemLoadersContainer.SetParent(uFrameMVVMKernel.transform);
        }

        var sceneLoaderContainer = uFrameMVVMKernel.transform.FindChild("SceneLoaders");
        if (sceneLoaderContainer == null)
        {
            sceneLoaderContainer = new GameObject("SceneLoaders").transform;
            sceneLoaderContainer.SetParent(uFrameMVVMKernel.transform);
        }

        var servicesNodes = node.CurrentRepository.NodeItems.OfType<ServiceNode>();
        foreach (var serviceNode in servicesNodes)
        {
            var type = InvertApplication.FindType(serviceNode.Name);
            if (type != null && uFrameMVVMKernel.GetComponentInChildren(type) == null)
            {
                servicesContainer.gameObject.AddComponent(type);
            }
        }

        var systemNodes = node.CurrentRepository.NodeItems.OfType<SubsystemNode>();
        foreach (var systemNode in systemNodes)
        {
            if (!systemNode.GetContainingNodes(systemNode.Graph.Project).OfType<ElementNode>().Any()) continue;
            var type = InvertApplication.FindType(string.Format("{0}Loader", systemNode.Name));
            if (type != null && uFrameMVVMKernel.GetComponentInChildren(type) == null)
            {
                systemLoadersContainer.gameObject.AddComponent(type);
            }
        }

        var sceneNodes = node.CurrentRepository.NodeItems.OfType<SceneTypeNode>();
        foreach (var sceneNode in sceneNodes)
        {
            var type = InvertApplication.FindType(string.Format("{0}Loader", sceneNode.Name));
            if (type != null && uFrameMVVMKernel.GetComponentInChildren(type) == null)
            {
                sceneLoaderContainer.gameObject.AddComponent(type);
            }
        }


        EditorUtility.SetDirty(uFrameMVVMKernel);
    }


    private T FindComponentInScene<T>() where T : MonoBehaviour
    {
        object[] obj = GameObject.FindSceneObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            var c = (T)g.GetComponent(typeof(T));
            if (c != null) return c;
        }
        return null;
    }


    public override string CanPerform(DiagramViewModel node)
    {
        return (string)null;
    }
}