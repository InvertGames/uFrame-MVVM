using System;
using UnityEditor;
using UnityEngine;

[UBCustomDrawer(typeof(InstanceTemplateListAttribute))]
public class InstanceTemplatesListDrawer : UBPropertyDrawer
{


    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        var container = UndoTarget as UBSharedBehaviour;
        if (container == null)
        {
            EditorGUILayout.HelpBox("This can't be on an instance.",MessageType.Error);
            return value;
        }
        var instanceTemplates = container.TriggerTemplates;
        var selectedIndex = Array.IndexOf(instanceTemplates, value);

        var newValue = EditorGUILayout.Popup("Template Name", selectedIndex, instanceTemplates);
        if (newValue != selectedIndex)
        {
            return instanceTemplates[newValue];
            
        }
        return value;
    }
}
