using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UBehaviours.Actions;
using UnityEditor;

public class UBUtils
{
    public static List<MemberInfo> CachedMemberInfos;

    public static UBAction ClipboardAction { get; set; }

    public static UBAction CopyAction(UBAction action)
    {
        var sheet = action.ActionSheet;
        var sheetCopy = CopyActionSheet(sheet);
        sheetCopy.Load(sheet.RootContainer, sheet.TriggerInfo);
        var newAction = sheetCopy.Items[sheet.Items.IndexOf(action)];
        // Clear all of the action sheets
        var sheetInfos = newAction.GetAvailableActionSheets(action.RootContainer);
        foreach (var sheetInfo in sheetInfos)
        {
            sheetInfo.Clear(newAction);
        }
        return newAction;
    }

    public static UBActionSheet CopyActionSheet(UBActionSheet source)
    {
        var actionSheet = new UBActionSheet(source);
        return actionSheet;
    }

    public static void CopyActionToClipboard(UBAction action)
    {
        ClipboardAction = action;
    }

    public static void DuplicateAction(UBAction action)
    {
        Undo.RecordObject(action.RootContainer as UnityEngine.Object, "Duplicate " + action.ToString());
        var copy = CopyAction(action);
        action.ActionSheet.AddItem(copy);
        EditorUtility.SetDirty(action.RootContainer as UnityEngine.Object);
    }

    public static void MoveActionAfter(UBAction action, UBAction before)
    {
        if (action == null || before == null) return;
        var sheet = action.ActionSheet;

        var oldIndex = sheet.Actions.IndexOf(action);
        var newIndex = sheet.Actions.IndexOf(before) + 1;

        MoveAction(sheet, oldIndex, newIndex);

        //sheet.Actions.Remove(action);
        //sheet.Actions.Insert(sheet.Actions.IndexOf(before) + 1, action);
        sheet.Save(action.RootContainer);
        EditorUtility.SetDirty(action.RootContainer as UnityEngine.Object);
    }

    private static void MoveAction(UBActionSheet sheet, int oldIndex, int newIndex)
    {
        if (oldIndex >= sheet.Actions.Count || newIndex > sheet.Actions.Count) return;
        var item = sheet.Actions[oldIndex];
        sheet.Actions.RemoveAt(oldIndex);

        if (newIndex > oldIndex) newIndex--;
        // the actual index could have shifted due to the removal

        sheet.Actions.Insert(newIndex, item);
    }

    public static void MoveActionBefore(UBAction action, UBAction before)
    {
        if (action == null || before == null) return;
        var sheet = action.ActionSheet;
        var oldIndex = sheet.Actions.IndexOf(action);
        var newIndex = sheet.Actions.IndexOf(before);
        MoveAction(action.ActionSheet,oldIndex,newIndex);
        sheet.Save(action.RootContainer);
        EditorUtility.SetDirty(action.RootContainer as UnityEngine.Object);
    }

    public static void MoveActionTo(UBAction action, UBActionSheet target, bool save = true)
    {
        if (save)
        {
            Undo.RecordObject(action.RootContainer as UnityEngine.Object, "Move Action");
        }
        var sourceSheet = action.ActionSheet;
        if (sourceSheet == null) return;
        sourceSheet.Actions.Remove(action);

        target.Actions.Add(action);
        action.ActionSheet = target;

        if (save == true)
        {
            target.Save();
            sourceSheet.Save();
            EditorUtility.SetDirty(target.RootContainer as UnityEngine.Object);
        }
    }

    public static void PasteAction(UBActionSheet sheet)
    {
        PasteAction(ClipboardAction, sheet);
    }

    public static void PasteAction(UBAction action, UBActionSheet sheet)
    {
        Undo.RecordObject(sheet.RootContainer as UnityEngine.Object, "Paste Action");
        var actionCopy = CopyAction(action);
        actionCopy.RootContainer = sheet.RootContainer;
        actionCopy.ActionSheet = sheet;
        sheet.Actions.Add(actionCopy);
        EditorUtility.SetDirty(sheet.RootContainer as UnityEngine.Object);
    }

    public static void RemoveAction(IUBehaviours behaviour, UBActionSheet sheet, UBAction action)
    {
        Undo.RecordObject(sheet.RootContainer as UnityEngine.Object, "Remove " + action.Name);
        var sheets = action.GetAvailableActionSheets(behaviour).Select(p => p.Sheet).ToArray();
        behaviour.Sheets.RemoveAll(p => sheets.Contains(p));
        sheet.Actions.Remove(action);
        sheet.Save(behaviour);
        sheet.Load(behaviour, sheet.TriggerInfo);
        EditorUtility.SetDirty(behaviour as UnityEngine.Object);
    }

    public static void SheetToCustomTrigger(UBActionSheet sheet)
    {
        Undo.RecordObject(sheet.RootContainer as UnityEngine.Object, "Sheet to Custom Trigger");
        var root = sheet.RootContainer;
        var targetSheet = root.CreateSheet(sheet.Name);
        var items = sheet.Items.ToArray();
        foreach (var action in items)
        {
            MoveActionTo(action, targetSheet, false);
        }

        var trigger = new TriggerInfo();
        trigger.DisplayName = sheet.Name;
        trigger.Data = sheet.Name;
        trigger.TriggerTypeName = typeof(UBCustomTrigger).AssemblyQualifiedName;
        trigger.Sheet = targetSheet;
        root.Triggers.Add(trigger);
        targetSheet.Save();
        sheet.ForwardTo = trigger;
        sheet.Save();
        EditorUtility.SetDirty(sheet.RootContainer as UnityEngine.Object);
    }

    public static IEnumerable<MemberInfo> TypeMemberSearch(string searchText)
    {
        if (CachedMemberInfos == null)
        {
            CachedMemberInfos = new List<MemberInfo>();
            var allowed = new string[]
            {
                "System.Core",
                "mscorlib",
                "UBehaviours",
                "Assembly-CSharp",
                "UnityEngine"
            };
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var item in assemblies)
            {
                if (!allowed.Any(p => item.FullName.Contains(p)))
                    continue;

                var types = item.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsSubclassOf(typeof(UBAction))) continue;

                    var methods = TypeActionsGenerator.GetApplicableMethods(type);
                    var properties = TypeActionsGenerator.GetApplicableProperties(type);
                    foreach (var method in methods)
                        CachedMemberInfos.Add(method);
                    foreach (var propertyInfo in properties)
                        CachedMemberInfos.Add(propertyInfo);
                }
            }
        }
        //var reg = new Regex(searchText, RegexOptions.IgnoreCase);

        foreach (var cachedMemberInfo in CachedMemberInfos)
        {
            if (cachedMemberInfo.DeclaringType != null)
            {
                var search = string.Format("{0}.{1}", cachedMemberInfo.DeclaringType.Name.ToUpper(), cachedMemberInfo.Name.ToUpper());
                if (search.Contains(searchText.ToUpper()))
                {
                    yield return cachedMemberInfo;
                }
            }
        }
    }

    public static string ValidateInstanceTemplate(SerializedProperty instanceTemplatesArray, string s)
    {
        for (var i = 0; i < instanceTemplatesArray.arraySize; i++)
        {
            if (s == instanceTemplatesArray.GetArrayElementAtIndex(i).stringValue)
            {
                return "This template trigger already exists.";
            }
        }
        return null;
    }

    public void CopyToOther(SerializedProperty sourceDeclaresArray, SerializedProperty targetDeclaresArray)
    {
        var inspectorFields = InspectorFields(sourceDeclaresArray).ToArray();
        targetDeclaresArray.arraySize = inspectorFields.Length;

        for (var i = 0; i < inspectorFields.Length; i++)
        {
            var sourceItem = sourceDeclaresArray.GetArrayElementAtIndex(inspectorFields[i].Key);
            var targetItem = targetDeclaresArray.GetArrayElementAtIndex(i);

            var copyDefault = string.IsNullOrEmpty(targetItem.FindPropertyRelative("_GUID").stringValue);

            if (copyDefault)
            {
                targetItem.serializedObject.CopyFromSerializedProperty(sourceItem);
            }
            else
            {
                targetItem.FindPropertyRelative("_varType").enumValueIndex = sourceItem.FindPropertyRelative("_varType").enumValueIndex;
                targetItem.FindPropertyRelative("_GUID").stringValue = sourceItem.FindPropertyRelative("_GUID").stringValue;
                targetItem.FindPropertyRelative("_name").stringValue = sourceItem.FindPropertyRelative("_name").stringValue;
                targetItem.FindPropertyRelative("_objectValueType").stringValue = sourceItem.FindPropertyRelative("_objectValueType").stringValue;
                targetItem.FindPropertyRelative("_enumType").stringValue = sourceItem.FindPropertyRelative("_enumType").stringValue;
            }
        }
    }

    public IEnumerable<KeyValuePair<int, SerializedProperty>> InspectorFields(SerializedProperty sourceDeclaresArray)
    {
        for (var i = 0; i < sourceDeclaresArray.arraySize; i++)
        {
            var item = sourceDeclaresArray.GetArrayElementAtIndex(i);
            if (item.FindPropertyRelative("_Expose").boolValue)
                yield return new KeyValuePair<int, SerializedProperty>(i, item);
        }
    }
}