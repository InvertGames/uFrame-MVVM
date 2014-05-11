using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(ViewBase), true)]
public class ViewEditor : uFrameEditor
{
    public bool ShowDefaultSettings
    {
        get
        {
            return EditorPrefs.GetBool("UFRAME_ShowDefaultSettings", true);
        }
        set
        {
            EditorPrefs.SetBool("UFRAME_ShowDefaultSettings", value);
        }
    }
    public bool ShowViewModelSettings
    {
        get
        {
            return EditorPrefs.GetBool("UFRAME_ShowViewModelSettings", true);
        }
        set
        {
            EditorPrefs.SetBool("UFRAME_ShowViewModelSettings", value);
        }
    }

    public bool ShowViewSettings
    {
        get
        {
            return EditorPrefs.GetBool("UFRAME_ShowViewSettings", true);
        }
        set
        {
            EditorPrefs.SetBool("UFRAME_ShowViewSettings", value);
        }
    }

    public override void OnInspectorGUI()
    {
        UBEditor.IsGlobals = false;
        var t = target as ViewBase;
      
        if (EditorApplication.isPlaying)
        {
            base.OnInspectorGUI();
            if (EditorApplication.isPlaying)
            {
                if (t != null && t.ViewModelObject != null)
                    foreach (var p in t.ViewModelObject.Properties)
                    {
                        if (p.Value.ValueType.IsPrimitive)
                        {
                            EditorGUILayout.LabelField(p.Key, p.Value.ObjectValue.ToString());
                        }
                        else
                        {
                            EditorGUILayout.LabelField(p.Key, p.Value.Serialize());
                        }
                    }
            }
            Repaint();
            return;
        }
        
        ShowDefaultSettings = Toggle("Default", ShowDefaultSettings);
        serializedObject.Update();
        if (ShowDefaultSettings)
        {
            base.OnInspectorGUI();
            
        }
        EditorGUILayout.Space();
        if (t.IsMultiInstance)
        {
            var resolveProperty = serializedObject.FindProperty("_forceResolveViewModel");
            EditorGUILayout.PropertyField(resolveProperty, new GUIContent("Force Resolve"));
        }
        if (!t.IsMultiInstance || t.ForceResolveViewModel)
        {
            var resolveNameProperty = serializedObject.FindProperty("_resolveName");
            EditorGUILayout.PropertyField(resolveNameProperty, new GUIContent("Resolve Name"));
        }
        var overrideProperty = serializedObject.FindProperty("_overrideViewModel");
        EditorGUILayout.PropertyField(overrideProperty, new GUIContent("Initialize ViewModel"));


        if (_groupFields == null)
            GetFieldInformation(t);
        

        if (_groupFields != null)
        {
         
            foreach (var groupField in _groupFields)
            {
                if (_toggleGroups.ContainsKey(groupField.Key)) continue;
                if (groupField.Key == "View Model Properties" &&
                    !(t.OverrideViewModel)) continue;

                EditorPrefs.SetBool(groupField.Key, Toggle(groupField.Key, EditorPrefs.GetBool(groupField.Key, false)));
                if (EditorPrefs.GetBool(groupField.Key, false))
                {
                    foreach (var field in groupField.Value)
                    {
                        try
                        {

                           // serializedObject.GetIterator().Reset();
                            var property = serializedObject.FindProperty(field.Name);
                            if (property == null) continue;
                            if (property.propertyType == SerializedPropertyType.Vector2)
                            {

                                var newValue = EditorGUILayout.Vector2Field(property.name, property.vector2Value);
                                if (newValue != property.vector2Value)
                                {
                                    property.vector2Value = newValue;
                                }

                            }
                            else if (property.propertyType == SerializedPropertyType.Vector3)
                            {
                                var newValue = EditorGUILayout.Vector3Field(property.name, property.vector3Value);
                                if (newValue != property.vector3Value)
                                {
                                    property.vector2Value = newValue;
                                }
                            }
                            else
                            {
                                EditorGUILayout.PropertyField(property);
                            }
                           
                        }
                        catch (Exception ex)
                        {
                            Debug.Log(field.Name + ex.Message);
                        }
                    }
                }
            }
            EditorPrefs.SetBool("UFRAME_BindingsOpen", Toggle("Bindings", EditorPrefs.GetBool("UFRAME_BindingsOpen", false)));
            if (EditorPrefs.GetBool("UFRAME_BindingsOpen", false))
            {
               
                foreach (var group in _toggleGroups)
                {
                    var property = serializedObject.FindProperty(@group.Value.Name);
                    EditorGUILayout.PropertyField(property, new GUIContent(property.name.Replace("_", "").Replace("Bind", "")));
                    if (property.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        if (_groupFields != null)
                        {
                            if (_groupFields.ContainsKey(@group.Key))
                            {
                                foreach (var groupField in _groupFields[@group.Key])
                                {
                                    var subProperty = serializedObject.FindProperty(groupField.Name);
                                    
                                    if (subProperty != null)
                                    {

                                        EditorGUILayout.PropertyField(subProperty, new GUIContent(subProperty.name.Replace(@group.Key, "").Replace("_", "")));
                                    }
                                }
                            }
                        }
                        EditorGUI.indentLevel--;
                    }
                    property.Reset();
                }
            }
        }


        serializedObject.ApplyModifiedProperties();
    }
}
