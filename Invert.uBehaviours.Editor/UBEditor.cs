using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UBehaviours.Actions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class UBEditor : Editor
{
    public static UBActionSheet _CurrentlyOverSheet;
    private static UBAction _CurrentlyOverAction;
    private static bool _CurrentlyOverActionInsertAfter = false;
    private static int _indent;
    protected IGrouping<string, Type>[] _actionGroups;
    private MemberInfo[] _members;
    private string _SearchText = "";
    private Vector2 _scrollPosition;


    public static DebugInfo CurrentDebugInfo
    {
        get
        {
            var debugHandler = UBActionSheet.ExecutionHandler as DebugExecutionHandler;
            if (debugHandler != null && debugHandler.DebugInfo != null)
            {
                return debugHandler.DebugInfo;
            }
            return null;
        }
    }

    public static UBActionContent DraggingAction { get; set; }

    public static int Indent
    {
        get { return _indent; }
        set
        {
            _indent = value;
            EditorGUI.indentLevel = _indent;
        }
    }


    public static bool IsGlobals { get; set; }
    public static bool MouseDown { get; set; }

    public string SearchText
    {
        get { return EditorPrefs.GetString("UBActionsSearchText", string.Empty); }
        set { EditorPrefs.SetString("UBActionsSearchText", value); }
    }

    public static void DebugToolbar()
    {
        if (CurrentDebugInfo == null) return;

        var rect = GetRect(UBStyles.ToolbarStyle);
        GUI.Box(rect, string.Empty, UBStyles.ToolbarStyle);

        var half = rect.width / 2f;
        if (GUI.Button(new Rect(half - 17f, rect.y, 32f, 25f), string.Empty, UBStyles.ContinueButtonStyle))
        {
            CurrentDebugInfo.CurrentBreakPoint = null;
            CurrentDebugInfo.IsStepping = false;
            EditorApplication.ExecuteMenuItem("Edit/Pause");
        }
        if (GUI.Button(new Rect(half + 16f, rect.y, 32f, 25f), string.Empty, UBStyles.NextButtonStyle))
        {
            CurrentDebugInfo.CurrentBreakPoint = null;
            CurrentDebugInfo.IsStepping = true;
            EditorApplication.ExecuteMenuItem("Edit/Pause");
        }
    }

    public static bool DoActionButton(UBActionContent buttonInfo, bool isLast = false)
    {
        var rect = GetRect(buttonInfo.BackgroundStyle, GUILayout.Height(20));

        var evt = Event.current;
        //        Debug.Log(evt.type);
        if (buttonInfo.AllowDrag && rect.Contains(evt.mousePosition))
        {
            switch (evt.type)
            {
                case EventType.MouseDown:

                    break;

                case EventType.MouseDrag:
                    // Clear out drag data
                    DragAndDrop.PrepareStartDrag();
                    // Set up what we want to drag
                    DragAndDrop.SetGenericData(typeof(UBActionContent).Name, buttonInfo);
                    DragAndDrop.paths = new[] { typeof(UBActionContent).Name };
                    // Start the actual drag
                    DragAndDrop.StartDrag("Dragging title");
                    // Make sure no one uses the event after us
                    Event.current.Use();
                    break;
            }
        }

        GUI.Box(rect, "", buttonInfo.BackgroundStyle);
        var dropRect = new Rect(rect);
        // dropRect.y -= rect.height / 2f;
        if (DropAreaGUI<UBActionContent, UBAction>(dropRect, buttonInfo.Action, (source, target) => UBUtils.MoveActionBefore(source.Action, target)))
        {
            _CurrentlyOverActionInsertAfter = false;
            _CurrentlyOverAction = buttonInfo.Action;
        }

        if (buttonInfo.IndicatorStyle != null)
        {
            var rectIndicator = new Rect(rect);
            rectIndicator.width = 2;
            //rectIndicator.y += 1;
            rectIndicator.x += 2;
            rectIndicator.height -= 1;
            GUI.Box(rectIndicator, "", buttonInfo.IndicatorStyle);
        }
        if (buttonInfo.OptionsStyle != null)
        {
            var eventOptionsButtonRect = new Rect(rect.x + 5, rect.y + ((rect.height / 2) - 9), 16, 16);
            if (GUI.Button(eventOptionsButtonRect, "", buttonInfo.OptionsStyle))
            {
                buttonInfo.OnShowOptions(buttonInfo);
            }
            //var seperatorRect = new Rect(rect);
            //seperatorRect.width = 3;
            //seperatorRect.y += 1;
            //seperatorRect.height -= 2;
            //seperatorRect.x = eventOptionsButtonRect.x + 20;
            //GUI.Box(seperatorRect, string.Empty, UBStyles.SeperatorStyle);
        }
        var labelStyle = new GUIStyle(EditorStyles.label) { alignment = buttonInfo.Anchor, fontSize = 10 };
        var labelRect = new Rect(rect.x + 26, rect.y + ((rect.height / 2) - 11), rect.width - 40, rect.height);
        var lbl = buttonInfo.DisplayText;
        var result = GUI.Button(labelRect, lbl, labelStyle);

        //if (buttonInfo.OnMoveDown != null)
        //{
        //    var moveDown = new Rect(rect.x + rect.width - 60, rect.y + ((rect.height / 2) - 8), 16, 16);
        //    if (GUI.Button(moveDown, "", UBStyles.ArrowDownButtonStyle))
        //    {
        //        buttonInfo.OnMoveDown();
        //    }
        //}
        //if (buttonInfo.OnMoveUp != null)
        //{
        //    var moveUpRect = new Rect(rect.x + rect.width - 40, rect.y + ((rect.height / 2) - 8), 16, 16);
        //    if (GUI.Button(moveUpRect, "", UBStyles.ArrowUpButtonStyle))
        //    {
        //        buttonInfo.OnMoveUp();
        //    }
        //}
        if (buttonInfo.OnRemove != null)
        {
            var removeRect = new Rect(rect.x + rect.width - 16, rect.y + ((rect.height / 2) - 8), 16, 16);
            if (GUI.Button(removeRect, "", UBStyles.RemoveButtonStyle))
            {
                buttonInfo.OnRemove();
            }
            var seperatorRect = new Rect(rect);
            seperatorRect.width = 3;
            seperatorRect.y += 2;
            seperatorRect.height -= 5;
            seperatorRect.x = removeRect.x - 3;
            GUI.Box(seperatorRect, string.Empty, UBStyles.SeperatorStyle);
        }
        if (isLast)
        {
            var lastRect = new Rect(rect);
            lastRect.y += rect.height;
            if (DropAreaGUI<UBActionContent, UBAction>(lastRect, buttonInfo.Action,
                (source, target) => UBUtils.MoveActionAfter(source.Action, target)))
            {
                _CurrentlyOverAction = buttonInfo.Action;
                _CurrentlyOverActionInsertAfter = true;
            }
            else
            {
            }
        }

        return result;
    }

    public static void DoActionSheet(UBActionContent[] actions, Action<UBActionSheet> onShowOptions)
    {
        foreach (var actionContent in actions)
        {
            if (!_CurrentlyOverActionInsertAfter && _CurrentlyOverAction == actionContent.Action)
            {
                var rectBefore = GetRect(UBStyles.CurrentActionBackgroundStyle, GUILayout.Height(3));
                GUI.Box(rectBefore, string.Empty, UBStyles.CurrentActionBackgroundStyle);
            }

            //UBActionContent content1 = actionContent;
            if (DoActionButton(actionContent, actionContent == actions.Last()))
            {
                actionContent.Action.Expanded = !actionContent.Action.Expanded;
                actionContent.Sheet.Save(actionContent.UndoTarget as IUBehaviours);
                EditorUtility.SetDirty(actionContent.UndoTarget);
            }

            if (actionContent.Action.Expanded && actionContent.Action.Enabled)
            {
                if (DoActionGUI(actionContent.UndoTarget, actionContent.Action))
                {
                    actionContent.Save();
                }
                actionContent.OnSubContents(actionContent.Action);
            }
            if (_CurrentlyOverActionInsertAfter && _CurrentlyOverAction == actionContent.Action)
            {
                var rectBefore = GetRect(UBStyles.CurrentActionBackgroundStyle, GUILayout.Height(3));
                GUI.Box(rectBefore, string.Empty, UBStyles.CurrentActionBackgroundStyle);
            }
        }
    }

    public static void DoErrors(IBehaviourNotification[] notifications, Action<IBehaviourNotification> navigateToAction)
    {
        if (notifications.Length < 1) return;

        var errors = notifications.Where(p => p is BehaviourError).ToArray();
        var breakpoints = notifications.Where(p => p is BehaviourBreakpoint).ToArray();
        if (breakpoints.Length > 0)
        {
            DoToolbar(string.Format("Breakpoints ({0})", breakpoints.Length));
            DoNotifications(breakpoints, navigateToAction, UBStyles.CommandBarClosedStyle);
        }
        if (errors.Length > 0)
        {
            DoToolbar(string.Format("Errors ({0})", errors.Length));
            DoNotifications(errors, navigateToAction, UBStyles.DebugBackgroundStyle);

        }


    }

    public static void DoNotifications(IBehaviourNotification[] notifications, Action<IBehaviourNotification> navigateToAction, GUIStyle backgroundStyle)
    {
        foreach (var error in notifications)
        {
            //var rect = GetRect(backgroundStyle, GUILayout.Height(25));
            if (DoActionButton(new UBActionContent()
            {
                OnRemove = error.Remove,
                BackgroundStyle = backgroundStyle,
                DisplayText = error.Message,
                OptionsStyle = UBStyles.BreakpointButtonStyle
            }))
            {
                var action = error.Source as UBAction;
                if (action == null || navigateToAction == null) return;
                navigateToAction(error);
            }

            //if (GUI.Button(rect, error.Message, backgroundStyle))
            //    //new UBTriggerContent(error.Message, UBStyles.DebugBackgroundStyle))))
            //{
            //    var action = error.Source as UBAction;
            //    if (action == null || navigateToAction == null) return;
            //    navigateToAction(action);
            //}
            //if (error.Remove != null)
            //{
            //    var removeButtonRect = new Rect(rect);
            //    removeButtonRect.x = rect.width - 25;
            //    removeButtonRect.width = 20;
            //    removeButtonRect.height = 20;
            //    removeButtonRect.y += (rect.height/2) - 10;

            //    if (GUI.Button(removeButtonRect, string.Empty, UBStyles.RemoveButtonStyle))
            //    {
            //        error.Remove();
            //        var action = error.Source as UBAction;
            //        action.ActionSheet.Save();
            //        EditorUtility.SetDirty(action.RootContainer as UnityEngine.Object);
            //    }
            //}
        }
    }

    public static void DoSubHeader(string label)
    {
        var rect = GetRect(UBStyles.SubHeaderStyle);
        GUI.Box(rect, label, UBStyles.SubHeaderStyle);
    }

    public static bool DoToolbar(string label, bool open, Action add = null, Action leftButton = null, Action paste = null, GUIStyle addButtonStyle = null, GUIStyle pasteButtonStyle = null)
    {
        var rect = GetRect(UBStyles.ToolbarStyle);
        GUI.Box(rect, "", UBStyles.ToolbarStyle);
        var labelStyle = new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 10
        };
        var labelRect = new Rect(rect.x + 2, rect.y + (rect.height / 2) - 8, rect.width - 50, 16);
        var result = open;
        if (leftButton == null)
        {
            result = GUI.Button(labelRect,
                new GUIContent(label,
                    open ? UBStyles.ArrowDownTexture : UBStyles.CollapseRightArrowTexture),
                labelStyle);
        }
        else
        {
            if (GUI.Button(labelRect, new GUIContent(label, UBStyles.ArrowLeftTexture), labelStyle))
            {
                leftButton();
            }
        }

        if (paste != null)
        {
            var addButtonRect = new Rect(rect.x + rect.width - 42, rect.y + (rect.height / 2) - 8, 16, 16);
            if (GUI.Button(addButtonRect, "", pasteButtonStyle ?? UBStyles.PasteButtonStyle))
            {
                paste();
            }
        }

        if (add != null)
        {
            var addButtonRect = new Rect(rect.x + rect.width - 21, rect.y + (rect.height / 2) - 8, 16, 16);
            if (GUI.Button(addButtonRect, "", addButtonStyle ?? UBStyles.AddButtonStyle))
            {
                add();
            }
        }
        return result;
    }

    public static bool DoToolbar(string label, Action add = null, Action leftButton = null, Action paste = null)
    {
        return DoToolbar(label, true, add, leftButton, paste);
    }

    public static bool IsMouseDown(Rect rect)
    {
        if (!MouseDown) return false;
        return rect.Contains(Event.current.mousePosition);
    }
    public static bool DoTriggerButton(UBTriggerContent ubTriggerContent)
    {
        var hasSubLabel = !String.IsNullOrEmpty(ubTriggerContent.SubLabel);

        var rect = !hasSubLabel
            ? GetRect(ubTriggerContent.BackgroundStyle)
            : GetRect(ubTriggerContent.BackgroundStyle, GUILayout.Height(35));
        if (DoubleClickArea(rect))
        {
            if (ubTriggerContent.OnDoubleClicked != null)
                ubTriggerContent.OnDoubleClicked();
        }
        if (DropAreaGUI<UBActionContent, UBActionSheet>(rect, ubTriggerContent.Sheet,
            (source, target) =>
            {
                if (ubTriggerContent.Sheet == null && ubTriggerContent.Info != null)
                {
                    var sheet = ubTriggerContent.Info.Initialize(source.Behaviour);
                    UBUtils.MoveActionTo(source.Action, sheet);
                }
                else
                {
                    UBUtils.MoveActionTo(source.Action, target);
                }

                _CurrentlyOverSheet = null;
            }))
        {
            ubTriggerContent.OnDragOver();
        }
        else
        {
            // _CurrentlyOverSheet = null;
        }
        var style = _CurrentlyOverSheet == ubTriggerContent.Sheet && ubTriggerContent.Sheet != null
            ? UBStyles.CurrentActionBackgroundStyle
            : ubTriggerContent.BackgroundStyle;

        if (IsMouseDown(rect))
            style = UBStyles.IncludeTriggerBackgroundStyle;

        GUI.Box(rect, "", style);

        if (ubTriggerContent.IndicatorStyle != null)
        {
            var rectIndicator = new Rect(rect);
            rectIndicator.width = 2;
            rectIndicator.y -= 2;
            rectIndicator.x = rect.width - 2;
            rectIndicator.height -= 3;
            GUI.Box(rectIndicator, "", ubTriggerContent.IndicatorStyle);
        }
        if (ubTriggerContent.OptionsStyle != null && ubTriggerContent.OnShowOptions != null)
        {
            var eventOptionsButtonRect = new Rect(rect.x + 5, rect.y + ((rect.height / 2) - 8), 16, 16);
            if (GUI.Button(eventOptionsButtonRect, "", ubTriggerContent.OptionsStyle))
            {
                ubTriggerContent.OnShowOptions();
            }
            var seperatorRect = new Rect(rect);
            seperatorRect.width = 3;
            seperatorRect.y += 2;
            seperatorRect.height -= 5;
            seperatorRect.x = eventOptionsButtonRect.x + 17;
            GUI.Box(seperatorRect, string.Empty, UBStyles.SeperatorStyle);
        }

        var labelStyle = new GUIStyle(EditorStyles.label) { alignment = ubTriggerContent.TextAnchor, fontSize = 10 };
        var labelRect = new Rect(rect.x, rect.y - (hasSubLabel ? 6 : 0), rect.width - 30, rect.height);
        var lbl = PrettyLabel(ubTriggerContent.Label);
        var result = GUI.Button(labelRect, lbl, labelStyle);

        if (hasSubLabel)
        {
            var subLabelRect = new Rect(labelRect);
            subLabelRect.y += 12;

            GUI.Label(subLabelRect, ubTriggerContent.SubLabel, UBStyles.SubLabelStyle);
        }
        if (ubTriggerContent.ShowArrow)
            GUI.DrawTexture(new Rect(rect.x + rect.width - 18f, rect.y + ((rect.height / 2) - 8), 16, 16), UBStyles.ArrowRightTexture);
        return result;
    }

    public static void DoTriggerList(List<UBTriggerContent> instanceTriggers)
    {
        foreach (var trigger in instanceTriggers)
        {
            DoTriggerButton(trigger);
        }
    }

    public static bool DoubleClickArea(Rect rect)
    {
        var e = Event.current;
        if (e.isMouse && e.type == EventType.MouseDown && e.clickCount == 2)
        {
            if (!rect.Contains(Event.current.mousePosition))
            {
                e.Use();
                return true;
            }
        }
        return false;
    }

    public static void DoVariableDeclare(Rect rect, SerializedProperty expanded, SerializedProperty elementProperty, SerializedProperty nameProperty, SerializedProperty varTypeProperty, SerializedProperty declaresProperty, int i, bool allowEditing, bool canAllowOverrides, bool defaultStyle)
    {
        //if (varTypeProperty.enumValueIndex == 5)// || varTypeProperty.enumValueIndex == 8 || varTypeProperty.enumValueIndex == 9)
        //{
        //    rect.height = 35;
        //}
        if (!defaultStyle)
            GUI.Box(rect, "", expanded.boolValue ? UBStyles.CommandBarOpenStyleSmall : UBStyles.CommandBarClosedStyleSmall);
        if (DoubleClickArea(rect))
        {
            Debug.Log("Double Click");
        }
        var expandRect = new Rect(rect.x + 5, rect.y + 3, 16, 16);
        if (allowEditing)
        {
            expanded.boolValue = GUI.Toggle(expandRect, expanded.boolValue, "", expanded.boolValue
                ? UBStyles.FoldoutCloseButtonStyle
                : UBStyles.FoldoutOpenButtonStyle);
        }
        var propertyRect = new Rect(rect);
        propertyRect.x += 23f;
        propertyRect.y += 4f;

        propertyRect.width -= allowEditing ? 45f : 28f;
        propertyRect.height -= 9f;

        EditorGUIUtility.wideMode = false;
        VariableDeclareDrawer.DoDeclareField(propertyRect, elementProperty, false);
        if (!allowEditing) return;

        if (expanded.boolValue)
        {
            var exposeField = elementProperty.FindPropertyRelative("_Expose");
            var newValue = EditorGUILayout.TextField("Name", nameProperty.stringValue);
            if (newValue != nameProperty.stringValue)
            {
                nameProperty.stringValue = Regex.Replace(newValue, @"[^a-zA-Z0-9]", String.Empty);
            }

            EditorGUILayout.PropertyField(varTypeProperty);
            if (varTypeProperty.enumValueIndex == varTypeProperty.enumNames.Length - 1 && !EditorPrefs.GetBool("SystemObjectNotification", false))
            {
                EditorGUILayout.HelpBox("SystemObject Properties are for runtime only. Values will not be saved.", MessageType.Warning);
                if (GUILayout.Button("Okay I got it!"))
                {
                    EditorPrefs.SetBool("SystemObjectNotification", true);
                }
            }
            if (canAllowOverrides)
                EditorGUILayout.PropertyField(exposeField, new GUIContent("Allow Overrides"));

            var methodInfo =
                typeof(UBEditor)
                    .GetMethod(String.Format("UB{0}Options", varTypeProperty.enumNames[varTypeProperty.enumValueIndex]));
            if (methodInfo != null)
            {
                methodInfo.Invoke(null, new object[] { elementProperty });
            }
        }

        if (GUI.Button(new Rect(rect.x + rect.width - 19, rect.y + 5, 18, 18), String.Empty, UBStyles.RemoveButtonStyle))
        {
            declaresProperty.DeleteArrayElementAtIndex(i);
        }
    }

    public static void DoVariablesEditor(SerializedProperty declaresProperty, bool allowEditing = true, bool canAllowOverrides = true, bool defaultStyle = false)
    {
        for (var i = 0; i < declaresProperty.arraySize; i++)
        {
            var elementProperty = declaresProperty.GetArrayElementAtIndex(i);
            var varTypeProperty = elementProperty.FindPropertyRelative("_varType");
            var nameProperty = elementProperty.FindPropertyRelative("_name");
            var expanded = elementProperty.FindPropertyRelative("_Expanded");

            var rect = GetRect(UBStyles.CommandBarClosedStyleSmall,
                GUILayout.Height(GetHeight(varTypeProperty.enumValueIndex)));
            //EditorGUILayout.LabelField("GUID", elementProperty.FindPropertyRelative("_GUID").stringValue);
            DoVariableDeclare(rect, expanded, elementProperty, nameProperty, varTypeProperty, declaresProperty, i, allowEditing, canAllowOverrides, defaultStyle);
        }
    }

    public static bool DropAreaGUI<TSourceData, TTargetData>(Rect rect, TTargetData data, Action<TSourceData, TTargetData> dragComplete)
    {
        //GUI.Box(rect, "Add Trigger");
        var eventType = Event.current.type;
        //        Debug.Log(eventType);
        if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
        {
            if (!rect.Contains(Event.current.mousePosition))
            {
                return false;
                //if (_CurrentlyOverSheet == content.Sheet)
                //{
                //    _CurrentlyOverSheet = null;
                //}
            }

            //_CurrentlyOverSheet = content.Sheet;
            // Show a copy icon on the drag

            DragAndDrop.visualMode = DragAndDropVisualMode.Move;
            if (eventType == EventType.DragPerform)
            {

                var dragData = DragAndDrop.GetGenericData(typeof(TSourceData).Name);
                if (dragData == null) return false;
                DragAndDrop.AcceptDrag();
                dragComplete((TSourceData)dragData, data);
                DragAndDrop.PrepareStartDrag();
                Event.current.Use();
                return false;
            }

            //content.OnDragOver();

            Event.current.Use();
            return true;
        }
        if (eventType == EventType.DragExited)
        {
            _CurrentlyOverAction = null;
            _CurrentlyOverSheet = null;
        }
        return false;
    }

    public static IEnumerable<IUBVariableDeclare> GetContextVars(IUBehaviours behaviour, UBAction ubAction)
    {
        var declares = behaviour.GetIncludedDeclares().Concat(behaviour.Declares.Cast<IUBVariableDeclare>());
        var triggerInfo = ubAction.ActionSheet.TriggerInfo;
        if (triggerInfo != null)
        {
            declares = declares.Concat(UBTrigger.AvailableStaticVariablesByType(triggerInfo.TriggerTypeName));
        }
        foreach (var declare in declares)
        {
            yield return declare;
        }
    }

    public static bool DeclareFilterByType(Type systemType, IUBVariableDeclare declare)
    {
        if (systemType == typeof(string))
        {
            return true;
        }
        else
        {
            if (declare == null)
                return false;
            if (declare.ValueType == null)
            {
                Debug.Log("ValueType is null " + declare.Name);
                return false;
            }
            if (typeof(Enum).IsAssignableFrom(systemType))
            {
                return true;
            }

            else if (systemType.IsAssignableFrom(declare.ValueType))
            {
                return true;
            }
        }
        return false;
    }

    public static bool DoTreeItem(TreeItemInfo treeItemInfo)
    {
        Indent = treeItemInfo.Indent;
        var rect = GetRect(treeItemInfo.BackgroundStyle);

        GUI.Box(rect, string.Empty, IsMouseDown(rect) ? UBStyles.IncludeTriggerBackgroundStyle : treeItemInfo.BackgroundStyle);
        if (treeItemInfo.IconStyle != null && treeItemInfo.OnShowOptions != null)
        {
            var eventOptionsButtonRect = new Rect(rect.x + 5, rect.y + ((rect.height / 2) - 8), 16, 16);
            if (GUI.Button(eventOptionsButtonRect, "", treeItemInfo.IconStyle))
            {
                treeItemInfo.OnShowOptions();
                //ubTriggerContent.OnShowOptions();
            }
            var seperatorRect = new Rect(rect) { width = 3 };
            seperatorRect.y += 2;
            seperatorRect.height -= 5;
            seperatorRect.x = eventOptionsButtonRect.x + 17;
            GUI.Box(seperatorRect, string.Empty, UBStyles.SeperatorStyle);
        }

        var labelStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft, fontSize = 10 };
        var labelRect = new Rect(rect.x + 25, rect.y, rect.width, rect.height);
        var lbl = PrettyLabel(treeItemInfo.Label);
        var result = GUI.Button(labelRect, lbl, labelStyle);
        if (treeItemInfo.ShowArrow)
            GUI.DrawTexture(new Rect(rect.x + rect.width - 18f, rect.y + ((rect.height / 2) - 8), 16, 16), UBStyles.ArrowRightTexture);
        return result;
    }

    public static int GetHeight(int enumIndex)
    {
        switch (enumIndex)
        {
            case 5:
                return 60;

            case 8:
            case 9:
                return 40;

            default:
                return 25;
        }
    }

    public static Rect GetRect(GUIStyle style, params GUILayoutOption[] options)
    {
        var rect = GUILayoutUtility.GetRect(GUIContent.none, style, options);
        if (IsGlobals) return rect;
        var indentAmount = (Indent * 25);
        rect.x -= 13;
        rect.x += +(indentAmount);
        rect.width += 17;
        rect.width -= indentAmount;
        rect.y += 3;
        return rect;
    }

    public static string PrettyLabel(string label)
    {
        return Regex.Replace(label.Replace("_", ""), @"((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim().Replace("  ", " ");
    }

    public static void UBEnumOptions(SerializedProperty property)
    {
        var enumTypeProperty = property.FindPropertyRelative("_enumType");
        GUILayout.BeginHorizontal();
        var type = String.IsNullOrEmpty(enumTypeProperty.stringValue)
            ? typeof(AnimationPlayMode)
            : UBHelper.GetType(enumTypeProperty.stringValue);

        if (GUILayout.Button(type.Name))
        {
            UBTypesWindow.Init("Object Type", ActionSheetHelpers.GetEnumTypes(),
                (prettyName, qualifiedName) =>
                {
                    enumTypeProperty.serializedObject.Update();
                    enumTypeProperty.stringValue = qualifiedName;
                    enumTypeProperty.serializedObject.ApplyModifiedProperties();
                });
        }
        GUILayout.EndHorizontal();
    }

    public static void UBObjectOptions(SerializedProperty property)
    {
        var typeProperty = property.FindPropertyRelative("_objectValueType");

        GUILayout.BeginHorizontal();
        var type = String.IsNullOrEmpty(typeProperty.stringValue)
            ? typeof(Object)
            : UBHelper.GetType(typeProperty.stringValue);

        if (GUILayout.Button(type.Name))
        {
            UBTypesWindow.Init("Object Type", ActionSheetHelpers.GetDerivedTypes<Object>(false, true).Where(p => !typeof(UBTrigger).IsAssignableFrom(p)),
                (prettyName, qualifiedName) =>
                {
                    typeProperty.serializedObject.Update();
                    typeProperty.stringValue = qualifiedName;
                    typeProperty.serializedObject.ApplyModifiedProperties();
                });
        }
        GUILayout.EndHorizontal();
    }

    public bool Button(string label)
    {
        var rect = GetRect(UBStyles.ButtonStyle);
        return GUI.Button(rect, label, UBStyles.ButtonStyle);
    }

    protected static void DoStackTrace()
    {
        var debugInfo = CurrentDebugInfo;
        if (debugInfo == null) return;
        if (debugInfo.Context == null) return;
        foreach (var item in debugInfo.Context.StackTrace)
        {
            DoTriggerButton(new UBTriggerContent(item.Name ?? string.Empty, UBStyles.CommandBarClosedStyle));
            //if (item is UBActionSheet)
            //{
            //    var actionSheet = item as UBActionSheet;
            //    var ubTriggerButton = new UBTriggerContent(item.Name, UBStyles.CommandBarClosedStyle);
            //    ubTriggerButton.BackgroundStyle = UBStyles.CommandBarClosedStyleSmall;
            //    DoTriggerButton(ubTriggerButton);
            //}
            //else
            //{
            //    var action = item as UBAction;
            //    var content = new UBActionContent()
            //    {
            //        BackgroundStyle = UBStyles.CommandBarClosedStyleSmall
            //    };
            //    DoActionButton(content);
            //}
        }
    }

    private static bool DoActionGUI(Object undoTarget, UBAction action)
    {
        var fields = action.GetVisibleFields();
        var changed = false;
        EditorGUILayout.Space();
        foreach (var field in fields)
        {
            if (field.GetCustomAttributes(typeof(HideInInspector), true).Length > 0) continue;
            var drawer = UBDrawers.GetDrawerFor(field);
            if (drawer == null) continue;

            if (drawer.Draw(undoTarget,
                action, field, field.GetValue(action), new GUIContent(PrettyLabel(field.Name))))
            {
                changed = true;
            }
        }
        
        return changed;
    }

    protected void AddAction(UBActionSheet actionSheet, string name, string typeName)
    {
      
        var type = UBHelper.GetType(typeName);
        var action = Activator.CreateInstance(type) as UBAction;
        if (action != null)
        {
            action.Expanded = false;
            Undo.RecordObject(target, "Add Action " + name);
            actionSheet.AddItem(action);
            EditorUtility.SetDirty(target);
            
        }
     
    }

    protected static string OnKeySelector(Type p)
    {
        var customAttribute = p.GetCustomAttributes(typeof(UBCategoryAttribute), true);

        if (customAttribute.Length > 0)
        {
            return ((UBCategoryAttribute)customAttribute[0]).Name;
        }
        return "Misc";
    }

    public string GetMemberName(MemberInfo member)
    {
        if (member is MethodInfo)
        {
            var methodInfo = member as MethodInfo;
            var pNames = methodInfo.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name).ToArray();
            return string.Format("{0}({1})", methodInfo.Name, string.Join(", ", pNames));
        }
        else
        {
            return member.Name;
        }
    }

    public void InitActions()
    {
        if (_actionGroups != null) return;
        
        _actionGroups = ActionSheetHelpers.GetDerivedTypes<UBAction>(false, false).GroupBy(p => OnKeySelector(p)).OrderBy(p => p.Key).ToArray();
    }

    public void DoAddActionGui(UBActionSheet sheet, Action close)
    {
        DoSubHeader("Add Action");
        EditorGUILayout.Space();
        InitActions();
        GUI.SetNextControlName("SearchText");
        var newText = GUILayout.TextField(SearchText ?? "");
        if (newText != SearchText || _members == null)
        {
            _members = UBUtils.TypeMemberSearch(newText).Take(20).ToArray();
            SearchText = newText;
        }
        var upperSearchText = SearchText.ToUpper();
        if (_actionGroups == null) return;
        Type firstItem = null;
        var itemsRendered = 0;
        foreach (var group in _actionGroups)
        {
            var filteredItems = @group.Where(p => p.Name.ToUpper().Contains(upperSearchText)).OrderBy(p => p.Name).ToArray();
            if (filteredItems.Length < 1) continue;
            DoSubHeader(group.Key);


            foreach (var item in filteredItems)
            {
                if (firstItem == null && Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return && !Event.current.shift)
                {
                    firstItem = item;
                    AddAction(sheet, item.Name, item.AssemblyQualifiedName);
                    SearchText = "";
                    //Event.current.Use();
                    close();
                    Event.current.Use();
                }
                //if (!_allowInstanceOnly && item.GetCustomAttributes(typeof(InstanceOnlyAttribute), true).Length > 0)
                //    continue;

                var help = item.GetCustomAttributes(typeof(UBHelpAttribute), true).FirstOrDefault() as UBHelpAttribute;
                var helpText = help == null ? string.Empty : help.Text;
                if (GUI.Button(GetRect(EditorStyles.toolbarButton),new GUIContent(item.Name, helpText), EditorStyles.toolbarButton))
                {
                    AddAction(sheet, item.Name, item.AssemblyQualifiedName);
                    close();
                }
                itemsRendered++;
                if (itemsRendered > 20)
                {
                    break;
                }
            }
            if (itemsRendered > 20)
            {
                break;
            }
        }
        if (itemsRendered < 20)
        {
            DoSubHeader("Generatable Actions");
            if (!string.IsNullOrEmpty(SearchText))
                foreach (var member in _members.OrderBy(p => p.DeclaringType.Name + p.Name))
                {
                    if (member.DeclaringType == null) continue;
                    if (GUI.Button(GetRect(EditorStyles.toolbarButton), member.DeclaringType.Name + "." + GetMemberName(member), EditorStyles.toolbarButton))
                    {
                        UBActionGeneratorWindow.Init(member);
                    }
                }
        }

  
       
    }
}
public class TreeItemInfo
{
    private string _label;
    private int _indent;
    private GUIStyle _backgroundStyle;
    private GUIStyle _iconStyle;
    private Action _onShowOptions;
    private bool _showArrow = true;

    public TreeItemInfo(string label, int indent, GUIStyle backgroundStyle, GUIStyle iconStyle, Action onShowOptions = null, bool showArrow = true)
    {
        _label = label;
        _indent = indent;
        _backgroundStyle = backgroundStyle;
        _iconStyle = iconStyle;
        _onShowOptions = onShowOptions;
        _showArrow = showArrow;
    }

    public TreeItemInfo(string label, UBActionSheet sheet, int indent, bool locked = false)
    {
        _indent = indent;

        _label = label;
        if (sheet != null && sheet.IsForward)
        {

            //_backgroundStyle = UBStyles.ForwardBackgroundStyle;
            //SubLabel = "Forward To: " + sheet.ForwardTo.DisplayName;
            _label += "->" + sheet.ForwardTo.DisplayName;
        }

        BackgroundStyle = locked ? UBStyles.IncludeTriggerBackgroundStyle : UBStyles.TreeItemStyle;
        _iconStyle = UBTriggerContent.GetTriggerOptionsStyle(sheet);
    }
    public TreeItemInfo(UBAction action, int indent)
    {
        _label = action.ToString();
        _indent = indent;
        BackgroundStyle = UBStyles.TreeItemStyleOdd;
        _iconStyle = UBStyles.TriggerForwardButtonStyle;

    }

    public string Label
    {
        get { return _label; }
        set { _label = value; }
    }

    public int Indent
    {
        get { return _indent; }
        set { _indent = value; }
    }

    public GUIStyle BackgroundStyle
    {
        get { return _backgroundStyle; }
        set { _backgroundStyle = value; }
    }

    public GUIStyle IconStyle
    {
        get { return _iconStyle; }
        set { _iconStyle = value; }
    }

    public Action OnShowOptions
    {
        get { return _onShowOptions; }
        set { _onShowOptions = value; }
    }

    public bool ShowArrow
    {
        get { return _showArrow; }
        set { _showArrow = value; }
    }
}
