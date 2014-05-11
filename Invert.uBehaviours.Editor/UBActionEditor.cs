//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(UBAction), true)]
//public class UBActionEditor : UBEditor
//{
//    public static UBAction Clipboard
//    {
//        get;
//        set;
//    }

//  //  public UBehavioursEditor MainEditor { get; set; }

//    public UBAction Target
//    {
//        get { return target as UBAction; }
//    }

//    public SerializedObject SheetObject { get; set; }

//    public SerializedProperty SheetActionArray { get; set; }

//    public int Index { get; set; }

//    public void MoveUp()
//    {
//        Target.ActionSheet.Actions.RemoveAt(Index);
//        Target.ActionSheet.Actions.Insert(--Index, Target);
//    }

//    public void MoveDown()
//    {
//        Target.ActionSheet.Actions.RemoveAt(Index);
//        Target.ActionSheet.Actions.Insert(++Index, Target);
//    }

//    public override void OnInspectorGUI()
//    {
//        //base.OnInspectorGUI();
//        EditorGUI.indentLevel = 0;
//        var expanedProperty = serializedObject.FindProperty("_Expanded");
//        var breakPointProperty = serializedObject.FindProperty("_Breakpoint");
//        var style = expanedProperty.boolValue ? UBStyles.CommandBarOpenStyle : UBStyles.CommandBarClosedStyle;
//        var rect = GetRect(style);

//        GUI.Box(rect, "", style);

//        if (breakPointProperty.boolValue || Target.IsCurrentlyActive)
//        {
//            var indicatorRect = new Rect(rect) { width = 4 };
//            indicatorRect.x += 1;
//            GUI.Box(indicatorRect, string.Empty, Target.IsCurrentlyActive ? UBStyles.CurrentActionBackgroundStyle : UBStyles.DebugBackgroundStyle);
//        }

//        var labelStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft, fontSize = 11 };
//        var labelRect = new Rect(rect.x + 30, rect.y, rect.width - 80, rect.height);
//        serializedObject.Update();
//        expanedProperty.boolValue = GUI.Toggle(labelRect, expanedProperty.boolValue, serializedObject.targetObject.ToString(), labelStyle);
//        serializedObject.ApplyModifiedProperties();
//        var eventOptionsButtonRect = new Rect(rect.x + 10, rect.y + 5, 16, 16);

//        DoActionOptions(Target, serializedObject, eventOptionsButtonRect, breakPointProperty);

//        if (Target.ActionSheet.Actions.LastOrDefault() != Target)
//        {
//            var moveDown = new Rect(rect.x + rect.width - 52, rect.y + 4, 16, 16);
//            if (GUI.Button(moveDown, "", UBStyles.ArrowDownButtonStyle))
//            {
//                MoveDown();
//                //UnityEditorInternal.ComponentUtility.MoveComponentDown(Target as UBAction);
//            }
//        }
//        if (Target.ActionSheet.Actions.FirstOrDefault() != Target)
//        {
//            var moveUpRect = new Rect(rect.x + rect.width - 36, rect.y + 4, 16, 16);
//            if (GUI.Button(moveUpRect, "", UBStyles.ArrowUpButtonStyle))
//            {
//                MoveUp();
//                //UnityEditorInternal.ComponentUtility.MoveComponentUp(actionProperty.targetObject as Component);
//            }
//        }

//        var removeRect = new Rect(rect.x + rect.width - 20, rect.y + 4, 16, 16);
//        if (GUI.Button(removeRect, "", UBStyles.RemoveButtonStyle))
//        {
//            //var actionSheets = Target.GetAvailableActionSheets(UBehavioursEditor.InstanceTarget, serializedObject);
//            //foreach (var actionSheet in actionSheets)
//            //{
//            //    //if (actionSheet.Value != null)
//            //    //{
//            //    //    DestroyImmediate(actionSheet.Value, true);
//            //    //}
//            //}

//            Target.ActionSheet.Actions.Remove(serializedObject.targetObject as UBAction);
//            DestroyImmediate(serializedObject.targetObject, true);
//        }
//        if (expanedProperty.boolValue && target != null)
//        {
//            DrawDefaultInspector();
//            EditorGUILayout.Space();
//        }
////        if (MainEditor != null)
////        {
////            var actionSheets = Target.GetAvailableActionSheets(UBehavioursEditor.InstanceTarget, serializedObject);
////            foreach (var actionSheet in actionSheets)
////            {
//////                DoTriggerButton(MainEditor, actionSheet.Key, actionSheet.Value, actionSheets, Target.ActionSheet.IsInstance, expanedProperty.boolValue);
////            }
////        }
//    }

//    public static void DoActionOptions(UBAction actionObject, SerializedObject serialiedAction, Rect eventOptionsButtonRect, SerializedProperty breakPointProperty)
//    {
//        var style = UBStyles.ActionEnabledButtonStyle;

//        if (breakPointProperty.boolValue)
//            style = UBStyles.BreakpointButtonStyle;
//        else if (!((UBAction)actionObject).enabled)
//            style = UBStyles.TriggerInActiveButtonStyle;

//        if (GUI.Button(eventOptionsButtonRect, string.Empty, style))
//        {
//            GenericMenu menu = new GenericMenu();
//            var behaviour = (UBAction)actionObject;
//            menu.AddItem(new GUIContent("Enabled"), !breakPointProperty.boolValue && behaviour.enabled, () =>
//            {
//                behaviour.enabled = true;
//                serialiedAction.Update();
//                breakPointProperty.boolValue = false;
//                serialiedAction.ApplyModifiedProperties();
//            });
//            menu.AddItem(new GUIContent("Disabled"), !breakPointProperty.boolValue && !behaviour.enabled, () =>
//            {
//                behaviour.enabled = false;
//                serialiedAction.Update();
//                breakPointProperty.boolValue = false;
//                serialiedAction.ApplyModifiedProperties();
//            });
//            menu.AddItem(new GUIContent("Breakpoint"), breakPointProperty.boolValue, () =>
//            {
//                behaviour.enabled = true;
//                serialiedAction.Update();
//                breakPointProperty.boolValue = true;
//                serialiedAction.ApplyModifiedProperties();
//            });
//            menu.AddSeparator("");
//            menu.AddItem(new GUIContent("Copy Action"), breakPointProperty.boolValue, () =>
//            {
//                Clipboard = actionObject;
//            });
//            menu.ShowAsContext();
//        }

//        //var newValue = GUI.Toggle(eventOptionsButtonRect, action.enabled, "",
//        //    action.enabled ? UBStyles.ActionEnabledButtonStyle : UBStyles.EventInActiveButtonStyle);

//        //if (newValue != action.enabled)
//        //{
//        //    action.enabled = newValue;
//        //}
//    }
//}