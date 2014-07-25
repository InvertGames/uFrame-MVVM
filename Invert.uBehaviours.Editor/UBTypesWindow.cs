using System;
using System.Collections.Generic;
using System.Linq;
using Invert.Common;
using UnityEditor;
using UnityEngine;

public class UBTypesWindow : EditorWindow
{
    public string _AssetPath;
    public UBSharedBehaviour _Context;
    public string _SearchText = "";
    public int _SelectedIndex;
    private static IEnumerable<IGrouping<string, Type>> _triggerGroups;
    private Action<string, string> _OnAdd;
    private Vector2 _scrollPosition;

    public static string GroupByCategoryAttributeSelector(Type p)
    {
        var customAttribute = p.GetCustomAttributes(typeof(UBCategoryAttribute), true);

        if (customAttribute.Length > 0)
        {
            return ((UBCategoryAttribute)customAttribute[0]).Name;
        }
        return "Misc";
    }

    public static void Init(string title, IEnumerable<Type> types, Action<string, string> onAdd, Func<Type, string> groupBy = null)
    {
        // Get existing open window or if none, make a new one:
        var window = (UBTypesWindow)GetWindow(typeof(UBTypesWindow));
        Func<Type, string> keySelector = groupBy ?? GroupByCategoryAttributeSelector;
        _triggerGroups = types.GroupBy(p => keySelector(p)).OrderBy(p => p.Key).ToArray();
        window.title = title;
        window._OnAdd = onAdd;
        window.Show();
    }

    public void OnGUI()
    {
        _SearchText = GUILayout.TextField(_SearchText ?? "");

        var upperSearchText = _SearchText.ToUpper();
        if (_triggerGroups == null) return;
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        foreach (var group in _triggerGroups)
        {
            var filteredItems = group.Where(p => p.Name.ToUpper().Contains(upperSearchText)).OrderBy(p => p.Name).ToArray();
            if (filteredItems.Length < 1) continue;

            GUILayout.Box(group.Key, UBStyles.EventButtonStyle);

            foreach (var item in filteredItems)
            {
                if (typeof(UBInstanceTrigger).IsAssignableFrom(item)) continue;
                var name = UBEditor.PrettyLabel(item.Name.Replace("UB", ""));
                if (GUILayout.Button(name, UBStyles.ButtonStyle))//EditorStyles.toolbarButton))
                {
                    _OnAdd(name, item.AssemblyQualifiedName);
                    Close();
                }
            }
        }
        EditorGUILayout.EndScrollView();
    }
}