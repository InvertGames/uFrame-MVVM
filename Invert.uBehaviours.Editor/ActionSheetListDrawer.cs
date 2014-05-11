using UnityEditor;
using UnityEngine;

//[CustomPropertyDrawer(typeof (ActionSheetList))]
//public class ActionSheetListDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        //base.OnGUI(position, property, label);

//        var actionSheetList = this.fieldInfo.GetValue(property.serializedObject.targetObject) as ActionSheetList;
        
//        UBEditor.DoToolbar("ActionSheets", true, () =>
//        {
//            actionSheetList.Add(new ActionSheet() { Name = "My Action Sheet" });
//        }); 

//        foreach (var actionsheet in actionSheetList.Items)
//        {
//            UBEditor.DoTriggerButton(actionsheet.Name, UBStyles.EventButtonStyle);
//        }
//    }
//}