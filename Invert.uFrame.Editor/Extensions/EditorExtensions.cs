using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class EditorExtensions
{
    public static string SelectedAssetPath
    {
        get
        {
            string path = "Assets";

            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);

                if (File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }

                break;
            }
            return path;
        }
    }

    public static string SelectedPath
    {
        get
        {
            return Path.Combine(Application.dataPath.Replace("Assets", ""), SelectedAssetPath).Replace("\\", "//").Replace("//", "/");
        }
    }

    public static Component ComponentField(string label, Component value, Type componentType = null)
    {
        componentType = componentType ?? typeof(MonoBehaviour);

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(label, "", GUILayout.Width(EditorGUIUtility.labelWidth - 10));

            GUILayout.Space(5);

            var displayText = value == null ? "[none]" : value.ToString();
            GUILayout.Label(displayText, "TextField", GUILayout.ExpandWidth(true), GUILayout.MinWidth(100));

            var evt = Event.current;
            if (evt != null)
            {
                var textRect = GUILayoutUtility.GetLastRect();
                if (evt.type == EventType.mouseDown && evt.clickCount == 2)
                {
                    if (textRect.Contains(evt.mousePosition))
                    {
                        if (GUI.enabled && value != null)
                        {
                            Selection.activeObject = value;
                            EditorGUIUtility.PingObject(value);
                            GUIUtility.hotControl = value.GetInstanceID();
                        }
                    }
                }
                else if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
                {
                    if (textRect.Contains(evt.mousePosition))
                    {
                        var reference = DragAndDrop.objectReferences.First();
                        var draggedComponent = (Component)null;
                        if (reference is Transform)
                        {
                            draggedComponent = (Transform)reference;
                        }
                        else if (reference is GameObject)
                        {
                            draggedComponent =
                                ((GameObject)reference)
                                    .GetComponents(componentType)
                                    .FirstOrDefault();
                        }
                        else if (reference is Component)
                        {
                            draggedComponent = reference as Component;
                            if (draggedComponent == null)
                            {
                                draggedComponent =
                                    ((Component)reference)
                                        .GetComponents(componentType)
                                        .FirstOrDefault();
                            }
                        }

                        DragAndDrop.visualMode = (draggedComponent == null) ? DragAndDropVisualMode.None : DragAndDropVisualMode.Copy;

                        if (evt.type == EventType.DragPerform)
                        {
                            value = draggedComponent;
                        }

                        evt.Use();
                    }
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(3);

        return value;
    }

   
    public static void WaitForCompile(string returnMethod, string returnArg, string returnArg2)
    {
        CompilingWindow.Init(returnMethod, returnArg, returnArg2);
    }

    //public static void CreateElementPrefab(string elementName, string resourcesFolder)
    //{

    //    var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    go.name = elementName;

    //    var viewComponent = go.AddComponent(elementName + "View") as ViewBase;

    //    if (viewComponent == null)
    //    {
    //        Debug.LogError("Something went wrong with trying to generate the prefab.  The templates may have errors.", go);
    //        return;
    //    }

    //    var ubInstance = go.AddComponent<UBInstanceBehaviour>();
    //    resourcesFolder.Replace("Resources", elementName + "Behaviour");
    //    var behaviour = UBAssetManager.Behaviours.FirstOrDefault(p => p.name == elementName + "Behaviour") as UFrameBehaviours;
    //    if (behaviour != null)
    //    {
    //        behaviour.ViewModelType = viewComponent.GetType().Assembly.GetType(elementName + "ViewModel");
    //        ubInstance.Includes.Add(new UBInclude()
    //         {
    //             Behaviour = behaviour,
    //         });
    //        EditorUtility.SetDirty(behaviour);
    //    }

    //    EditorUtility.SetDirty(ubInstance);

    //    //viewComponent._ViewModelFrom = ViewModelRegistryType.Controller;

    //    //var type = viewComponent.GetType().Assembly.GetType(elementName + "Controller");

    //    //if (type != null)
    //    //    viewComponent._ViewModelControllerType = type.AssemblyQualifiedName;

    //    //viewComponent._ViewModelControllerMethod = "Create" + elementName;

    //    var prefabFilename =
    //        Path.Combine(resourcesFolder,
    //            elementName.ToString(CultureInfo.InvariantCulture) + ".prefab").FixAssetPath();

    //    var prefab = PrefabUtility.CreateEmptyPrefab(prefabFilename);
    //    PrefabUtility.ReplacePrefab(go, prefab, ReplacePrefabOptions.Default);

    //    Object.DestroyImmediate(go);
    //    PrefabUtility.InstantiatePrefab(Resources.LoadAssetAtPath<GameObject>(prefabFilename));
    //}

    //public static string TemplateDialog()
    //{
    //    var path =
    //        Path.GetFileNameWithoutExtension(EditorUtility.SaveFilePanelInProject("Element Name", "New Element", "", "",
    //            SelectedPath));

    //    if (path == null)
    //        return null;

    //    return Regex.Replace(path, @"\s", "");
    //}

    //[MenuItem("Assets/[u]Frame/New Controller", false, 2)]
    //public static void CreateController()
    //{
    //    SaveTemplate(SelectedPath, "SingleTemplates/ControllerTemplate", (name) => name.Replace("Controller", "") + "Controller");
    //}

    //[MenuItem("Assets/[u]Frame/New Element Structure", false, 0)]
    //public static void CreateElement()
    //{
    //    var elementName = TemplateDialog();
    //    if (string.IsNullOrEmpty(elementName)) return;
    //    var workingFolder = SelectedAssetPath;
    //    var rootFolder = SelectedAssetPath + "/" + elementName;
    //    var resourcesFolder = rootFolder + "/Resources";

    //    AssetDatabase.CreateFolder(workingFolder, elementName);
    //    AssetDatabase.CreateFolder(rootFolder, "Models");
    //    AssetDatabase.CreateFolder(rootFolder, "Resources");
    //    AssetDatabase.CreateFolder(resourcesFolder, "Materials");
    //    AssetDatabase.CreateFolder(resourcesFolder, "Textures");
    //    AssetDatabase.CreateFolder(resourcesFolder, "Effects");
    //    //AssetDatabase.CreateFolder(resourcesFolder, "Prefabs");
    //    var path = SelectedPath + "/" + elementName;

    //    SaveTemplate(path, "CombinedTemplates/ControllerTemplate", (name) => name.Replace("Controller", "") + "Controller", elementName);
    //    SaveTemplate(path, "CombinedTemplates/ViewModelTemplate", (name) => name.Replace("ViewModel", "") + "ViewModel", elementName);
    //    SaveTemplate(path, "CombinedTemplates/ViewTemplate", (name) => name.Replace("View", "") + "View", elementName);
    //    UBAssetManager.CreateAsset<UFrameBehaviours>(rootFolder, elementName + "Behaviour");
    //    AssetDatabase.Refresh();
    //    WaitForCompile("CreateElementPrefab", elementName, resourcesFolder);
    //}
    //[MenuItem("Assets/[u]Frame/New Scene Manager", false, 1)]
    //public static void CreateGame()
    //{
    //    SaveTemplate(SelectedPath, "SingleTemplates/SceneManagerTemplate", (name) => name.Replace("SceneManager", "") + "SceneManager");
    //}

    //[MenuItem("Assets/[u]Frame/New View", false, 4)]
    //public static void CreateView()
    //{
    //    uFrameToolsWindow.Init();
    //}

    //[MenuItem("Assets/[u]Frame/New ViewModel", false, 3)]
    //public static void CreateViewModel()
    //{
    //    SaveTemplate(SelectedPath, "SingleTemplates/ViewModelTemplate", (name) => name.Replace("ViewModel", "") + "ViewModel");
    //}
    //private static void SaveTemplate(string path, string templateName, Func<string, string> getFilename, string name = null)
    //{
    //    var finalName = name ?? TemplateDialog();

    //    if (string.IsNullOrEmpty(finalName)) return;

    //    var saveFilename = Path.Combine(path, getFilename(finalName) + ".cs").Replace("\\", "/");

    //    uFrameUtility.GenerateGenericTemplate(templateName, finalName, saveFilename);
    //    AssetDatabase.Refresh();
    //}
}