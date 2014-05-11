using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class UBSettingAttribute : Attribute
{

    public UBSettingAttribute(string name, Type settingType, object defaultValue)
    {
        Name = name;
        SettingType = settingType;
        DefaultValue = defaultValue;
    }

    public Type SettingType { get; set; }
    public string Name { get; set; }
    public object DefaultValue { get; set; }
}