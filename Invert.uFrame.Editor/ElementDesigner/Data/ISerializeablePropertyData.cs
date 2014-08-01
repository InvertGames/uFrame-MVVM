using System;

public interface ISerializeablePropertyData
{
    string Name { get; }
    Type Type { get; }
    string RelatedTypeName { get; }
    IDiagramNode TypeNode();
}