using System;

public class UFToggleGroup : Attribute
{
    public UFToggleGroup(string checkers)
    {
        Name = checkers;
    }

    public string Name { get; set; }
}