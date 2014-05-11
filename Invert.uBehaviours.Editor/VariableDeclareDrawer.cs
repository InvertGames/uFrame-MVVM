using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UBVariableDeclare))]
public class VariableDeclareDrawer : PropertyDrawer
{
    public static void DoDeclareField(Rect position, SerializedProperty property, bool isLabel = false, bool editWithCopy = false)
    {
        var nameProperty = property.FindPropertyRelative("_name");
        var varTypeProperty = property.FindPropertyRelative("_varType");
        var varTypeValue = (UBVarType)varTypeProperty.enumValueIndex;
        var defaultValue = property.FindPropertyRelative(string.Format("_{0}Value", varTypeValue.ToString().ToLower()));

        if (defaultValue == null)
        {
            defaultValue = property.FindPropertyRelative("_objectValue");
        }
        if (defaultValue.name == "_vector2Value")
        {
            defaultValue.vector2Value = EditorGUI.Vector2Field(position, nameProperty.stringValue, defaultValue.vector2Value);
        }
        else if (defaultValue.name == "_vector3Value")
        {
            defaultValue.vector3Value = EditorGUI.Vector3Field(position, nameProperty.stringValue, defaultValue.vector3Value);
        }
        else if (defaultValue.name == "_objectValue")
        {
            var objectValueTypeString = property.FindPropertyRelative("_objectValueType").stringValue;
            Type objectValueType = null;
            if (string.IsNullOrEmpty(objectValueTypeString))
                objectValueType = typeof(UnityEngine.Object);
            else
                objectValueType = UBHelper.GetType(objectValueTypeString);

            if (objectValueType == null)
                EditorGUILayout.HelpBox(
                    string.Format("Could not find type {0} for {1} variable.", objectValueTypeString,
                        nameProperty.stringValue), MessageType.Error);
            else
            {
                if (isLabel)
                {
                    EditorGUI.LabelField(position, nameProperty.stringValue, objectValueType.Name);
                }
                else
                {
                    defaultValue.objectReferenceValue = EditorGUI.ObjectField(position, nameProperty.stringValue,
                        defaultValue.objectReferenceValue,
                        objectValueType, true);
                }
            }
        }
        else if (defaultValue.name == "_enumValue")
        {
            var enumValueTypeString = property.FindPropertyRelative("_enumType").stringValue;
            Type enumValueType = null;
            if (string.IsNullOrEmpty(enumValueTypeString))
                enumValueType = typeof(AnimationPlayMode);
            else
                enumValueType = UBHelper.GetType(enumValueTypeString);

            if (enumValueType == null)
                EditorGUILayout.HelpBox(
                    string.Format("Could not find type {0} for {1} variable.", enumValueTypeString,
                        nameProperty.stringValue), MessageType.Error);
            else
            {
                defaultValue.intValue = EditorGUI.Popup(position, nameProperty.stringValue, defaultValue.intValue,
                    Enum.GetNames(enumValueType));

                //defaultValue.objectReferenceValue = EditorGUI.EnumPopup(position, nameProperty.stringValue,
                //    defaultValue.objectReferenceValue,
                //    enumValueType, true);
            }
        }
        else
        {
            if (editWithCopy)
            {
                EditorGUI.PropertyField(
                    position,
                    defaultValue, new GUIContent(nameProperty.stringValue));
            }
            else
            {
                var copy = defaultValue.Copy();
                EditorGUI.PropertyField(
                    position,
                    copy, new GUIContent(nameProperty.stringValue));
            }
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        DoDeclareField(position, property);
    }
}