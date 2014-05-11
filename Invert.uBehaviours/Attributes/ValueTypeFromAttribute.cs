using System;

public class ValueTypeFromAttribute : Attribute
{
    public string FieldName { get; set; }

    public ValueTypeFromAttribute(string fieldName)
    {
        FieldName = fieldName;
    }
}