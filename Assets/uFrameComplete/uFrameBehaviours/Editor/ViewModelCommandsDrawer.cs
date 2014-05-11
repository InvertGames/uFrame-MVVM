using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


[UBCustomDrawer(typeof(ViewModelCommandsAttribute))]
public class ViewModelCommandsDrawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
//        var behaviour = UndoTarget as IUBehaviours;
        //base.OnGUI(position, property, label);
        var action = target as UBAction;
        UFrameBehaviours uBehaviour = action.RootContainer as UFrameBehaviours ??
                                      action.RootContainer.Includes.Select(p => p.Behaviour)
                                          .OfType<UFrameBehaviours>()
                                          .FirstOrDefault();
        if (uBehaviour == null)
        {
            EditorGUILayout.HelpBox("This must be on a uFrameBehaviours asset.", MessageType.Error);
            return value;
        }
        var commands = ViewModel.GetReflectedCommands(uBehaviour.ViewModelType).Select(p => p.Key).ToArray();

        var selectedIndex = Array.IndexOf(commands, value);

        var newIndex = EditorGUILayout.Popup(label.text, selectedIndex, commands);
        if (newIndex != selectedIndex && newIndex > -1)
        {
            return commands[newIndex];
        }
        return value;
    }
}