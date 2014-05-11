//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(UBehaviours), true)]
//public class UBehavioursEditor : UBEditor
//{
//    private UBAction _selected;

//    private Stack<UBActionSheet> _sheetStack = new Stack<UBActionSheet>();

//    private bool _TriggersExpanded = true;

//    public static UBehaviours CurrentBehaviour { get; set; }

//    public static UBehavioursInstanceEditor InstanceEditor { get; set; }

//    public static UBInstanceBehaviour InstanceTarget
//    {
//        get
//        {
//            if (InstanceEditor == null) return null;
//            return InstanceEditor.Target;
//        }
//    }

//    public string AssetPath
//    {
//        get
//        {
//            return AssetDatabase.GetAssetPath(target);
//        }
//    }

//    public UBActionSheet CurrentActionSheet
//    {
//        get
//        {
//            if (SheetStack.Count < 1) return null;
//            return SheetStack.Peek() as UBActionSheet;
//        }
//    }

//    public bool InstanceTemplatesOpen
//    {
//        get { return EditorPrefs.GetBool("UBInstanceTemplatesOpen"); }
//        set { EditorPrefs.SetBool("UBInstanceTemplatesOpen", value); }
//    }

//    public bool InstanceTriggersOpen
//    {
//        get { return EditorPrefs.GetBool("UBInstanceTriggersOpen"); }
//        set { EditorPrefs.SetBool("UBInstanceTriggersOpen", value); }
//    }

//    public UBehavioursEditor ParentEditor { get; set; }

//    public UBehavioursEditor RootEditor
//    {
//        get
//        {
//            var editor = this;
//            while (editor != null)
//            {
//                if (editor.ParentEditor == null)
//                {
//                    return editor;
//                }
//                editor = editor.ParentEditor;
//            }
//            return this;
//        }
//    }

//    public Stack<UBActionSheet> SheetStack
//    {
//        get { return _sheetStack; }
//        set { _sheetStack = value; }
//    }

//    public UBehaviours Target
//    {
//        get { return target as UBehaviours; }
//    }

//    public bool TriggersOpen
//    {
//        get { return EditorPrefs.GetBool("UBTriggersExpanded"); }
//        set { EditorPrefs.SetBool("UBTriggersExpanded", value); }
//    }

//    public bool VariablesActive
//    {
//        get { return EditorPrefs.GetBool("UBVariablesActive"); }
//        set { EditorPrefs.SetBool("UBVariablesActive", value); }
//    }

//    public static IEnumerable<IUBVariableDeclare> GetContextVars(UBAction action, Type typeFilter)
//    {
//        if (action.ActionSheet.IsInstance)
//        {
//            if (InstanceTarget != null)
//            {
//                var instanceVars = InstanceTarget.GetInstanceVariables();

//                foreach (var declare in instanceVars)
//                {
//                    if (!declare.ValueType.IsAssignableFrom(typeFilter)) continue;
//                    yield return declare;
//                }
//            }
//        }

//        var vars = action.ActionSheet.RootEventContainer.GetAllVariableDeclares().ToArray();
//        foreach (var declare in vars)
//        {
//            if (typeFilter != null && !typeFilter.IsAssignableFrom(declare.ValueType)) continue;
//            yield return declare;
//        }
//    }

//    public static string PrettyLabel(string label)
//    {
//        return Regex.Replace(label.Replace("_", ""), @"((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1");
//    }

//    public void Forward(UBActionSheet sheet, bool isInstance)
//    {
//        if (sheet != null)
//        {
//            sheet.IsInstance = isInstance;
//        }
//        RootEditor.SheetStack.Push(sheet);
//    }

//    public string GetContextLabel()
//    {
//        return "";
//    }

//    public void NavigateToAction(UBAction action)
//    {
//        var path = action.GetPath();
//        if (path.Count < 1) return;
//        SheetStack.Clear();
//        foreach (var p in path)
//            Forward(p, path[0].IsInstance);
//    }

//    public void OnEnable()
//    {
//        this.Repaint();
//    }

//    public override void OnInspectorGUI()
//    {
//        if (serializedObject.targetObject == null) return;
//        serializedObject.Update();

//        //EditorGUILayout.BeginVertical();

//        DoEditMode();
//        DoSettingsButton();
//        serializedObject.ApplyModifiedProperties();
//    }

//    public void UBEnumOptions(SerializedProperty property)
//    {
//        var enumTypeProperty = property.FindPropertyRelative("_enumType");
//        GUILayout.BeginHorizontal();
//        var type = string.IsNullOrEmpty(enumTypeProperty.stringValue)
//            ? typeof(AnimationPlayMode)
//            : Type.GetType(enumTypeProperty.stringValue);

//        if (GUILayout.Button(type.Name))
//        {
//            UBTypesWindow.Init("Object Type", ActionSheetHelpers.GetEnumTypes(),
//           (prettyName, qualifiedName) =>
//           {
//               enumTypeProperty.serializedObject.Update();
//               enumTypeProperty.stringValue = qualifiedName;
//               enumTypeProperty.serializedObject.ApplyModifiedProperties();
//           });
//        }
//        GUILayout.EndHorizontal();
//    }

//    public void UBObjectOptions(SerializedProperty property)
//    {
//        var typeProperty = property.FindPropertyRelative("_objectValueType");

//        GUILayout.BeginHorizontal("Object Type:");
//        var type = string.IsNullOrEmpty(typeProperty.stringValue)
//            ? typeof(UnityEngine.Object)
//            : Type.GetType(typeProperty.stringValue);

//        if (GUILayout.Button(type.Name))
//        {
//            UBTypesWindow.Init("Object Type", ActionSheetHelpers.GetDerivedTypes<UnityEngine.Object>(false, true),
//           (prettyName, qualifiedName) =>
//           {
//               typeProperty.serializedObject.Update();
//               typeProperty.stringValue = qualifiedName;
//               typeProperty.serializedObject.ApplyModifiedProperties();
//           });
//        }
//        GUILayout.EndHorizontal();
//    }

//    private void AddBehaviourTrigger(string name, string triggerType)
//    {
//        var actionSheet = Target.CreateActionSheet(name);
//        actionSheet.TriggerType = triggerType;
//        actionSheet.IsCustom = typeof(UBCustomTrigger).AssemblyQualifiedName == triggerType;
//        //AssetDatabase.AddObjectToAsset(actionSheet, AssetPath);
//        Target.ActionSheets.Add(actionSheet);
//    }

//    private void AddInstanceTrigger(SerializedProperty triggers, string name, string triggerType)
//    {
//        triggers.serializedObject.Update();

//        triggers.InsertArrayElementAtIndex(0);
//        var property = triggers.GetArrayElementAtIndex(0);
//        property.FindPropertyRelative("_data").stringValue = name;
//        property.FindPropertyRelative("_displayName").stringValue = name;
//        property.FindPropertyRelative("_triggerTypeName").stringValue = triggerType;
//        var sheet = this.Target.CreateActionSheet(name);
//        sheet.RootEventContainer = Target;
//        sheet.IsInstance = true;
//        sheet.IsCustom = typeof(UBCustomTrigger).AssemblyQualifiedName == triggerType;
//        //AssetDatabase.AddObjectToAsset(sheet, AssetPath);
//        var sheetProperty = property.FindPropertyRelative("_sheet");
//        //sheetProperty.objectReferenceValue = sheet;
//        triggers.serializedObject.ApplyModifiedProperties();
//    }

//    private void DebugToolbar()
//    {
//        var rect = GetRect(UBStyles.ToolbarStyle);
//        GUI.Box(rect, string.Empty, UBStyles.ToolbarStyle);

//        var half = rect.width / 2f;
//        if (GUI.Button(new Rect(half - 17f, rect.y + 3, 32f, 25f), string.Empty, UBStyles.ContinueButtonStyle))
//        {
//            InstanceTarget.Context.DebugInfo.CurrentBreakPoint = null;
//            InstanceTarget.Context.DebugInfo.IsStepping = false;
//            EditorApplication.ExecuteMenuItem("Edit/Pause");
//        }
//        if (GUI.Button(new Rect(half + 16f, rect.y + 3, 32f, 25f), string.Empty, UBStyles.NextButtonStyle))
//        {
//            InstanceTarget.Context.DebugInfo.CurrentBreakPoint = null;
//            InstanceTarget.Context.DebugInfo.IsStepping = true;
//            EditorApplication.ExecuteMenuItem("Edit/Pause");
//        }
//    }

//    //private void DoBehaviourTriggers(Dictionary<string, UBActionSheet> actionSheets)
//    //{
//    //    if (TriggersOpen)
//    //    {
//    //        foreach (var nameSheetPair in actionSheets)
//    //        {
//    //            var sp = nameSheetPair.Value;
//    //            if (_TriggersExpanded || sp!= null && sp.IsVisibleWhenCollapsed)
//    //                DoTriggerButton( nameSheetPair.Key, sp, actionSheets);
//    //        }
//    //    }
//    //}

//    private void DoEditMode()
//    {
//        if (ParentEditor == null)
//        {
//            DoToolbar();
//            if (InstanceTarget != null && InstanceTarget.Context != null && InstanceTarget.Context.DebugInfo.IsDebugging)
//            {
//                DebugToolbar();
//            }
//        }
//        if (_sheetStack.Count == 0)
//        {
//            if (Target != null)
//            {
//                var actionSheets = Target.GetAvailableActionSheets(serializedObject);
//                //DoBehaviourTriggers(actionSheets);
//                if (InstanceTarget == null)
//                {
//                    var triggerTemplates = serializedObject.FindProperty("_triggerTemplates");
//                    DoInstanceTemplates(triggerTemplates);
//                }
//                DoInstanceTriggers(actionSheets);
//            }
//        }
//        else
//        {
//            DoActionSheet(this, SheetStack.Peek());
//        }

//        DoVariablesToolbar("Variables", VariablesActive);

//        if (VariablesActive)
//        {
//        }
//    }

//    private void DoInstanceTemplates(SerializedProperty instanceTemplatesArray)
//    {
//        if (DoToolbar("Instance Templates", InstanceTemplatesOpen, () => UBInputDialog.Init("Instance Template Name",
//            (templateName) =>
//            {
//                instanceTemplatesArray.serializedObject.Update();
//                var item = instanceTemplatesArray.GetArrayElementAtIndex(instanceTemplatesArray.arraySize++);
//                item.stringValue = templateName;
//                instanceTemplatesArray.serializedObject.ApplyModifiedProperties();
//            })))
//        {
//            InstanceTemplatesOpen = !InstanceTemplatesOpen;
//        }

//        for (var i = 0; i < instanceTemplatesArray.arraySize; i++)
//        {
//            var template = instanceTemplatesArray.GetArrayElementAtIndex(i);
//            DoTriggerButton(template.stringValue, UBStyles.CommandBarButtonStyle, GUIStyle.none);
//        }
//    }

//    private void DoInstanceTriggers(Dictionary<string, SerializedProperty> actionSheets)
//    {
//        if (InstanceEditor == null)
//        {
//            return;
//        }
//        var currentTriggers = InstanceEditor.serializedObject.FindProperty("_instanceTriggers");
//        if (DoToolbar(InstanceTarget.gameObject.name + " Triggers", InstanceTriggersOpen,
//            () => { UBTriggersWindow.Init((n, tt) => AddInstanceTrigger(currentTriggers, n, tt), true); }))
//        {
//            InstanceTriggersOpen = !InstanceTriggersOpen;
//        }
//        if (!InstanceTriggersOpen) return;

//        for (var i = 0; i < currentTriggers.arraySize; i++)
//        {
//            var trigger = currentTriggers.GetArrayElementAtIndex(i);
//            var sheetName = trigger.FindPropertyRelative("_displayName").stringValue;
//            var sheetProperty = trigger.FindPropertyRelative("_sheet");
//            int i1 = i;
//            DoTriggerButton(this, sheetName, sheetProperty, actionSheets, true, true,
//                (sp) =>
//                {
                  
//                    InstanceEditor.serializedObject.Update();
//                    var triggers = InstanceEditor.serializedObject.FindProperty("_instanceTriggers");
//                    triggers.DeleteArrayElementAtIndex(i1);
//                    InstanceEditor.serializedObject.ApplyModifiedProperties();
//                });
//        }
//        var instanceTriggers = InstanceTarget.GetAvailableInstanceTriggers().ToArray();
//        foreach (var instanceTrigger in instanceTriggers)
//        {
//            if (DoTriggerButton(instanceTrigger.DisplayName, UBStyles.CommandBarButtonStyle, indicatorStyle: UBStyles.InstanceTriggerBackgroundStyle))
//            {
//                currentTriggers.InsertArrayElementAtIndex(currentTriggers.arraySize);
//                var property = currentTriggers.GetArrayElementAtIndex(currentTriggers.arraySize - 1);
//                property.FindPropertyRelative("_data").stringValue = instanceTrigger.Data;
//                property.FindPropertyRelative("_displayName").stringValue = instanceTrigger.DisplayName;
//                property.FindPropertyRelative("_triggerTypeName").stringValue = instanceTrigger.TriggerTypeName;
//                var sheet = this.Target.CreateActionSheet(instanceTrigger.Data.ToString());
//                sheet.RootEventContainer = Target;
//                sheet.IsInstance = true;
//                AssetDatabase.AddObjectToAsset(sheet, AssetPath);
//                var sheetProperty = property.FindPropertyRelative("_sheet");
//                sheetProperty.objectReferenceValue = sheet;
//                Forward(sheetProperty.objectReferenceValue as UBActionSheet, true);
//            }
//        }
//    }

//    private void DoSettingsButton()
//    {
//        if (GUI.Button(GetRect(UBStyles.ButtonStyle), "Generate Code", UBStyles.ButtonStyle))
//        {
//            IBehaviourVisitable visitable = InstanceTarget == null ? (IBehaviourVisitable)Target : (IBehaviourVisitable)InstanceTarget;
//            var generator = new UBehaviourCSharpGenerator(visitable);//, "MyBehaviour", "public class {0} : MonoBehaviour");
//            Debug.Log(generator.ToString());
//        }

//        return;
//        //if (SheetStack.Count > 0) return;
//        //var rect = GetRect(UBStyles.ToolbarStyle);
//        //GUI.Box(rect, "", UBStyles.ToolbarStyle);
//        //var labelStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 12 };
//        //var labelRect = new Rect(rect.x + 25, rect.y, rect.width - 50, rect.height);
//        //if (GUI.Button(labelRect, new GUIContent("Settings", _settingsActive ? UBStyles.ArrowUpTexture : UBStyles.ArrowDownTexture), labelStyle))
//        //{
//        //    _settingsActive = !_settingsActive;
//        //}
//        //if (_settingsActive)
//        //{
//        //    EditorGUILayout.Space();
//        //    DrawDefaultInspector();
//        //}
//    }

//    private void DoToolbar()
//    {
//        if (SheetStack.Count > 0)
//        {
//            var trailItems = SheetStack.Reverse().Take(3).Select(
//                p => PrettyLabel(p.Name).Replace(" ", "")).ToArray();

//            var trailText = (SheetStack.Count > 2 ? "..." : "") + string.Join(":", trailItems);

//            DoToolbar(trailText, () =>
//            {
//                UBActionsWindow.Init(SheetStack.Peek(), AssetPath);
//            }, GoBack, UBActionEditor.Clipboard == null ? (Action)null : () =>
//            {
//                var copyItem = UBActionEditor.Clipboard;
//                var copiedItem = ScriptableObject.CreateInstance(copyItem.GetType()) as UBAction;
//                var actionSheet = SheetStack.Peek();
//                copiedItem.ActionSheet = actionSheet;
//                copiedItem.RootEventContainer = actionSheet.RootEventContainer;
//                EditorUtility.CopySerialized(copyItem, copiedItem);
//                actionSheet.Actions.Add(copiedItem as UBAction);
//            });
//        }
//        else
//        {
//            if (DoToolbar(Target.name + " Triggers", TriggersOpen, () =>
//            {
//                UBTriggersWindow.Init(AddBehaviourTrigger);
//            }))
//            {
//                TriggersOpen = !TriggersOpen;
//            }
//        }
//    }

//    private void DoVariables()
//    {
//        var declaresProperty = serializedObject.FindProperty("_declares");

//        for (var i = 0; i < declaresProperty.arraySize; i++)
//        {
//            var rect = GetRect(UBStyles.CommandBarClosedStyleSmall);

//            var elementProperty = declaresProperty.GetArrayElementAtIndex(i);
//            var varTypeProperty = elementProperty.FindPropertyRelative("_varType");
//            var nameProperty = elementProperty.FindPropertyRelative("_name");
//            var expanded = elementProperty.FindPropertyRelative("_Expanded");

//            GUI.Box(rect, "", expanded.boolValue ? UBStyles.CommandBarOpenStyleSmall : UBStyles.CommandBarClosedStyleSmall);
//            var expandRect = new Rect(rect.x + 5, rect.y + 3, 16, 16);
//            expanded.boolValue = GUI.Toggle(expandRect, expanded.boolValue, "", expanded.boolValue
//                ? UBStyles.FoldoutCloseButtonStyle
//                : UBStyles.FoldoutOpenButtonStyle);

//            var propertyRect = new Rect(rect);
//            propertyRect.x += 23f;
//            propertyRect.y += 4f;
//            propertyRect.width -= 56f;
//            propertyRect.height -= 9f;
//            VariableDeclareDrawer.DoDeclareField(propertyRect, elementProperty, true);

//            if (expanded.boolValue)
//            {
//                var exposeField = elementProperty.FindPropertyRelative("_Expose");
//                var newValue = EditorGUILayout.TextField("Name", nameProperty.stringValue);
//                if (newValue != nameProperty.stringValue)
//                {
//                    nameProperty.stringValue = Regex.Replace(newValue, @"[^a-zA-Z0-9]", String.Empty);
//                }

//                EditorGUILayout.PropertyField(varTypeProperty);

//                EditorGUILayout.PropertyField(exposeField, new GUIContent("Allow Overrides"));

//                var methodInfo = this.GetType().GetMethod(string.Format("UB{0}Options", varTypeProperty.enumNames[varTypeProperty.enumValueIndex]));
//                if (methodInfo != null)
//                {
//                    methodInfo.Invoke(this, new object[] { elementProperty });
//                }
//            }

//            if (GUI.Button(new Rect(rect.width - 22, rect.y + 5, 18, 18), string.Empty, UBStyles.RemoveButtonStyle))
//            {
//                declaresProperty.DeleteArrayElementAtIndex(i);
//            }
//        }
//        IEnumerable<IUBVariableDeclare> lockedDeclares = Target.GetStaticVariableDeclares();
//        if (InstanceTarget != null)
//        {
//            lockedDeclares = lockedDeclares.Concat(InstanceTarget.GetInstanceVariables());
//        }

//        foreach (var lockedDeclare in lockedDeclares)
//        {
//            var rect = GetRect(UBStyles.CommandBarClosedStyleSmall);

//            GUI.Box(rect, string.Empty, UBStyles.CommandBarClosedStyleSmall);

//            GUI.Label(new Rect(rect.x + 22, rect.y + 4, rect.width / 2, rect.height), lockedDeclare.Name.ToString());
//            GUI.Label(new Rect(rect.x + (rect.width / 2) + 5, rect.y + 4, rect.width / 2, rect.height), lockedDeclare.DefaultValue == null ? "null" : lockedDeclare.DefaultValue.ToString());

//            //GUI.Box(rect, "",);
//        }
//    }

//    private void DoVariablesToolbar(string label, bool up = false)
//    {
//        if (EditorApplication.isPlaying || EditorApplication.isPaused) return;
//        //if (Application.isPlaying)
//        //{
//        //    var ctx = target as UBEventContainer;
//        //    foreach (var variable in ctx.Variables)
//        //    {
//        //        EditorGUILayout.LabelField(variable.Name, variable.LiteralObjectValue == null ? "NULL" : variable.LiteralObjectValue.ToString());
//        //    }
//        //}
//        //else
//        //{
//        var variablesClicked = DoToolbar("Variables", VariablesActive, () =>
//        {
//            var declaresProperty = serializedObject.FindProperty("_declares");

//            declaresProperty.InsertArrayElementAtIndex(declaresProperty.arraySize);
//            var element = declaresProperty.GetArrayElementAtIndex(declaresProperty.arraySize - 1);
//            var guidProperty = element.FindPropertyRelative("_GUID");
//            var nameProperty = element.FindPropertyRelative("_name");
//            nameProperty.stringValue = "NewVariable";
//            guidProperty.stringValue = Guid.NewGuid().ToString();
//        });

//        if (variablesClicked)
//        {
//            VariablesActive = !VariablesActive;
//        }
//        if (VariablesActive)
//        {
//            DoVariables();
//        }
//        //}
//    }

//    private void GoBack()
//    {
//        SheetStack.Pop();
//    }
//}