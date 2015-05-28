using System;

[AttributeUsage(AttributeTargets.Class)]
public class DiagramInfoAttribute : Attribute
{
    public DiagramInfoAttribute(string diagramName)
    {
        DiagramName = diagramName;
    }

    public string DiagramName { get; set; }
}