using System;

public interface IDiagramFilter
{
    bool ImportedOnly { get; }

    FilterLocations Locations { get; set; }
    FilterCollapsedDictionary CollapsedValues { get; set; }
    string Name { get; }

    bool IsAllowed(object item, Type t);
    bool IsItemAllowed(object item, Type t);
}