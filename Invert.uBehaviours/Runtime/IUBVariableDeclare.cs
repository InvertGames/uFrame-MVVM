using System;

public interface IUBVariableDeclare : IBehaviourVisitable
{
    string Guid { get; }

    string Name { get; }

    object DefaultValue { get; }

    Type ValueType { get; }

    Type EnumType { get; }

    Type ObjectType { get; }
}