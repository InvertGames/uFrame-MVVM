using System.Linq;
using Invert.Common;
using UnityEditor;
using UnityEngine;

public class UBExplorerWindow : EditorWindow
{
    private Vector2 _scrollPosition;

    public string SearchText
    {
        get { return EditorPrefs.GetString("UBActionsSearchText", string.Empty); }
        set { EditorPrefs.SetString("UBActionsSearchText", value); }
    }
    public bool SceneBehavioursOpen
    {
        get { return EditorPrefs.GetBool("UBSceneBehavioursOpen", true); }
        set { EditorPrefs.SetBool("UBSceneBehavioursOpen", value); }
    }
    public bool CommonBehavioursOpen
    {
        get { return EditorPrefs.GetBool("UBCommonBehavioursOpen", true); }
        set { EditorPrefs.SetBool("UBCommonBehavioursOpen", value); }
    }
    public bool PrefabBehavioursOpen
    {
        get { return EditorPrefs.GetBool("UBPrefabBehavioursOpen", true); }
        set { EditorPrefs.SetBool("UBPrefabBehavioursOpen", value); }
    }
    public bool ErrorsBehavioursOpen
    {
        get { return EditorPrefs.GetBool("UBErrorsBehavioursOpen", true); }
        set { EditorPrefs.SetBool("UBErrorsBehavioursOpen", value); }
    }

    [MenuItem("Window/UB Explorer", false, 1)]
    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        var window = (UBExplorerWindow)GetWindow(typeof(UBExplorerWindow));

        window.title = "UB Explorer";
        window.Show();
    }

    public void OnGUI()
    {
        UBEditor.IsGlobals = true;

        //if (Selection.gameObjects.Length > 0)
        //{
        //    Selection.gameObjects[0].GetComponent<UBInstanceBehaviour>();
        //    if (Selection.gameObjects[0].GetComponent<UBInstanceBehaviour>() == null)
        //    {
        //        if (
        //           UBEditor.DoTriggerButton(new UBTriggerContent("Create Behaviour on " + Selection.gameObjects[0].name, UBStyles.CommandBarOpenStyle, null, null,
        //               null, true) { }))
        //        {
        //            Selection.gameObjects[0].AddComponent<UBInstanceBehaviour>();
        //        }
        //    }
        //}
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        EditorGUILayout.BeginVertical();
        var debugHandler = UBActionSheet.ExecutionHandler as DebugExecutionHandler;
        if (debugHandler != null)
        {
            if (UBEditor.DoToolbar("Errors", ErrorsBehavioursOpen, () =>
            {

                debugHandler.Errors.Clear();
            }, null, null, UBStyles.RemoveButtonStyle))
            {
                ErrorsBehavioursOpen = !ErrorsBehavioursOpen;
            };
        }
       
        if (ErrorsBehavioursOpen && debugHandler != null)
            UBEditor.DoNotifications(debugHandler.Errors.Distinct(new BehaviourErrorComparer()).Cast<IBehaviourNotification>().ToArray(), (action) =>
            {
                TBehavioursEditorBase.JumpTo = action.Source as UBAction;
                Selection.activeObject = action.SourceObject;
            }, UBStyles.DebugBackgroundStyle);


        //var scene = FindObjectsOfType<UBInstanceBehaviour>();
        //if (UBEditor.DoToolbar("Scene Behaviours", SceneBehavioursOpen))
        //    SceneBehavioursOpen = !SceneBehavioursOpen;
        //if (SceneBehavioursOpen)
        //foreach (var instance in scene)
        //{
        //    if (
        //        UBEditor.DoTriggerButton(new UBTriggerContent(instance.name, UBStyles.EventSmallButtonStyle, null, null,
        //            null, true)))
        //    {
        //        Selection.activeObject = instance;
        //    }
        //}

        //if (UBEditor.DoToolbar("Prefab Behaviours", PrefabBehavioursOpen))
        //    PrefabBehavioursOpen = !PrefabBehavioursOpen;

        //if (PrefabBehavioursOpen)
        //    foreach (var uBehaviour in UBAssetManager.BehaviourPrefabs)
        //    {
        //        if (
        //            UBEditor.DoTriggerButton(new UBTriggerContent(uBehaviour.name, UBStyles.EventSmallButtonStyle, null, null,
        //                null, true)))
        //        {
        //            Selection.activeObject = uBehaviour;
        //        }
        //    }

        if (UBEditor.DoToolbar("Asset Behaviours", CommonBehavioursOpen))
            CommonBehavioursOpen = !CommonBehavioursOpen;

        if (CommonBehavioursOpen)
        foreach (var uBehaviour in UBAssetManager.Behaviours)
        {
            UBSharedBehaviour behaviour = uBehaviour;
            if (
                UBEditor.DoTriggerButton(new UBTriggerContent(uBehaviour.name, UBStyles.EventSmallButtonStyle, null, UBStyles.AddButtonStyle,
                    ()=>AddToSelection(behaviour), true)))
            {
                Selection.activeObject = uBehaviour;
            }
        }

        foreach (var global in UBAssetManager.Globals)
        {
            var editor = Editor.CreateEditor(global) as TBehavioursEditorBase;
            editor.OnInspectorGUI();
        }
        var newMode = EditorGUILayout.Toggle("Debug Mode", UBAssetManager.DebugMode);
        if (newMode != UBAssetManager.DebugMode)
        {
             UBAssetManager.DebugMode = newMode;
        }
        
        EditorGUILayout.EndVertical();
    EditorGUILayout.EndScrollView();
    }

    private void AddToSelection(UBSharedBehaviour uBehaviour)
    {
        var component = Selection.gameObjects[0].AddComponent<UBComponent>();
        component.BehaviourInclude.Behaviour = uBehaviour;
        EditorUtility.SetDirty(component);
        EditorUtility.SetDirty(Selection.gameObjects[0]);
        
    }

    public void OnSelectionChange()
    {
        Repaint();
    }
}