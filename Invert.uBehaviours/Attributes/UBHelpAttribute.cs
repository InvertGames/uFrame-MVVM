using System;

public class UBHelpAttribute : Attribute
{
    private readonly string _text;

    public string Text
    {
        get { return _text; }
    }

    public UBHelpAttribute(string text)
    {
        _text = text;
    }
}