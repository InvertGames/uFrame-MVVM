using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ElementItemTypesWindow : EditorWindow
{
    public string _AssetPath;
    //public UBSharedBehaviour _Context;
    public string _SearchText = "";
    public int _SelectedIndex;
    protected IGrouping<string, ElementItemType>[] _triggerGroups;
    protected Action<ElementItemType> _OnAdd;
    private Vector2 _scrollPosition;
    private int _limit = 50;
    protected Func<ElementItemType, string> _labelSelector;

    public static void InitTypeListWindow(string title, ElementItemType[] items, Action<ElementItemType> onAdd)
    {
        // Get existing open window or if none, make a new one:
        var window = (ElementItemTypesWindow)GetWindow(typeof(ElementItemTypesWindow));
        window._triggerGroups = items.GroupBy(p => window.GroupBy(p)).ToArray();
        window.title = title;
        window._OnAdd = onAdd;
        window._labelSelector = window.GetLabel;
        window.Show();
    }

    public static string GroupByCategoryAttributeSelector(string p)
    {
        if (p == null)
            return "Context";
        var type = Type.GetType(p);
        if (type == null)
        {
            return "Context";
        }
        return type.AssemblyQualifiedName;

    }


    public virtual string GroupBy(ElementItemType item)
    {
        return item.Group;
    }

    public virtual string GetLabel(ElementItemType item)
    {
        return item.Label;
    }

    public void OnGUI()
    {
        _SearchText = GUILayout.TextField(_SearchText ?? "");
        GUILayout.Label("Search to find more...");
        var upperSearchText = _SearchText.ToUpper();
        if (_triggerGroups == null) return;
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        foreach (var group in _triggerGroups.OrderBy(p => p.Key))
        {
            var numberItems = 0;
            var filteredItems = group.Where(p => _labelSelector(p).ToUpper().Contains(upperSearchText)).OrderBy(p => _labelSelector(p)).ToArray();
            if (filteredItems.Length < 1) continue;

            GUILayout.Box(group.Key, UFStyles.SubHeaderStyle);

            foreach (var item in filteredItems)
            {
                if (GUILayout.Button(_labelSelector(item), UFStyles.ButtonStyle))
                {
                    _OnAdd(item);


                }
                numberItems++;
                if (numberItems >= _limit)
                {

                    EditorGUILayout.EndScrollView();
                    return;
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }

}