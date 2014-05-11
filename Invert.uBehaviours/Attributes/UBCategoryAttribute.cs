using System;

public class UBCategoryAttribute : Attribute
{
    public string Name { get; set; }

    public UBCategoryAttribute(string name)
    {
        Name = name;
    }
}