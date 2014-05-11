using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumTypesAttribute))]
public class EnumTypesDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        var type = string.IsNullOrEmpty(property.stringValue)
            ? typeof(AnimationPlayMode)
            : UBHelper.GetType(property.stringValue);

        if (GUILayout.Button(type.Name))
        {
            UBTypesWindow.Init("Enum Type", ActionSheetHelpers.GetEnumTypes(),
                (prettyName, qualifiedName) =>
                {
                    property.serializedObject.Update();
                    property.stringValue = qualifiedName;
                    property.serializedObject.ApplyModifiedProperties();
                });
        }
    }
}