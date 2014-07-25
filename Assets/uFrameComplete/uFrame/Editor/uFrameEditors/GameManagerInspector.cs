using System;
using System.Collections.Generic;
using System.IO;
using Invert.Common;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(GameManager), true)]
public class GameManagerInspector : ManagerInspector<SceneManager>
{
    private bool _RenderSettingsOpen;



    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawTitleBar("Game Manager");
        serializedObject.Update();




        if (Application.isPlaying)
        {
            Toggle("Game Container - Instances", _RenderSettingsOpen,false);
            foreach (var instance in GameManager.Container.Instances)
            {
                if (UBEditor.DoTriggerButton(new UBTriggerContent(instance.GetHashCode().ToString() + ": " + (instance.Name ?? instance.Base.Name),
                    UBStyles.EventButtonLargeStyle, null, UBStyles.RemoveButtonStyle, null, false,
                    TextAnchor.MiddleCenter) {SubLabel = instance.Instance.GetType().Name}))
                {
                    Debug.Log(instance.Instance);
                }

            }
            Toggle("Game Container - Mappings", _RenderSettingsOpen,false);
            foreach (var instance in GameManager.Container.Mappings)
            {
                if (UBEditor.DoTriggerButton(new UBTriggerContent(instance.From.Name,
                    UBStyles.EventButtonLargeStyle, null, UBStyles.RemoveButtonStyle, null, false,
                    TextAnchor.MiddleCenter) {SubLabel = instance.To.GetType().Name}))
                {

                }

            }
            

        }
        else
        {
            EditorGUILayout.HelpBox("The View to load when the scene starts.", MessageType.None);
            var p = serializedObject.FindProperty("_Start");
            if (p.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Error start scene manager is not set.", MessageType.Error);
            }
            EditorGUILayout.PropertyField(p);
            EditorGUILayout.HelpBox("The loading scene that will be used when switching scenes via GameManager.SwitchSceneAndLevel<View>().", MessageType.None);
            p = serializedObject.FindProperty("_LoadingLevel");
            EditorGUILayout.PropertyField(p);
            EditorGUILayout.HelpBox("Pro Users: Will use non asynchronous loading.", MessageType.None);
            p = serializedObject.FindProperty("_DontUseAsyncLoading");
            EditorGUILayout.PropertyField(p);


            EditorGUILayout.HelpBox("The render settings that will apply when the scene loads.", MessageType.None);
            _RenderSettingsOpen = Toggle("Render Settings", _RenderSettingsOpen);
            if (_RenderSettingsOpen)
            {
                p = serializedObject.FindProperty("_Fog");
                EditorGUILayout.PropertyField(p);
                p = serializedObject.FindProperty("_FogColor");
                EditorGUILayout.PropertyField(p);
                p = serializedObject.FindProperty("_FogMode");
                EditorGUILayout.PropertyField(p);
                p = serializedObject.FindProperty("_FogDensity");
                EditorGUILayout.PropertyField(p);
                p = serializedObject.FindProperty("_LinearFogStart");
                EditorGUILayout.PropertyField(p);
                p = serializedObject.FindProperty("_LinearFogEnd");
                EditorGUILayout.PropertyField(p);
                p = serializedObject.FindProperty("_AmbientLight");
                EditorGUILayout.PropertyField(p);
                p = serializedObject.FindProperty("_SkyboxMaterial");
                EditorGUILayout.PropertyField(p);
                p = serializedObject.FindProperty("_HaloStrength");
                EditorGUILayout.PropertyField(p);
                p = serializedObject.FindProperty("_FlareStrength");
                EditorGUILayout.PropertyField(p);
                if (GUILayout.Button("Load From Scene"))
                {
                    var t = Target as GameManager;
                    t.LoadRenderSettings();
                }
            }
            
        }

        if (serializedObject.ApplyModifiedProperties())
        {
            var t = Target as GameManager;
            t.ApplyRenderSettings();
        }
    }

    protected override bool ExistsInScene(Type itemType)
    {
        return FindObjectOfType(itemType) != null;
    }

    protected override string GetTypeNameFromName(string name)
    {
        return name + "Game";
    }

    protected override void OnAdd(string typeName)
    {
        //base.OnAdd(typeName);
        var go = new GameObject("_" + typeName);
        go.AddComponent(typeName);
        go.transform.parent = null;
        Selection.objects = new Object[] { go };
    }

}