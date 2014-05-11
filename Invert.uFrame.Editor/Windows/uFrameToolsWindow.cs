//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;

//public class uFrameToolsWindow : EditorWindow
//{
//    public string _WorkingAssetFolder;
//    public string _WorkingFolder;
//    private string _compileCompleteCallback;
//    private bool _generateModel = true;
//    private bool _generatePrefab = true;
//    private string _modelBaseType = null;
//    private bool _shouldGeneratePrefabNow = false;
//    private string _typeName = "";
//    //private static GameManager _manager = null;

//    //[MenuItem("[u]Frame/View Tools", false, 1)]
//    public static void Init()
//    {
//        // Get existing open window or if none, make a new one:
//        var window = (uFrameToolsWindow)GetWindow(typeof(uFrameToolsWindow));
//        window.title = "View Tools";
//        window._WorkingFolder = EditorExtensions.SelectedPath;
//        window._WorkingAssetFolder = EditorExtensions.SelectedAssetPath;
//        window.Show();
//    }

//    public void OnGUI()
//    {
//        DrawTitleBar("View Tools");

//        GUILayout.Label("Create View", EditorStyles.boldLabel);
//        CreateViewGUI();
//    }
//    public static void DrawTitleBar(string subTitle)
//    {
//        //GUI.Label();

//        GUILayout.Label("", uFrameStyles.TitleStyleWithBackground);
//        var rectA = GUILayoutUtility.GetLastRect();
//        GUI.Label(new Rect(52, 6 + rectA.y, 100, 40), subTitle, uFrameStyles.TitleStyle);

//        var rect = GUILayoutUtility.GetLastRect();
//        GUI.DrawTexture(new Rect(18, 4 + rect.y, 128, 32), uFrameStyles._UFrameTextureSmall2D);
//    }

//    public void OnViewCompleted()
//    {
//        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
//        go.name = _typeName;
//        go.AddComponent(_typeName + "View");
//        if (_WorkingFolder == null)
//        {
//            _WorkingFolder = EditorExtensions.SelectedPath;
//            _WorkingAssetFolder = EditorExtensions.SelectedAssetPath;
//        }

//        //if (Selection.activeGameObject != null)
//        //    go.transform.parent = Selection.activeGameObject.transform;

//        var prefabFilename =
//            Path.Combine(EditorExtensions.SelectedAssetPath,
//                _typeName.ToString(CultureInfo.InvariantCulture) + ".prefab").FixAssetPath();
//        var prefab = PrefabUtility.CreateEmptyPrefab(prefabFilename);
//        PrefabUtility.ReplacePrefab(go, prefab, ReplacePrefabOptions.Default);

//        DestroyImmediate(go);
//        PrefabUtility.InstantiatePrefab(Resources.LoadAssetAtPath<GameObject>(prefabFilename));
//    }

//    public void Update()
//    {
//        if (_shouldGeneratePrefabNow)
//        {
//            if (EditorApplication.isCompiling)
//            {
//                return;
//            }

//            _shouldGeneratePrefabNow = false;
//            this.GetType().GetMethod(_compileCompleteCallback).Invoke(this, null);
//            //_compileCompleteCallback = null;
//        }
//    }

//    public void WaitForCompileToComplete(string complete)
//    {
//        _compileCompleteCallback = complete;
//        AssetDatabase.Refresh();
//        _shouldGeneratePrefabNow = _generatePrefab;
//    }

//    private void CreateView()
//    {
//        if (_generateModel)
//        {
//            var modelFilename = Path.Combine(_WorkingFolder,
//                _typeName.ToString(CultureInfo.InvariantCulture) + "ViewModel.cs");

//            uFrameUtility.GenerateGenericTemplate("CombinedTemplates/ViewModelTemplate", _typeName, modelFilename, new Dictionary<string, string>()
//            {
//                { "ViewModelBaseType", _modelBaseType}
//            });
//        }

//        var viewFilename = Path.Combine(_WorkingFolder,
//            _typeName.ToString(CultureInfo.InvariantCulture) + "View.cs");

//        uFrameUtility.GenerateGenericTemplate("CombinedTemplates/ViewTemplate", _typeName, viewFilename, new Dictionary<string, string>()
//            {
//                { "ViewModelBaseType", _generateModel ? _typeName + "ViewModel" : _modelBaseType},
//                { "ViewModelType", _generateModel ? _typeName + "ViewModel" : _modelBaseType}
//            });

//        WaitForCompileToComplete("OnViewCompleted");
//    }

//    private void CreateViewGUI()
//    {
//        _typeName = EditorGUILayout.TextField("Name", _typeName);

//        _generateModel = EditorGUILayout.Toggle("Generate ViewModel Class", _generateModel);

//        var txt = _generateModel ? "ViewModel Base Type" : "ViewModel For View";
//        var types = uFrameUtility.GetDerivedTypes<ViewModel>(true, _generateModel).Select(p => p.FullName).ToArray();
//        uFrameUtility.PopupField(txt, _modelBaseType, v => _modelBaseType = v, types);

//        _generatePrefab = EditorGUILayout.Toggle("Create Prefab", _generatePrefab);

//        if (_generatePrefab)
//        {
//            EditorGUILayout.HelpBox("Make sure the prefab makes it to a resources path.", MessageType.Info);
//        }

//        if (!string.IsNullOrEmpty(this._typeName))
//        {
//            if (GUILayout.Button("Generate"))
//            {
//                CreateView();
//            }
//        }

//        else
//        {
//            EditorGUILayout.HelpBox("Specify a name to generate.", MessageType.Warning);
//        }
//    }
//}