using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UBehaviours.Actions;
using UnityEditor;
using UnityEngine;

//[CustomPropertyDrawer(typeof(UBVariableBase), false)]
[UBCustomDrawer(typeof(UBVariableBase))]
public class VariableDrawer : UBPropertyDrawer
{
    public override object DrawProperty(object target, IUBFieldInfo info, object value, GUIContent label)
    {
        var variable = value as UBVariableBase;

        if (variable == null)
        {
            EditorGUILayout.HelpBox("UBVariable is null.  Make sure you give it a default value.", MessageType.Warning);
            return value;
        }

        var customAttributes = info.GetCustomAttributes(true);
        var t = target as UBAction;
        var message = variable.CheckForErrors(UndoTarget as IUBehaviours, t, t.ActionSheet.TriggerInfo, IsRequired);
        if (message != null)
        {
            EditorGUILayout.HelpBox(message, MessageType.Error);
        }
        var help = HelpText;
        if (help != null)
        {
            EditorGUILayout.HelpBox(help, MessageType.None);
        }

        var lastRect = GUILayoutUtility.GetLastRect();
        var rect = new Rect(8, lastRect.y + lastRect.height + 3, 17, 17);
        var v = (UBVariableBase)Activator.CreateInstance(info.FieldType);

        var contextVars = UBEditor.GetContextVars(UndoTarget as IUBehaviours, target as UBAction).ToArray();
        var options = GetContextOptions(target, v, contextVars, customAttributes);
        //Debug.Log(string.Join(Environment.NewLine ,options.Select(p=>p._ValueFrom + " : " + p._ValueFromVariableName).ToArray()));
        //Debug.Log(variable.ValueFrom + " : " + variable._ValueFromVariableName);
        var strOptions = options.Select(p => p._DisplayName).ToArray();
        var intOptions = options.Select(p => p._ValueFrom).ToArray();

        var selectedOption =
            options.FirstOrDefault(p => p._ValueFrom == variable.ValueFrom && p._ValueFromVariableName == variable._ValueFromVariableName) ?? options.FirstOrDefault(p=>p._ValueFrom == variable.ValueFrom);

        var selectedIndex = options.IndexOf(selectedOption);
        if (selectedIndex < 0)
        {
            selectedOption = options.First();
            selectedIndex = 0;
            variable.ValueFrom = 1;
            ForceSave = true;

        }
        EditorGUI.BeginChangeCheck();
        var newSelectedIndex = EditorGUI.Popup(rect, string.Empty, selectedIndex, strOptions, EditorStyles.popup);
        if (EditorGUI.EndChangeCheck())
        {
            selectedOption = options[newSelectedIndex];
            variable.ApplyValueFromInfo(selectedOption);
            ForceSave = true;
        }

        return DoValueFromGUI(target, variable, variable.ValueFrom,  selectedOption._DisplayName, label, contextVars);
    }

    private List<ValueFromInfo> GetContextOptions(object target, UBVariableBase v, IUBVariableDeclare[] contextVars,
        object[] customAttributes)
    {
        var options = v.GetValueFromOptions(Behaviour, target as UBAction, contextVars).ToList();
        
        if (customAttributes.OfType<UBRequireVariableAttribute>().FirstOrDefault() != null)
        {
            var removeKeys = new List<ValueFromInfo>();
            foreach (var option in options)
            {
                if (option._ValueFrom != 1)
                {
                    removeKeys.Add(option);
                }
            }
            foreach (var removeKey in removeKeys)
            {
                options.Remove(removeKey);
            }
        }
        return options;
    }

    protected virtual object DoValueFromGUI(object target, UBVariableBase variable, int index, string valueFromText, GUIContent label, IUBVariableDeclare[] contextVars)
    {
        EditorGUI.indentLevel++;
        Type valueType = variable.ValueType;
        switch (index)
        {
            case 0:

                var valueTypeFromAttribute = FieldInfo.GetCustomAttributes(typeof(ValueTypeFromAttribute), true).FirstOrDefault() as ValueTypeFromAttribute;

                if (valueTypeFromAttribute != null)
                {
                    var valueTypeVariable = target.GetType().GetField(valueTypeFromAttribute.FieldName).GetValue(target) as UBVariableBase;
                    if (valueTypeVariable != null)
                    {
                        var declare = ((IUBehaviours)UndoTarget).FindDeclare(valueTypeVariable._ValueFromVariableName);
                        if (declare != null)
                        valueType = declare.ValueType;
                    }
                }
                var valueField = variable.GetType().GetField("_value", BindingFlags.NonPublic | BindingFlags.Instance);
                if (variable is UBEnum)
                {
                    var values = (int[])Enum.GetValues(valueType);
                    var names = Enum.GetNames(valueType);
                    var enumIndex = Array.IndexOf(values, (int)variable.LiteralObjectValue);
                    var v = EditorGUILayout.Popup(label, enumIndex, names.Select(p => new GUIContent(p)).ToArray());
                    if (v > -1 && v < values.Length && values[v] != (int)variable.LiteralObjectValue)
                    {
                        valueField.SetValue(variable, values[v]);
                        ForceSave = true;
                        //variable.LiteralObjectValue = values[v];
                    }
                    else
                    {
                        // variable.LiteralObjectValue = values[0];
                    }
                }
                else if (variable is UBGameObject)
                {
                    var drawer = UBDrawers.GetDrawerFor(valueType);
                    drawer.Draw(UndoTarget, variable, new ReflectionFieldInfo(valueField), variable.LiteralObjectValue, label);
                }else if (variable is UBObject)
                {
                    var drawer = UBDrawers.GetDrawerFor(valueType);
                    drawer.Draw(UndoTarget, variable, new UBFieldInfo() { FieldInfo = valueField, FieldType = valueType }, variable.LiteralObjectValue, label);
                }
                else
                {
                    var drawer = UBDrawers.GetDrawerFor(valueType);
                    drawer.Draw(UndoTarget, variable, new ReflectionFieldInfo(valueField), variable.LiteralObjectValue, label);
                }

                //var value = variable.LiteralObjectValue;
                //if (prop.type == "UBEnum")
                //{
                //    var enumTypeString = prop.FindPropertyRelative("_enumType").stringValue;
                //    var enumType = UBHelper.GetType(enumTypeString);
                //    if (enumType == null)
                //    {
                //        EditorGUILayout.HelpBox("Couldn't find enum type.", MessageType.Error);
                //        return;
                //    }
                //    var names = Enum.GetNames(enumType);
                //    var values = (int[])Enum.GetValues(enumType);
                //    EditorGUI.BeginChangeCheck();
                //    var newResult = EditorGUI.Popup(pos, UBEditor.PrettyLabel(prop.name), Array.IndexOf(values, value.intValue), names);
                //    if (EditorGUI.EndChangeCheck())
                //    {
                //        value.intValue = values[newResult];
                //    }
                //}
                //else
                //{
                //    EditorGUI.PropertyField(pos, value, new GUIContent(UBEditor.PrettyLabel(prop.name)));
                //}

                break;

            case 1:

                string valueFrom = variable._ValueFromVariableName;

                var declares = contextVars.Where(p => UBEditor.DeclareFilterByType(variable.ValueType, p)).ToList();
                declares.Insert(0, new UBStaticVariableDeclare(){Name="[Empty]",Guid=string.Empty});
                var guidDeclares = declares.Select(p => p.Guid).ToArray();

                var filteredDeclares = declares.Select(p => p.Name).Concat(new[] { "Add New" }).ToArray();

                var selectedIndex = Array.IndexOf(guidDeclares, valueFrom);

                var newIndex = EditorGUILayout.Popup(label, selectedIndex, filteredDeclares.Select(p => new GUIContent(p)).ToArray());
                if (newIndex != selectedIndex && newIndex > -1 && newIndex < guidDeclares.Length)
                {
                    variable.ValueFrom = 1;
                    variable._ValueFromVariableName = declares[newIndex].Guid;
                    ForceSave = true;
                }
                else if (newIndex == guidDeclares.Length)
                {
                    var ubehaviours = UndoTarget as IUBehaviours;
                    var declare = variable.CreateAsDeclare();
                    variable.ValueFrom = 1;
                    variable._ValueFromVariableName = declare.Guid;
                    declare.Name = string.Format("{0}_{1}", target.GetType().Name, UBEditor.PrettyLabel(FieldInfo.Name).Replace(" ", ""));
                    ubehaviours.Declares.Add(declare);
                    ForceSave = true;
                } 
                break;

            default:

                EditorGUILayout.LabelField(label, new GUIContent(valueFromText));
                break;
        }
        EditorGUI.indentLevel--;
        return variable;
    }
}