using System;

public class ElementItemType
{
    public string Group { get; set; }
    public string Label { get; set; }

    public Type Type
    {
        get { return Type.GetType(AssemblyQualifiedName); }
        set { AssemblyQualifiedName = value.AssemblyQualifiedName; }
    }

    public string AssemblyQualifiedName { get; set; }
}