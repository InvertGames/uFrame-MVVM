using System;
using System.Reflection;
using UBehaviours.Actions;

public interface IBehaviourVisitor
{
    void Visit(IUBFieldInfo actionVariable, UBAction container);

    void Visit(IUBVariableDeclare variable);

    void Visit(UBActionSheet actionSheet);

    void Visit(UBAction action);

    void Visit(TriggerInfo triggerInfo);
}