using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UBInclude
{
    [SerializeField]
    private UBSharedBehaviour _behaviour;

    [SerializeField]
    private List<UBVariableDeclare> _declares = new List<UBVariableDeclare>();

    public UBSharedBehaviour Behaviour
    {
        get { return _behaviour; }
        set { _behaviour = value; }
    }

    public List<UBVariableDeclare> Declares
    {
        get { return _declares; }
        set { _declares = value; }
    }

    public void Initialize(IUBContext instance)
    {
        if (Behaviour == null) return;  
        Behaviour.Initialize(instance);
        Sync();
        instance.Variables.AddRange(Declares.ToArray());
        
        if (Behaviour.Globals == null) return;
        Behaviour.Globals.Push(instance);
    }

    public void Sync()
    {
        if (Behaviour == null) return;
        foreach (var declare in Behaviour.Declares.Where(p=>p._Expose))
        {
            var found = Declares.FirstOrDefault(p => p.Guid == declare.Guid);
            if (found != null)
            {
                found.Name = declare.Name;
                found.EnumType = declare.ValueType;
                found.ObjectType = declare.ValueType;
                found.VarType = declare.VarType; 
            }
            else
            {
                var newDeclare = declare.CreateAsDeclare();
                Declares.Add(newDeclare);
            }
        }
        Declares.RemoveAll(p => Behaviour.Declares.All(x => x.Guid != p.Guid));
    }
}