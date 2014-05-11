using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[UBCustomDrawer(typeof(TriggerListAttribute))]
public class TriggerListDrawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        var behaviour = UndoTarget as IUBehaviours;
        //var trigger = target as ForwardTo;
        //if (trigger != null)
        //{
        //    var behavioursInstance = trigger..GetReferenceIfUsed(target as UBAction).LiteralObjectValue as UBSharedBehaviour;
        //    if (behavioursInstance != null)
        //    {
        //        behaviour = behavioursInstance;
        //    }
        //}
        var triggers = behaviour.GetAllTriggers();
        var triggerNames = triggers.Select(p => p.DisplayName).ToArray();
        var triggerGuids = triggers.Select(p => p.Guid).ToArray();
        var selectedIndex = Array.IndexOf(triggerGuids, (string)value);
        var newIndex = EditorGUILayout.Popup(label.text, selectedIndex, triggerNames);
        if (newIndex != selectedIndex)
        {
            return triggerGuids[newIndex];
        }
        return value;
    }
}