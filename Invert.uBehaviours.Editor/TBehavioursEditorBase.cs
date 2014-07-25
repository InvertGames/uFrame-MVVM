using System;
using System.Collections.Generic;
using System.Linq;
using Invert.Common;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class TBehavioursEditorBase : UBEditor
{
    private static Stack<Object> _behaviourStack = new Stack<Object>();

    public static Stack<Object> BehaviourStack
    {
        get { return _behaviourStack; }
        set { _behaviourStack = value; }
    }

    public static bool TreeView
    {
        get
        {
            return EditorPrefs.GetBool("UBTreeView", false);
        }
        set
        {
            EditorPrefs.SetBool("UBTreeView", value);
        }
    }
    public static UBAction JumpTo { get; set; }

    [NonSerialized]
    private bool _initialized;

    [SerializeField]
    private List<string> _persistedSheetStack = new List<string>();

    [NonSerialized]
    private List<UBActionSheet> _sheetStack;

    private IBehaviourNotification[] _errors;

    public UBActionSheet CurrentActionSheet
    {
        get
        {
            return SheetStack.LastOrDefault();
        }
    }

    public TriggerInfo CurrentTrigger
    {
        get
        {
            if (SheetStack.Count < 1) return null;
            var sheet = SheetStack.First();
            return Target.Triggers.FirstOrDefault(p => p.Sheet == sheet);
        }
    }

    public bool IncludesOpen
    {
        get { return EditorPrefs.GetBool("UBReferencesOpen", true); }
        set { EditorPrefs.SetBool("UBReferencesOpen", value); }
    }

    public bool IsShowingTriggers
    {
        get
        {
            return CurrentActionSheet == null;
        }
    }

    public List<string> PersistedSheetStack
    {
        get { return _persistedSheetStack; }
        set { _persistedSheetStack = value; }
    }

    public bool SettingsOpen
    {
        get { return EditorPrefs.GetBool("UBSettingsOpen", true); }
        set { EditorPrefs.SetBool("UBSettingsOpen", value); }
    }

    public List<UBActionSheet> SheetStack
    {
        get { return _sheetStack ?? (_sheetStack = new List<UBActionSheet>()); }
        set { _sheetStack = value; }
    }

    public virtual IUBehaviours Target
    {
        get
        {
            return target as IUBehaviours;
        }
    }

    public bool TriggerDeclaresOpen
    {
        get { return EditorPrefs.GetBool("UBTriggerVariablesOpen", true); }
        set { EditorPrefs.SetBool("UBTriggerVariablesOpen", value); }
    }

    public bool TriggersOpen
    {
        get { return EditorPrefs.GetBool("UBTriggersOpen", true); }
        set { EditorPrefs.SetBool("UBTriggersOpen", value); }
    }

    public bool VariablesOpen
    {
        get { return EditorPrefs.GetBool("UBVariablesOpen", true); }
        set { EditorPrefs.SetBool("UBVariablesOpen", value); }
    }

    public void DoActionOptions(UBActionContent content)
    {
        var menu = new GenericMenu();

        menu.AddItem(new GUIContent("Enabled"), content.Action.Enabled, () =>
        {
            content.Action.Enabled = !content.Action.Enabled;
            content.Save();
        });
        menu.AddItem(new GUIContent("Breakpoint"), content.Action.Breakpoint, () =>
        {
            content.Action.Breakpoint = !content.Action.Breakpoint;
            content.Save();
        });
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Copy"), false, () =>
        {
            UBUtils.CopyActionToClipboard(content.Action);
        });
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Duplicate"), false, () =>
        {
            UBUtils.DuplicateAction(content.Action);
        });
        if (SheetStack.Count > 1)
        {
            menu.AddItem(new GUIContent("Move Up"), false, () =>
            {
                UBUtils.MoveActionTo(content.Action, SheetStack[SheetStack.Count - 2]);
            });
        }

        menu.ShowAsContext();
    }

    public virtual IEnumerable<IUBVariableDeclare> GetLockedVariableDeclares()
    {

        var locked = Target.GetIncludedDeclares();
        foreach (var ubVariableDeclare in locked)
        {
            yield return ubVariableDeclare;
        }
    }

    public virtual IEnumerable<UBActionContent> GetSheetActionContents(UBActionSheet sheet)
    {
        foreach (var action in sheet.Actions)
        {
            UBAction action1 = action;
            var content = new UBActionContent(sheet, action)
            {
                Behaviour = Target,
                OnShowOptions = DoActionOptions,
                OnMoveForward = (s) => SheetStack.Add(s),
                OnSubContents = DoSubContents
            };

            yield return content;
        }
    }

    public virtual IEnumerable<IUBVariableDeclare> GetTriggerDeclares()
    {
        var trigger = CurrentTrigger;
        if (trigger != null)
        {
            var result = UBTrigger.AvailableStaticVariablesByType(trigger.TriggerTypeName);
            foreach (var item in result)
                yield return item;
        }
    }

    public void NavigateTo(string[] path)
    {
        var sheets = path.Select(p => Target.FindSheet(p)).ToArray();
        NavigateTo(sheets);
    }

    /// <summary>
    /// Navigate to a path based off an ActionSheet array
    /// </summary>
    /// <param name="path"></param>
    public void NavigateTo(UBActionSheet[] path)
    {
        if (path.Length < 1) return;
        SheetStack.Clear();
        PersistedSheetStack.Clear();
        foreach (var p in path)
            Forward(p);
    }

    /// <summary>
    /// Navigate to an action
    /// </summary>
    /// <param name="action"></param>
    public void NavigateTo(UBAction action)
    {
        NavigateTo(action.GetPath().ToArray());
        foreach (var a in action.ActionSheet.Actions)
        {
            a.Expanded = a == action;
        }
    }

    public void NavigateTo(UBActionSheet sheet)
    {
        NavigateTo(sheet.SheetPath.ToArray());
    }

    public override void OnInspectorGUI()
    {
        foreach (var trigger in Target.Triggers)
        {
            trigger.Sheet.Load(Target, CurrentTrigger);
        }
        foreach (var sheet in Target.Sheets)
        {
            sheet.Load(Target);
        }
        _errors = Target.GetNotifications().ToArray();
        foreach (var include in Target.Includes)
        {
            include.Sync();
        }
        if (Event.current.type == EventType.MouseDown)
        {
            UBEditor.MouseDown = true;
        }
        else if (Event.current.type == EventType.MouseUp)
        {
            UBEditor.MouseDown = false;
        }
        if (JumpTo != null)
        {
            if (JumpTo.RootContainer == Target)
            {
                NavigateTo(JumpTo);
                JumpTo = null;
            }


        }
        if (ReloadNavigation())
        {

            //Repaint();

            //
            //
            // return;
        }

        if (BehaviourStack.Count > 0)
        {
            if (Button("Back to " + BehaviourStack.Peek().name))
            {
                var b = BehaviourStack.Pop();
                Selection.activeObject = b;
            }
        }
        IsGlobals = false;
   

        serializedObject.Update();

        //if (!_initialized)
        //{
      

        if (EditorApplication.isPlaying || EditorApplication.isPaused)
        {
            DebugToolbar();
            DoStackTrace();
        }
        if (!_initialized)
        {
            SheetStack.Clear();
            _initialized = true;
        }

        //}

        if (IsShowingTriggers)
        {
            if (DoToolbar("Triggers", TriggersOpen, OnShowTriggerWindow, null, () => { TreeView = !TreeView; }, null, TreeView ? UBStyles.ListButtonStyle : UBStyles.TreeviewButtonStyle)) TriggersOpen = !TriggersOpen;
            if (TriggersOpen)
            {
                if (TreeView)
                {

                    var groups = Target.GetTriggerGroups();
                    foreach (var triggerGroup in groups)
                    {
                        if (triggerGroup.Locked) continue;
                        if (triggerGroup.Triggers.Any())
                            DoSubHeader(triggerGroup.Name);
                        DoTriggerTree(triggerGroup.Triggers, triggerGroup.Locked);
                    }

                }
                else
                {
                    DoTriggerLists();
                }
            }


            DoAdditionalTriggerLists();


        }
        else
        {
            DoToolbar(CurrentActionSheet.Path, true, null, OnGoBack, UBUtils.ClipboardAction == null ? (Action)null : () =>
            {
                UBUtils.PasteAction(CurrentActionSheet);
            });
            if (CurrentActionSheet == null) return;
            var actionContents = GetSheetActionContents(CurrentActionSheet).ToArray();
            DoActionSheet(actionContents, DoTriggerOptions);
            InitActions();

            if (IsShowActions)
            {
                DoAddActionGui(CurrentActionSheet, ()=> { IsShowActions = false; });
                if (GUI.GetNameOfFocusedControl() == string.Empty)
                {

                    EditorGUI.FocusTextInControl("SearchText");
                    //GUI.FocusControl("SearchText");
                }
            }
            else
            {
                if (GUI.Button(GetRect(UBStyles.ButtonStyle),"Add Action",UBStyles.ButtonStyle) || (Event.current.keyCode == KeyCode.Return && Event.current.shift) && Event.current.type == EventType.KeyDown)
                {
                    IsShowActions = true;
                    Event.current.Use();
                    return;
                }    
            }
            
        }

        if (DoToolbar("Variables", VariablesOpen, OnAddVariable)) VariablesOpen = !VariablesOpen;
        if (VariablesOpen) DoVariables();

        DoAdditionalGUI();
        if (IsShowingTriggers)
        {

            DoSettings(Target);
        }
        serializedObject.ApplyModifiedProperties();
        DoErrors();
        foreach (var trigger in Target.Triggers)
        {
            trigger.Sheet.Load(Target, CurrentTrigger);
        }
        foreach (var sheet in Target.Sheets)
        {
            sheet.Load(Target);
        }
        //if (DoToolbar("Settings", SettingsOpen)) SettingsOpen = !SettingsOpen;
        //if (SettingsOpen)
        //{
        //}
        //if (GUI.Button(GetRect(UBStyles.ButtonStyle), "Generate Code", UBStyles.ButtonStyle))
        //{
        //    var visitable = Target as IBehaviourVisitable;
        //    var generator = new UBehaviourCSharpGenerator(visitable);//, "MyBehaviour", "public class {0} : MonoBehaviour");
        //    Debug.Log(generator.ToString());
        //}
      
   
        if (Event.current.type == EventType.ValidateCommand &&
           Event.current.commandName == "UndoRedoPerformed")
        {
            // Tell our target to update because it's properties have changed
            foreach (var trigger in Target.Triggers)
            {
                trigger.Sheet.Load(Target);
            }
        }
        if (EditorApplication.isPlaying || EditorApplication.isPaused)
        {
            var debugInfo = CurrentDebugInfo;
            if (debugInfo != null)
            {
                if (debugInfo.CurrentBreakPoint != null)
                {
                    NavigateTo(debugInfo.CurrentBreakPoint);
                    debugInfo.CurrentBreakPoint = null;
                    Debug.Log(SheetStack.Count);
                    Debug.Log(CurrentActionSheet != null);
                }
            }
            DebugToolbar();
            Repaint();
        }



    }

    public bool IsShowActions
    {
        get
        {
            return EditorPrefs.GetBool("UBACTIONS_IsShowingActions", false);
        }
        set
        {
            EditorPrefs.SetBool("UBACTIONS_IsShowingActions",value);
        }
    }

    public virtual void DoSettings(IUBehaviours behaviour)
    {

        var oldCount = behaviour.Settings.Count;
        var settings = behaviour.GetSettings();
        var changed = behaviour.Settings.Count != oldCount;

        if (DoToolbar("Settings", SettingsOpen))
            SettingsOpen = !SettingsOpen;

        if (!SettingsOpen) return;
        EditorGUILayout.Space();
        foreach (var behaviourSetting in settings)
        {
            var drawer = UBDrawers.GetDrawerFor(behaviourSetting.FieldType);
            EditorGUI.BeginChangeCheck();
            var value = drawer.DrawProperty(behaviourSetting, behaviourSetting, behaviourSetting.Value, new GUIContent(behaviourSetting.Name));
            if (EditorGUI.EndChangeCheck())
            {
                behaviourSetting.Value = value;
                changed = true;
            }
            //if (drawer.Draw(behaviour as UnityEngine.Object, behaviourSetting, behaviourSetting, behaviourSetting.Value,
            //    new GUIContent(behaviourSetting.Name)))
            //{
            //    changed = true;
            //}

        }
        var globalsProperty = serializedObject.FindProperty("_globals");
        EditorGUILayout.PropertyField(globalsProperty);

        if (changed)
        {
            EditorUtility.SetDirty(behaviour as UnityEngine.Object);
        }
    }
    /// <summary>
    /// Pop the sheet stack list.
    /// </summary>
    public void Pop()
    {
        if (SheetStack.Count > 0)
        {
            SheetStack.RemoveAt(SheetStack.Count - 1);
            PersistedSheetStack.RemoveAt(PersistedSheetStack.Count - 1);
        }
    }

    /// <summary>
    /// Reload the navigation if the stacks are not consistent
    /// </summary>
    public bool ReloadNavigation()
    {
        //return false;
        if (SheetStack.Count == PersistedSheetStack.Count) return false;
        NavigateTo(PersistedSheetStack.ToArray());
        // If for some reason it can't reload
        if (SheetStack.Count != PersistedSheetStack.Count)
        {
            PersistedSheetStack.Clear();
            SheetStack.Clear();
        }
        return true;
    }

    public void SaveAll()
    {
        foreach (var trigger in Target.Triggers)
        {
            trigger.Sheet.Save(Target);
        }
        foreach (var sheet in Target.Sheets)
        {
            sheet.Save(Target);
        }
        EditorUtility.SetDirty(Target as UnityEngine.Object);
    }

    public string ValidateCustomTrigger(string name)
    {
        if (Target.Triggers.FirstOrDefault(p => p.DisplayName == name) != null)
        {
            return "A trigger with this name already exists.";
        }

        return null;
    }

    protected virtual void AddTrigger(SerializedProperty triggers, string name, string triggerType)
    {
        Undo.RecordObject(target, "Add Trigger " + name);
        Target.Triggers.Add(new TriggerInfo()
        {
            Guid = Guid.NewGuid().ToString(),
            DisplayName = name,
            TriggerTypeName = triggerType,
            Data = name,
            Sheet = Target.CreateSheet(name)
        });
        EditorUtility.SetDirty(this.target);
    }

    protected virtual void DoAdditionalGUI()
    {
    }

    protected virtual void DoAdditionalTriggerLists()
    {
    }

    protected virtual void DoTriggerLists()
    {
        //Target.GetAllTriggers()
        //DoSubHeader("Included Triggers");
        foreach (var group in Target.GetTriggerGroups())
        {
            var exists = false;
            foreach (var trigger in group.Triggers)
            {
                if (!exists)
                {
                    DoSubHeader(group.Name);
                    exists = true;
                }
                if (group.Locked)
                {
                    if (DoTriggerButton(
                        new UBTriggerContent(trigger.Name,
                            UBStyles.IncludeTriggerBackgroundStyle)))
                    {
                        if (!BehaviourStack.Contains(target))
                        {

                            BehaviourStack.Push(target);
                        }
                        Selection.activeObject = group.Behaviour;
                    }
                }
                else
                {
                    DoTriggerItem(trigger);
                }
            }
        }
        //foreach (var trigger in Target.GetInstanceTriggers())//.Where(p => p.Flags != TriggerFlags.Custom && p.Flags != TriggerFlags.IncludedEvent && p.Flags != TriggerFlags.Included))
        //{
        //    DoTriggerItem(trigger);
        //}

        //var exists = false;
        //foreach (var trigger in Target.GetCustomTriggers())
        //{
        //    if (!exists)
        //    {
        //        DoSubHeader("Custom");
        //        exists = true;
        //    }
        //    DoTriggerItem(trigger);
        //}
        //exists = false;
        //var predefinedTriggers = Target.GetIncludedTriggers();
        //foreach (var predefinedTrigger in predefinedTriggers)
        //{
        //    if (!exists)
        //    {
        //        DoSubHeader("Included");
        //        exists = true;
        //    }

        //    if (!predefinedTrigger.Exists)
        //    {
        //        if (DoTriggerButton(new UBTriggerContent(predefinedTrigger.DisplayName, UBStyles.EventSmallButtonStyle)))
        //        {
        //            predefinedTrigger.Sheet = Target.CreateSheet(predefinedTrigger.Data.ToString());
        //            Target.Triggers.Add(predefinedTrigger);
        //            Forward(predefinedTrigger.Sheet);
        //            EditorUtility.SetDirty(target);
        //        }
        //    }
        //    else if (predefinedTrigger.Flags == TriggerFlags.IncludedEvent) // When the include has been created
        //    {
        //        DoTriggerItem(predefinedTrigger);
        //    }
        //    else // Included only
        //    {
        //        if (DoTriggerButton(
        //            new UBTriggerContent(predefinedTrigger.Name,
        //                UBStyles.IncludeTriggerBackgroundStyle, UBStyles.InstanceTriggerBackgroundStyle, null, null,
        //                false)))
        //        {

        //        }
        //    }
        //}
    }

    protected virtual void DoTriggerTree(IEnumerable<TriggerInfo> triggers, bool locked)
    {
        Indent = 0;
        var odd = false;
        foreach (var trigger in triggers)
        {
            TriggerInfo trigger1 = trigger;

            var sheetInfo = new TreeItemInfo(trigger.DisplayName, trigger.Sheet, 0, locked)
            {
                OnShowOptions = () =>
                {

                    DoTriggerOptions(trigger1.Sheet);
                }
            };
            if (DoTreeItem(sheetInfo))
            {
                TriggerForward(trigger1);
            }

            //GUILayout.Box(trigger.DisplayName, UBStyles.CommandBarButtonStyle, GUILayout.ExpandWidth(false),GUILayout.Height(25));
            //DoTriggerItem(trigger);
            odd = !odd;
            if (trigger.Sheet == null || locked) continue;
            DoActionSheetTree(trigger.Sheet, 1, odd);

        }
        Indent = 0;
    }

    protected virtual void DoActionSheetTree(UBActionSheet sheet, int indent, bool isOdd)
    {
        var odd = isOdd;
        foreach (var action in sheet.Actions)
        {
            var sheets = action.GetAvailableActionSheets(Target).ToArray();
            //DoActionButton(new UBActionContent(sheet, action));
            UBAction action1 = action;
            if (action1.RootContainer == null)
                action1.RootContainer = Target;

            var actionInfo = new TreeItemInfo(action, indent)
            {
                OnShowOptions = () =>
                {
                    DoActionOptions(new UBActionContent(action1.ActionSheet, action1) { Behaviour = Target });
                }
            };

            if (DoTreeItem(actionInfo))
            {
                Forward(action.ActionSheet);
                foreach (var a in action.ActionSheet.Actions)
                {
                    a.Expanded = a == action;
                }
            }

            foreach (var actionSheet in sheets)
            {
                if (actionSheet.Sheet == null) continue;
                odd = !odd;
                UBActionSheetInfo sheet1 = actionSheet;
                var sheetInfo = new TreeItemInfo(actionSheet.Name, sheet1.Sheet, indent + 1)
                {
                    OnShowOptions = () => { DoTriggerOptions(sheet1.Sheet); }
                };

                if (DoTreeItem(sheetInfo))
                {
                    if (actionSheet.Sheet == null)
                    {
                        var s = actionSheet.Initialize(Target);
                        Forward(s);
                        EditorUtility.SetDirty(target);
                    }
                    else
                    {
                        Forward(actionSheet.Sheet);
                    }

                }
                //DoTriggerButton(new UBTriggerContent(actionSheet.Name, actionSheet.Sheet));

                DoActionSheetTree(actionSheet.Sheet, indent + 2, odd);
                //Indent--;
            }
            odd = !odd;
        }

    }
    private void DoTriggerItem(TriggerInfo trigger)
    {
        var content = new UBTriggerContent(trigger.DisplayName, trigger.Sheet);

        //if (trigger.IsCustom)
        //{
        //    content.BackgroundStyle = UBStyles.CommandBarClosedStyle;
        //    content.IndicatorStyle = UBStyles.ForwardBackgroundStyle;
        //}
        TriggerInfo closureSafeTrigger = trigger;
        content.OnClicked = () => TriggerClicked(closureSafeTrigger);
        if (trigger.Exists)
            content.OnShowOptions = () => DoTriggerOptions(closureSafeTrigger.Sheet, closureSafeTrigger);

        if (DoTriggerButton(content))
        {
            TriggerForward(trigger);
        }
    }

    private void TriggerForward(TriggerInfo trigger)
    {
        if (trigger.Sheet == null)
        {
            trigger.Sheet = Target.CreateSheet(trigger.Data.ToString());
            EditorUtility.SetDirty(target);
        }
        if (!Target.Triggers.Contains(trigger))
        {
            Target.Triggers.Add(trigger);
            EditorUtility.SetDirty(target);
            Forward(trigger.Sheet);
        }
        else
        {
            TriggerClicked(trigger);
        }
    }

    protected virtual void DoVariables()
    {
        if (EditorApplication.isPlaying && Target is IUBContext)
        {
            EditorGUILayout.Space();
            var context = Target as IUBContext;

            foreach (var v in context.Variables)
            {
                var value = v.LiteralObjectValue;
                EditorGUILayout.LabelField(v.Name, value == null ? "null" : value.ToString());
            }
        }
        else
        {
            var declaresProperty = serializedObject.FindProperty("_declares");
            if (declaresProperty.arraySize > 0)
                DoSubHeader(Target.Name);

            DoVariablesEditor(declaresProperty, true, CanAllowOverrides);
            var includes = serializedObject.FindProperty("_includes");

            for (var i = 0; i < includes.arraySize; i++)
            {
                var include = includes.GetArrayElementAtIndex(i);

                DoIncludeVariables(include);
            }


            var lockedDeclares = GetLockedVariableDeclares();
            var exists = false;
            if (lockedDeclares != null)
                foreach (var lockedDeclare in lockedDeclares)
                {
                    if (!exists)
                    {
                        DoSubHeader("Locked");
                        exists = true;
                    }

                    DoLockedDeclare(lockedDeclare);

                    //GUI.Box(rect, "",);
                }
            if (!IsShowingTriggers)
            {
                var triggerDeclares = GetTriggerDeclares().ToArray();
                if (triggerDeclares.Length > 0)
                {
                    DoSubHeader(CurrentTrigger.DisplayName);
                    foreach (var lockedDeclare in triggerDeclares)
                    {
                        DoLockedDeclare(lockedDeclare);
                    }
                }
            }




        }
    }

    protected void DoIncludeVariables(SerializedProperty include, bool defaultStyle = false)
    {
        if (include == null) return;
        //Debug.Log(include.propertyType);
        //if (include.propertyType != SerializedPropertyType.ObjectReference) return;
        //if (include.objectReferenceValue == null) return;
        try
        {
            var includeDeclaresProperty = include.FindPropertyRelative("_declares");

            if (includeDeclaresProperty.arraySize > 0)
            {
                if (!defaultStyle)
                DoSubHeader(include.FindPropertyRelative("_behaviour").objectReferenceValue.name);

                DoVariablesEditor(includeDeclaresProperty, false, true, defaultStyle);
            }
        }
        catch (NullReferenceException ex)
        {
            
        }
    }

    public virtual bool CanAllowOverrides
    {
        get { return true; }
    }

    protected virtual IEnumerable<UBActionSheet> GetActionSheets()
    {
        return Target.Triggers.Select(p => p.Sheet);
    }

    protected virtual void OnAddVariable()
    {
        var declaresProperty = serializedObject.FindProperty("_declares");
        declaresProperty.InsertArrayElementAtIndex(declaresProperty.arraySize);
        var element = declaresProperty.GetArrayElementAtIndex(declaresProperty.arraySize - 1);
        var guidProperty = element.FindPropertyRelative("_GUID");
        var nameProperty = element.FindPropertyRelative("_name");
        nameProperty.stringValue = "NewVariable" + declaresProperty.arraySize;
        guidProperty.stringValue = Guid.NewGuid().ToString();
    }

    protected virtual void OnGoBack()
    {
        Pop();
    }

    protected virtual void OnShowActionsWindow()
    {
        

        //UBActionsWindow.Init(AddAction, true);
    }

    protected virtual void OnShowTriggerWindow()
    {
        var triggersProperty = serializedObject.FindProperty("_triggers");
        UBTriggersWindow.Init((n, tt) => AddTrigger(triggersProperty, n, tt), true, ValidateCustomTrigger);
    }

    protected virtual void TriggerClicked(TriggerInfo trigger)
    {
        if (trigger.Sheet == null)
        {
            trigger.Sheet = Target.CreateSheet(trigger.DisplayName);
        }
        Forward(trigger.Sheet);
    }

    private static void DoLockedDeclare(IUBVariableDeclare lockedDeclare)
    {
        var rect = GetRect(UBStyles.EventSmallButtonStyle);

        GUI.Box(rect, String.Empty, GUIStyle.none);
        var style = new GUIStyle(EditorStyles.label);
        style.normal.textColor = new Color(0.5f, 0.5f, 0.5f);
        GUI.Label(new Rect(rect.x + 22, rect.y + 1, rect.width / 2, rect.height), lockedDeclare.Name.ToString(), style);
        GUI.Label(new Rect(rect.x + (rect.width / 2) + 5, rect.y + 1, rect.width / 2, rect.height),
            lockedDeclare.DefaultValue == null ? "null" : lockedDeclare.DefaultValue.ToString(), style);
    }

    private void DoErrors()
    {
        
        DoErrors(_errors, action =>
        {
            NavigateTo(action.Source as UBAction);
        });
    }

    private void DoSubContents(UBAction action)
    {
        var sheets = action.GetAvailableActionSheets(Target);

        foreach (var actionSheet in sheets)
        {
            var content = new UBTriggerContent(actionSheet.Name, actionSheet.Sheet)
            {
                Info = actionSheet
            };
            UBActionSheetInfo info = actionSheet;
            content.OnShowOptions = () =>
            {
                if (info.Sheet == null)
                {
                    var sheet = info.Initialize(Target);
                    EditorUtility.SetDirty(target);
                    DoTriggerOptions(sheet);
                }
                else
                    DoTriggerOptions(info.Sheet);
            };
            content.OnDragOver = () =>
            {
                _CurrentlyOverSheet = info.Sheet;
                Repaint();
            };
            if (DoTriggerButton(content))
            {
                if (info.Sheet == null)
                {
                    var sheet = info.Initialize(Target);
                    EditorUtility.SetDirty(target);
                    Forward(sheet);
                }

                else
                    Forward(info.Sheet);
            }
        }
    }

    private void DoTriggerOptions(UBActionSheet actionSheet)
    {
        DoTriggerOptions(actionSheet, null);
    }

    private void DoTriggerOptions(UBActionSheet actionSheet, TriggerInfo triggerInfo)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Remove"), false, () => RemoveActionSheet(actionSheet));
        if (actionSheet.IsForward)
        {
            menu.AddItem(new GUIContent("Remove Forward"), false, () =>
            {
                actionSheet.IsForward = false;
                EditorUtility.SetDirty(target);
            });
        }
        menu.AddSeparator("");
        if (triggerInfo != null)
        {
            menu.AddItem(new GUIContent("Rename"), false, () =>
            {
                UBInputDialog.Init("Rename Trigger", (newName) =>
                {
                    Undo.RecordObject(target, "Change Trigger Name");
                    triggerInfo.DisplayName = newName;
                    triggerInfo.Sheet.Name = newName;
                    EditorUtility.SetDirty(target);
                }, triggerInfo.DisplayName);
            });
            menu.AddSeparator("");
        }

        var actionSheets2 = Target.GetAllTriggers();// GetActionSheets().ToArray();
        foreach (var trigger in actionSheets2)
        {
            var closureSafeTrigger = trigger;
            var sheet = trigger.Sheet;

            if (sheet == null || sheet == actionSheet) continue;
            var canCopyTo = true;//objReference.IsInstance == actionSheet.IsInstance ||
            //(!actionSheet.IsInstance && objReference.IsInstance);

            if (canCopyTo)
            {
                menu.AddItem(new GUIContent(sheet.Name + "/Send Copy To"), false, () =>
                {
                });
            }
            if (canCopyTo)
            {
                menu.AddItem(new GUIContent(sheet.Name + "/Move To"), false, () =>
                {
                    var targetSheet = sheet;

                    foreach (var action in actionSheet.Actions)
                    {
                        action.ActionSheet = targetSheet;
                        targetSheet.Actions.Add(action);
                    }
                    actionSheet.Actions.Clear();
                });
            }
            if (canCopyTo)
            {
                menu.AddItem(new GUIContent(sheet.Name + "/Move and Replace"), false, () =>
                {
                    Undo.RecordObject(target, "Move and Replace Sheet");
                    var targetSheet = sheet;
                    targetSheet.Actions.Clear();
                    foreach (var action in actionSheet.Actions)
                    {
                        action.ActionSheet = targetSheet;
                        targetSheet.Actions.Add(action);
                    }
                    actionSheet.Actions.Clear();
                    SaveAll();
                });
            }
            menu.AddItem(new GUIContent(sheet.Name + "/Forward To"), actionSheet != null && actionSheet.IsForward && actionSheet.ForwardTo == closureSafeTrigger,
                new GenericMenu.MenuFunction(() =>
                {
                    Undo.RecordObject(target, "Forward");
                    actionSheet.ForwardTo = closureSafeTrigger;
                    SaveAll();
                })
                );
        }
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("To Custom Trigger"), false, () =>
        {
            Undo.RecordObject(target, "Sheet To Custom Trigger");
            UBUtils.SheetToCustomTrigger(actionSheet);
            SaveAll();
        });
        menu.ShowAsContext();
    }

    private void Forward(UBActionSheet ubActionSheet)
    {
        SheetStack.Add(ubActionSheet);
        PersistedSheetStack.Add(ubActionSheet.Guid);
    }

    private void RemoveActionSheet(UBActionSheet actionSheet)
    {
        var triggerItem = Target.Triggers.FirstOrDefault(p => p.Sheet == actionSheet);
        if (triggerItem != null)
        {
            Target.Triggers.Remove(triggerItem);
        }
        else
        {
            actionSheet.Actions.Clear();
        }
    }
}