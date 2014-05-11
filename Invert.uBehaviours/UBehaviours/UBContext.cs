//using System;
//using System.Collections.Generic;
//using System.Linq;

//using UnityEngine;

//[Serializable]
//public class UBContext : IUBContext
//{
//    private Stack<IContextItem> _stackTrace = new Stack<IContextItem>();
//    private List<UBVariableBase> _variables;

//    public UBInstanceBehaviour Behaviour
//    {
//        get { return ParentContext.Behaviour; }
//        set { }
//    }

//    public IUBContext ParentContext { get; set; }

//    public Stack<IContextItem> StackTrace
//    {
//        get { return _stackTrace; }
//        set { _stackTrace = value; }
//    }

//    public List<UBVariableBase> Variables
//    {
//        get { return _variables ?? (_variables = new List<UBVariableBase>()); }
//        set { _variables = value; }
//    }

//    public virtual void Awake()
//    {
//        if (ParentContext == null)
//        {
//            ParentContext = ParentContext;
//        }
//    }

//    public void ExecuteSheet(UBActionSheet sheet)
//    {
//        ParentContext.ExecuteSheet(sheet);
//    }

//    public UBVariableBase GetVariable(UBVariableBase v)
//    {
//        foreach (UBVariableBase p in Variables)
//        {
//            if (p == v) return p;
//        }
//        return null;
//    }

//    public UBVariableBase GetVariable(string name)
//    {
//        return ParentContext.GetVariable(name);
//    }

//    public UBVariableBase GetVariableById(string id)
//    {
//        IUBContext ctx = this;
//        while (ctx != null)
//        {
//            var v = ctx.Variables.FirstOrDefault(p => p.Guid == id);
//            if (v != null)
//                return v;
//            ctx = ctx.ParentContext;
//        }
//        return null;
//    }

//    public void GoTo(string triggerId)
//    {
//        ParentContext.GoTo(triggerId);
//    }

//    public T GetVariableAs<T>(string name)
//    {
//        throw new NotImplementedException();
//    }

//    public void SetVariable(string variableName, object value)
//    {
//        throw new NotImplementedException();
//    }

//    public void SetVariableById(string id, object value)
//    {
//        IUBContext ctx = this;
//        while (ctx != null)
//        {
//            var v = ctx.GetVariableById(id);
//            if (v != null)
//            {
//                v.LiteralObjectValue = value;
//            }
//            ctx = ctx.ParentContext;
//        }
//    }

//    public void SetVariable(UBVariableBase variableDeclare)
//    {
//        throw new NotImplementedException();
//    }
//}