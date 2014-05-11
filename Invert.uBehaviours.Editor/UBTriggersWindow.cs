using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UBTriggersWindow : EditorWindow
{
    public string _AssetPath;
    public UBSharedBehaviour _Context;
    public string _SearchText = "";
    public int _SelectedIndex;
    private static IEnumerable<IGrouping<string, Type>> _triggerGroups;
    private bool _allowInstanceOnly;
    private Action<string, string> _OnAdd;
    private Vector2 _scrollPosition;
    private Func<string, string> _Validator;
    //[MenuItem("Tools/ActionSheets/Actions", false, 1)]
    //public static void Init()
    //{
    //    // Get existing open window or if none, make a new one:
    //    var window = (UBActionsWindow)GetWindow(typeof(UBActionsWindow));
    //    window.title = "View Tools";

    //    window.Show();
    //}
    //[MenuItem("Tools/ActionSheets/Actions", false, 1)]
    //public static void Init(UBehaviours behaviours, string assetPath)
    //{
    //    // Get existing open window or if none, make a new one:
    //    var window = (UBTriggersWindow)GetWindow(typeof(UBTriggersWindow));
    //    _triggerGroups = ActionSheetHelpers.GetDerivedTypes<UBTrigger>(false, false).GroupBy(p => OnKeySelector(p)).OrderBy(p => p.Key).ToArray();
    //    window.title = "uBehaviour Triggers";
    //    window._Context = behaviours;
    //    window._AssetPath = assetPath;
    //    window.Show();
    //}
    public static void Init(Action<string, string> onAdd, bool allowInstanceOnly = false, Func<string, string> validator = null)
    {
        // Get existing open window or if none, make a new one:
        var window = (UBTriggersWindow)GetWindow(typeof(UBTriggersWindow));
        _triggerGroups = ActionSheetHelpers.GetDerivedTypes<UBTrigger>(false, false).GroupBy(p => OnKeySelector(p)).OrderBy(p => p.Key).ToArray();
        window.title = "uBehaviour Triggers";
        window._OnAdd = onAdd;
        window._allowInstanceOnly = allowInstanceOnly;
        window._Validator = validator;
        window.Show();
    }

    public void OnGUI()
    {
        _SearchText = GUILayout.TextField(_SearchText ?? "");

        if (!string.IsNullOrEmpty(_SearchText))
        {
            if (_Validator != null)
            {
                var result = _Validator(_SearchText);
                if (result != null)
                {
                    EditorGUILayout.HelpBox(result, MessageType.Error);
                }
                else
                {
                    if (GUILayout.Button("Create as Custom"))
                    {
                        _OnAdd(_SearchText, typeof (UBCustomTrigger).AssemblyQualifiedName);
                        Close();
                    }

                }
            }
            else
            {
                if (GUILayout.Button("Create as Custom"))
                {
                    _OnAdd(_SearchText, typeof(UBCustomTrigger).AssemblyQualifiedName);
                    Close();
                }
            }
         
        }
        else
        {
            EditorGUILayout.HelpBox("Type to search or create a custom trigger.", MessageType.Info);
        }

        var upperSearchText = _SearchText.ToUpper();
        if (_triggerGroups == null) return;
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        var count = 0;
        foreach (var group in _triggerGroups)
        {
            var filteredItems = group.Where(p => p.Name.ToUpper().Contains(upperSearchText)).OrderBy(p => p.Name).ToArray();
            if (filteredItems.Length < 1) continue;

            GUILayout.Box(group.Key, UBStyles.CommandBarOpenStyle);

            foreach (var item in filteredItems)
            {
                if (typeof(UBInstanceTrigger).IsAssignableFrom(item)) continue;
                if (!_allowInstanceOnly && item.GetCustomAttributes(typeof(InstanceOnlyAttribute), true).Length > 0)
                    continue;
                count++;
                var name = UBEditor.PrettyLabel(item.Name.Replace("UB", ""));
                if (GUILayout.Button(name, EditorStyles.toolbarButton))
                {

                    _OnAdd(name, item.AssemblyQualifiedName);
                    Close();
                    //var actionSheet = _Context.CreateActionSheet(name);
                    //actionSheet.TriggerType = item.AssemblyQualifiedName;
                    //AssetDatabase.AddObjectToAsset(actionSheet, _AssetPath);
                    //_Context.ActionSheets.Add(actionSheet);
                    //Close();
                }
            }
        }

        EditorGUILayout.EndScrollView();
        //if (count < 1)
    }

    private static string OnKeySelector(Type p)
    {
        var customAttribute = p.GetCustomAttributes(typeof(UBCategoryAttribute), true);

        if (customAttribute.Length > 0)
        {
            return ((UBCategoryAttribute)customAttribute[0]).Name;
        }
        return "Misc";
    }
}