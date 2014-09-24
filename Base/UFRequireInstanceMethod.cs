using System;

public class UFRequireInstanceMethod : Attribute
{
    public UFRequireInstanceMethod(string canmovetochanged)
    {
        MethodName = canmovetochanged;
    }

    public string MethodName { get; set; }
}