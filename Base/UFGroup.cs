using System;

public class UFGroup : Attribute
{
    public UFGroup(string viewModelProperties)
    {
        Name = viewModelProperties;
    }

    public string Name { get; set; }
}