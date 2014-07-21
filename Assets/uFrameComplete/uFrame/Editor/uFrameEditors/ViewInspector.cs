using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(ViewBase), true)]
public class ViewInspector : uFrameInspector
{
    private Dictionary<string, ICommand> _commands;

    public static bool ShowInfoLabels
    {
        get
        {
            return EditorPrefs.GetBool("UFRAME_ShowInfoLabels", true);
        }
        set
        {
            EditorPrefs.SetBool("UFRAME_ShowInfoLabels", value);
        }
    }

    [MenuItem("Tools/[u]Frame/Info Labels")]
    public static void ShowHideInfoLabels()
    {
        ShowInfoLabels = !ShowInfoLabels;
    }

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

    public void OnSceneGUI()
    {
        Handles.BeginGUI();
        var padding = 10f;
        var titleContent = new GUIContent(target.name);
        var subTitleContent = new GUIContent(target.GetType().Name);
        var titleSize = UFStyles.ViewBarTitleStyle.CalcSize(titleContent);
        var subTitleSize = UFStyles.ViewBarSubTitleStyle.CalcSize(subTitleContent);
        var maxTextWidth = Mathf.Max(titleSize.x, subTitleSize.x);
        var barWidth = (padding*4f) + maxTextWidth + (36 * 1);
        var rect = new Rect(15f, 15f, barWidth, 48f);
        UFStyles.DrawExpandableBox(rect, UFStyles.SceneViewBar, "");
        GUILayout.BeginArea(rect);
        
        GUILayout.BeginHorizontal();
        GUILayout.Space(padding);
        if (GUILayout.Button(new GUIContent("", "View " + subTitleContent.text + " in Element Designer"), UFStyles.EyeBall))
        {
            uFrameEditorSceneManager.NavigateBack(target as ViewBase);
        }
        GUILayout.Space(padding);
        GUILayout.BeginVertical();
        GUILayout.Space(6f);
        GUILayout.Label(titleContent,UFStyles.ViewBarTitleStyle,GUILayout.Width(maxTextWidth));
        GUILayout.Label(subTitleContent, UFStyles.ViewBarSubTitleStyle, GUILayout.Width(maxTextWidth));

        GUILayout.EndVertical();
        //GUILayout.Space(padding);
        //if (GUILayout.Button(new GUIContent("", "Move to the previous " + subTitleContent.text), UFStyles.NavigatePreviousStyle))
        //{
            
        //    uFrameEditorSceneManager.NavigatePrevious();
        //}
        //if (GUILayout.Button(new GUIContent("","Move to the next " + subTitleContent.text), UFStyles.NavigateNextStyle))
        //{
        //    uFrameEditorSceneManager.NavigateNext();
        //}
        //GUILayout.Space(padding);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        Handles.EndGUI();
    }

    public void Info(string message)
    {
        if (!ShowInfoLabels) return;
        EditorGUILayout.HelpBox(message,MessageType.Info);
    }
    public void Warning(string message)
    {
        
        EditorGUILayout.HelpBox(message, MessageType.Warning);
    }
    public override void OnInspectorGUI()
    {
        UBEditor.IsGlobals = false;
        var t = target as ViewBase;
      
    
        
        ShowDefaultSettings = Toggle("Default", ShowDefaultSettings);
        serializedObject.Update();
        if (ShowDefaultSettings)
        {
          
            base.OnInspectorGUI();
            
        }
        if (EditorApplication.isPlaying)
        {
            if (t != null)
                EditorGUILayout.LabelField("Id", t.ViewModelObject.Identifier);
            if (t != null)
                EditorGUILayout.LabelField("# References", t.ViewModelObject.References.ToString());

            base.OnInspectorGUI();
            DrawPlayModeGui(t);
            serializedObject.ApplyModifiedProperties();
            return;
        }
        EditorGUILayout.Space();
        if (t.IsMultiInstance)
        {
            Info("Store the ViewModel instance in the container.  So other views can use the same instance.");
            var resolveProperty = serializedObject.FindProperty("_forceResolveViewModel");
            EditorGUILayout.PropertyField(resolveProperty, new GUIContent("Force Resolve"));
        }

        if (t != null)
            EditorGUILayout.LabelField("Id", t.Identifier);

        if (!t.IsMultiInstance || t.ForceResolveViewModel)
        {
            Info("The name that is used to share this instance among others (if any).");
            var resolveNameProperty = serializedObject.FindProperty("_resolveName");
            EditorGUILayout.PropertyField(resolveNameProperty, new GUIContent("Resolve Name"));
            if (!t.IsMultiInstance && !string.IsNullOrEmpty(resolveNameProperty.stringValue))
            {
                Warning(
                    "When using a 'ResolveName' on a single instance element, the element must be initialized manually in the scene manager's setup method.");
            }
            
        }

        Info("This should always be checked except when you are instantiating it manually, or its using a shared instance that is already being initialized.");
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
           
            var btnContent = new GUIContent("Show In Designer");
            if (GUI.Button(UBEditor.GetRect(UFStyles.ButtonStyle),btnContent,UFStyles.ButtonStyle))
            {
                uFrameEditorSceneManager.NavigateBack(target as ViewBase);
            }
        }


        serializedObject.ApplyModifiedProperties();
    }

    public ViewBase Target
    {
        get { return (ViewBase) target; }
    }
    public Dictionary<string, ICommand> Commands
    {
        get
        {
            if (_commands == null)
            {
                if (Target.ViewModelObject != null)
                {
                    _commands = Target.ViewModelObject.Commands;
                }
            }
            return _commands;
        }
    }
    private void DrawPlayModeGui(ViewBase t)
    {
        if (EditorApplication.isPlaying)
        {
            if (t != null && t.ViewModelObject != null)
                foreach (var p in t.ViewModelObject.Properties)
                {
                    var serialized = p.Value.Serialize().ToString();
                    if (serialized.Length > 100)
                    {
                        serialized = serialized.Substring(0, 100);
                    }
                    EditorGUILayout.LabelField(p.Key.Replace("_", "").Replace("Property", ""), serialized);
                    //if (p.Value.ValueType.IsPrimitive)
                    //{
                    //    EditorGUILayout.LabelField(p.Key, p.Value.ObjectValue.ToString());
                    //}
                    //else
                    //{
                    //    EditorGUILayout.LabelField(p.Key, p.Value.Serialize());
                    //}
                }

            if (Commands != null)
            {
                foreach (var command in Commands)
                {
                    if (GUI.Button(UBEditor.GetRect(UFStyles.ButtonStyle), command.Key, UFStyles.ButtonStyle))
                    {
                        Target.ExecuteCommand(command.Value);
                    }
                }
            }
        }
        Repaint();
        return;
    }
}