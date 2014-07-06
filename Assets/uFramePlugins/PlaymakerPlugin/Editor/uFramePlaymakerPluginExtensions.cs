using System;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMakerEditor;
using Invert.uFrame.Editor;
using UnityEngine;

public static class uFramePlaymakerPluginExtensions
{
    public static Dictionary<Type, VariableType> VariableTypes = new Dictionary<Type, VariableType>()
    {
        
        {typeof(bool),VariableType.Bool},
        {typeof(int),VariableType.Int},
        {typeof(Quaternion),VariableType.Quaternion},
        {typeof(Rect),VariableType.Rect},
        {typeof(Vector2),VariableType.Vector2},
        {typeof(Vector3),VariableType.Vector3},
        {typeof(float),VariableType.Float},
        {typeof(Color),VariableType.Color},
        {typeof(String),VariableType.String},
        {typeof(Texture),VariableType.Texture},
        
    };

    public static VariableType FsmVarType(this IViewModelItem item, IElementDesignerData data)
    {
        var type = Type.GetType(item.RelatedType);
        if (type != null)
        {
            if (VariableTypes.ContainsKey(type))
            {
                return VariableTypes[type];
            }
        }
        if (item.IsEnum(data))
        {
            return VariableType.String;
        }
        return VariableType.Object;
    }
    public static bool IsEnum(this IViewModelItem vmItem, IElementDesignerData data)
    {
        var item = data.AllDiagramItems.FirstOrDefault(p => p.Name == vmItem.RelatedTypeName);

        if (item == null)
            return false;

        return item is EnumData;
    }

    public static Type GetFsmType(this IViewModelItem item, IElementDesignerData data, bool asArray, out bool isEnum)
    {
        var type = Type.GetType(item.RelatedType);
        isEnum = false;
        if (type == null)
        {
            if (item.IsEnum(data))
            {
                isEnum = true;
                return asArray ? typeof(FsmString[]) : typeof(FsmString);
            }
        }

        if (type == null) return null;
        if (type == typeof(float))
            return asArray ? typeof(FsmFloat[]) : typeof(FsmFloat);
        if (type == typeof(bool))
            return asArray ? typeof(FsmBool[]) : typeof(FsmBool);
        if (type == typeof(string))
            return asArray ? typeof(FsmString[]) : typeof(FsmString);
        if (type == typeof(int))
            return asArray ? typeof(FsmInt[]) : typeof(FsmInt);
        if (type == typeof(float))
            return asArray ? typeof(FsmFloat[]) : typeof(FsmFloat);
        if (type == typeof(Vector3))
            return asArray ? typeof(FsmVector3[]) : typeof(FsmVector3);
        if (type == typeof(Vector2))
            return asArray ? typeof(FsmVector2[]) : typeof(FsmVector2);
        if (typeof(Enum).IsAssignableFrom(type))
        {
            isEnum = true;
            return asArray ? typeof(FsmString[]) : typeof(FsmString);

        }

        return null;
    }

    public static void CreateEnumPropertyFsm(this ViewModelPropertyData data)
    {
        
        FsmBuilder.AddFsmToSelected();
        FsmEditor.SelectedFsmComponent.FsmName = data.NameAsChangedMethod;
        AddEnumPropertyFsm(data,FsmEditor.SelectedState);
    }

    public static void AddVariableToFsm(this ViewModelPropertyData data)
    {
        var variable = FsmEditor.SelectedFsm.Variables.GetVariable(data.Name);
        if (variable == null)
        {
            FsmBuilder.AddVariable(FsmEditor.SelectedFsm, data.FsmVarType(data.Node.Data),
                data.Name, false);
        }
    }
    public static void AddBindingEventToFsm(this ViewModelPropertyData data)
    {
        var evt = FsmEditor.SelectedFsm.Events.FirstOrDefault(p => p.Name == data.NameAsChangedMethod);
        if (evt == null)
        {
            FsmEditor.Builder.AddEvent(FsmEditor.SelectedFsm, new FsmEvent(data.NameAsChangedMethod));
        }
    }
    public static void AddEnumPropertyFsm(this ViewModelPropertyData data,FsmState fsmState = null)
    {
        var state = fsmState ?? FsmEditor.Builder.AddState(new Vector2(30f, 30f));
        state.Name = "Determine Current " + data.Name;
        var en = data.RelatedNode() as EnumData;

        FsmEditor.Builder.SetStartState(state.Name);
        FsmEditor.Builder.AddGlobalTransition(state, FsmEditor.SelectedFsm.GetEvent(data.NameAsChangedMethod));

        data.AddBindingEventToFsm();
        data.AddVariableToFsm();

        if (en != null)
        {
            var statePosition = new Vector2(400f, 50f);
            foreach (var enumItem in en.EnumItems)
            {
                var evt = new FsmEvent(enumItem.Name);
                FsmEditor.Builder.AddEvent(FsmEditor.SelectedFsm, evt);
                var itemState = FsmEditor.Builder.AddState(statePosition);
                itemState.Name = enumItem.Name;
                FsmEditor.Builder.AddTransition(state, itemState.Name, evt);
                statePosition += new Vector2(0f, 30f);
           


            }
            
        }
        else return;


        FsmEditor.SetFsmDirty(FsmEditor.SelectedFsm, true);
        state.Position = new Rect(35f, 100f, state.Position.width, state.Position.height);
        FsmEditor.RepaintAll();
        var action = FsmEditor.Builder.AddAction(state, uFrameEditor.FindType(
            string.Format("{0}.PlaymakerActions.{1}", data.Node.Data.Name, data.NameAsEnumCompareClass())));
        var dataProperty = FsmEditor.SelectedFsm.Variables.GetVariable(data.Name);
        action.GetType().GetField("_" + data.Name).SetValue(action,dataProperty);
     
    }

    public static string NameAsEnumCompareClass(this ViewModelPropertyData data)
    {
        return data.Node.Name + data.Name + "Compare";
    }

    public static void AddPropertyFsm(this ViewModelPropertyData data, FsmState fsmState = null)
    {
        var state = fsmState ?? FsmEditor.Builder.AddState(new Vector2(30f, 30f));
        state.Name = "Apply " + data.Name;
        FsmEditor.Builder.SetStartState(state.Name);
        data.AddBindingEventToFsm();
        FsmEditor.Builder.AddGlobalTransition(state, FsmEditor.SelectedFsm.GetEvent(data.NameAsChangedMethod));
        data.AddVariableToFsm();
    }
    public static void CreatePropertyFsm(this ViewModelPropertyData data)
    {
        FsmBuilder.AddFsmToSelected();
        FsmEditor.SelectedFsmComponent.FsmName = data.NameAsChangedMethod;
        AddPropertyFsm(data,FsmEditor.SelectedState);
    }
}