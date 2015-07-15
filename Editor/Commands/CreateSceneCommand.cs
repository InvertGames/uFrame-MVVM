using System.IO;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Kernel;
using uFrame.MVVM.Services;
using UnityEditor;
using UnityEngine;

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

        var paths = node.Project.SystemDirectory;
        var scenesPath = System.IO.Path.Combine(paths, "Scenes");
        var relativeScenesPath = System.IO.Path.Combine(node.Graph.AssetDirectory, "Scenes");

        if (!Directory.Exists(scenesPath))
        {
            Directory.CreateDirectory(scenesPath);
        }
        EditorApplication.NewScene();
        var go = new GameObject(string.Format("_{0}Root", node.Name));
        var type = InvertApplication.FindType(node.FullName);
        if (type != null) go.AddComponent(type);
        EditorUtility.SetDirty(go);
        var scenePath = System.IO.Path.Combine(scenesPath, node.Name + ".unity");
        var relativeScenePath = System.IO.Path.Combine(relativeScenesPath, node.Name + ".unity");
        if (!File.Exists(scenePath))
        {
            EditorApplication.SaveScene(System.IO.Path.Combine(relativeScenesPath, node.Name + ".unity"), false);
            AssetDatabase.Refresh();
        }
        else
        {
            EditorApplication.SaveScene();
            AssetDatabase.Refresh();
        }
        if (!UnityEditor.EditorBuildSettings.scenes.Any(s =>
        {
            return s.path.EndsWith(node.Name + ".unity");
        }))
        {
            var list = EditorBuildSettings.scenes.ToList();
            list.Add(new EditorBuildSettingsScene(relativeScenePath, true));
            EditorBuildSettings.scenes = list.ToArray();
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
        if (!EditorUtility.DisplayDialog("Warning!", "Before scaffolding the core, make sure you saved and compiled!",
            "Yes, I saved and compiled!", "Cancel")) return;

        var paths = node.GraphData.Project.SystemDirectory;
        var scenesPath = System.IO.Path.Combine(paths, "Scenes");

        var sceneName = node.GraphData.Project.Name + "KernelScene.unity";
        var sceneNameWithPath = System.IO.Path.Combine(scenesPath, sceneName);

        var prefabName = node.GraphData.Project.Name + "Kernel.prefab";
        var project = node.GraphData.Project as Object;
        var path = AssetDatabase.GetAssetPath(project);

        var prefabNameWithPath = path.Replace(project.name + ".asset", prefabName);

        var relativeScenesPath = System.IO.Path.Combine(node.GraphData.AssetDirectory, "Scenes");
        var relativeScenePath = System.IO.Path.Combine(relativeScenesPath, sceneName);


        uFrameKernel uFrameMVVMKernel = null;

        if (File.Exists(sceneNameWithPath))
        {
            //EditorApplication.OpenScene(sceneNameWithPath);
            var gameObject = (GameObject)AssetDatabase.LoadAssetAtPath(prefabNameWithPath, typeof(GameObject));
            uFrameMVVMKernel = gameObject.GetComponent<uFrameKernel>();
            SyncKernel(node, uFrameMVVMKernel.gameObject);

        }
        else
        {
#if UNITY_5
            EditorApplication.NewEmptyScene();
#else
            EditorApplication.NewScene();
#endif
            if (!Directory.Exists(scenesPath))
            {
                Directory.CreateDirectory(scenesPath);
            }
            uFrameMVVMKernel = FindComponentInScene<uFrameKernel>() ??
                                new GameObject("Kernel").AddComponent<uFrameKernel>();
            var services = SyncKernel(node, uFrameMVVMKernel.gameObject);

            services.gameObject.AddComponent<ViewService>();
            services.gameObject.AddComponent<SceneManagementService>();

            //    var pref ab : Object = PrefabUtility.CreateEmptyPrefab(localPath);
            //PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
            PrefabUtility.CreatePrefab(prefabNameWithPath, uFrameMVVMKernel.gameObject, ReplacePrefabOptions.ConnectToPrefab);
            EditorApplication.SaveScene(relativeScenePath);
        }
        if (!UnityEditor.EditorBuildSettings.scenes.Any(s =>
        {
            return s.path.EndsWith(node.GraphData.Project.Name + "KernelScene.unity");
        }))
        {
            var list = EditorBuildSettings.scenes.ToList();
            list.Add(new EditorBuildSettingsScene(relativeScenePath, true));
            EditorBuildSettings.scenes = list.ToArray();
        }
        AssetDatabase.Refresh();
    }

    private static Transform SyncKernel(DiagramViewModel node, GameObject uFrameMVVMKernel)
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
            var type = InvertApplication.FindType(serviceNode.FullName);
            if (type != null && servicesContainer.GetComponent(type) == null)
            {
                servicesContainer.gameObject.AddComponent(type);
            }
        }

        var systemNodes = node.GraphData.Project.AllGraphItems.OfType<SubsystemNode>();
        foreach (var systemNode in systemNodes)
        {
            //if (!systemNode.GetContainingNodes(systemNode.Graph.Project).OfType<ElementNode>().Any()) continue;
            var type = InvertApplication.FindType(string.Format("{0}Loader", systemNode.FullName));
            if (type != null && systemLoadersContainer.GetComponent(type) == null)
            {
                systemLoadersContainer.gameObject.AddComponent(type);
            }
        }

        var sceneNodes = node.CurrentRepository.NodeItems.OfType<SceneTypeNode>();
        foreach (var sceneNode in sceneNodes)
        {
            var type = InvertApplication.FindType(string.Format("{0}Loader", sceneNode.FullName));
            if (type != null && sceneLoaderContainer.GetComponent(type) == null)
            {
                sceneLoaderContainer.gameObject.AddComponent(type);
            }
        }


        EditorUtility.SetDirty(uFrameMVVMKernel);
        return servicesContainer;
    }


    private T FindComponentInScene<T>() where T : MonoBehaviour
    {
        object[] obj = Object.FindObjectsOfType(typeof(GameObject));
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