using System;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
public class DiagramContextMenuAttribute : Attribute
{
    public DiagramContextMenuAttribute(string menuPath)
    {
        MenuPath = menuPath;
    }

    public DiagramContextMenuAttribute(string menuPath, int index)
    {
        MenuPath = menuPath;
        Index = index;
    }

    public string MenuPath { get; set; }
    public int Index { get; set; }
}