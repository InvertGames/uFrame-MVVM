using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class UBListWindow<TListItem> : EditorWindow
{
    public string _AssetPath;
    public UBSharedBehaviour _Context;
    public string _SearchText = "";
    public int _SelectedIndex;
    protected IGrouping<string, TListItem>[] _triggerGroups;
    protected Action<TListItem> _OnAdd;
    private Vector2 _scrollPosition;
    private int _limit = 50;
    protected Func<TListItem, string> _labelSelector;

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


    public virtual string GroupBy(TListItem item)
    {
        return string.Empty;
    }

    public virtual string GetLabel(TListItem item)
    {
        return item.ToString();
    }
    
    public void OnGUI()
    {
        _SearchText = GUILayout.TextField(_SearchText ?? "");
        GUILayout.Label("Search to find more...");
        var upperSearchText = _SearchText.ToUpper();
        if (_triggerGroups == null) return;
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
      
        foreach (var group in _triggerGroups.OrderBy(p=>p.Key))
        {
            var numberItems = 0;
            var filteredItems = group.Where(p => _labelSelector(p).ToUpper().Contains(upperSearchText)).OrderBy(p=>_labelSelector(p)).ToArray();
            if (filteredItems.Length < 1) continue;

            GUILayout.Box(group.Key, UBStyles.SubHeaderStyle);

            foreach (var item in filteredItems)
            {
                if (GUILayout.Button(_labelSelector(item), UBStyles.ButtonStyle))
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