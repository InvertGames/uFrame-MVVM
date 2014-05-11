using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class UBGlobals : ScriptableObject
{
    private static UBGlobals _instance;

    [SerializeField, HideInInspector]
    private List<UBVariableDeclare> _declares = new List<UBVariableDeclare>();

    private List<UBVariableBase> _variables;

    public static List<UBVariableBase> CurrentVariables
    {
        get;
        set;
    }

    public static bool HasInstance { get; set; }

    public static UBGlobals Instance
    {
        get
        {
            return _instance;
        }
        set { _instance = value; }
    }

    //private static UBGlobals AddToScene()
    //{
    //}
    

    public List<UBVariableDeclare> Declares
    {
        get { return _declares; }
        set { _declares = value; }
    }

    public IUBContext ParentContext { get { return null; } }

    public List<UBVariableBase> Variables
    {
        get { return _variables; }
        private set { _variables = value; }
    }

    public UBGlobals()
    {
        //if (HasInstance)
        //    throw new Exception("Only one UBGlobals is allowed");
        //else
        //    Instance = this;
    }

    public void ExecuteSheet(UBActionSheet sheet)
    {
        throw new NotImplementedException();
    }

    public UBVariableBase GetVariable(UBVariableBase v)
    {
        return null;
    }

    public UBVariableBase GetVariable(string variable)
    {
        return Variables.FirstOrDefault(p => p.Guid == variable);
    }

    public void Push(IUBContext context)
    {
        
        if (CurrentVariables == null || CurrentVariables.Count < 1)
        {
            if (CurrentVariables == null)
                CurrentVariables = new List<UBVariableBase>();
            foreach (var declare in Declares)
            {
                var asVariable = declare.CreateUBVariable();
                CurrentVariables.Add(asVariable);
            }
        }

        foreach (var v in CurrentVariables)
        {
            if (context.Variables.Contains(v)) continue;
            context.Variables.Add(v);
        }
    }

    public void SetVariable(string variableName, object value)
    {
        var variable = GetVariable(variableName);
        if (variable != null)
            variable.LiteralObjectValue = value;
    }
}