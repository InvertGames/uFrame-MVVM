using System;
using System.Collections.Generic;
using System.Linq;
using Invert.Common;
using UnityEditor;
using UnityEngine;

public class ElementItemTypesWindow : ElementDataWindow
{
    public string _AssetPath;
    //public UBSharedBehaviour _Context;
    protected IGrouping<string, ElementItemType>[] _triggerGroups;
    protected Action<ElementItemType> _OnAdd;

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

    public override void OnGUIScrollView()
    {
        if (_triggerGroups == null) return;
        foreach (var group in Enumerable.OrderBy<IGrouping<string, ElementItemType>, string>(_triggerGroups, p => p.Key))
        {
            var numberItems = 0;
            var filteredItems = @group.Where(p => _labelSelector(p).ToUpper().Contains(_upperSearchText)).OrderBy(p => _labelSelector(p)).ToArray();
            if (filteredItems.Length < 1) continue;

            GUILayout.Box(@group.Key, ElementDesignerStyles.SubHeaderStyle);

            foreach (var item in filteredItems)
            {
                if (GUILayout.Button(_labelSelector(item), ElementDesignerStyles.ButtonStyle))
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
    }
}