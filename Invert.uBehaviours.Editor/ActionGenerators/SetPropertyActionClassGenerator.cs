using System;
using System.Reflection;
using System.Text;

public class SetPropertyActionClassGenerator : ActionClassGenerator
{
    public override string Filename
    {
        get
        {
            if (NameOverrides.ContainsKey(PropertyInfo.ToString()))
            {
                return NameOverrides[PropertyInfo.ToString()];
            }
            return string.Format("Set{0}", PropertyInfo.DeclaringType.Name + char.ToUpper(Name[0]) + Name.Substring(1));
        }
    }

    public bool IsStatic
    {
        get
        {
            return (PropertyInfo.CanRead && PropertyInfo.GetGetMethod().IsStatic) || (PropertyInfo.CanWrite && PropertyInfo.GetSetMethod().IsStatic);
        }
    }

    public PropertyInfo PropertyInfo { get; set; }

    public SetPropertyActionClassGenerator(string name, string decleration, PropertyInfo propertyInfo)
        : base(decleration, name)
    {
        PropertyInfo = propertyInfo;
    }

    public SetPropertyActionClassGenerator(PropertyInfo propertyInfo)
        : base("public class {0} : UBAction", propertyInfo.Name)
    {
        PropertyInfo = propertyInfo;
    }

    public override void AddVariables()
    {
    }

    public string GetParameterVarName(ParameterInfo p)
    {
        return string.Format("_{0}{1}", char.ToUpper(p.Name[0]) + p.Name.Substring(1), IsUbType(p.ParameterType) ? ".GetValue(context)" : string.Empty);
    }

    public override void WriteCodeExecuteString(StringBuilder builder)
    {
    }

    public override void WriteExecute(StringBuilder builder, Func<Type, string, string> variableAccessor)
    {
        builder.Append(IsStatic ? PropertyInfo.DeclaringType.Name : variableAccessor(PropertyInfo.DeclaringType, null));
        builder.Append(".");
        builder.Append(PropertyInfo.Name);
    }

    public override void WriteExecuteMethod(MethodGenerator mg)
    {
        mg.Enclose("if (_Value != null)", g =>
        {
            g.Tab();

            WriteExecute(g.Body, GetValueString);
            g.A(" = ");
            g.A(GetValueString(PropertyInfo.PropertyType, "Value"));
            g.A(";");
        });
    }

    protected override void AddDeclaringVariable()
    {
        if (!IsStatic)
            WriteUbVariableDecleration(PropertyInfo.DeclaringType, PropertyInfo.DeclaringType.Name);
    }

    protected override void WriteToStringMethod(MethodGenerator method)
    {
        method.AF("return string.Format(\"Set {{0}}'s {0} to {{1}}\", {1}, _Value.ToString(RootContainer));", PropertyInfo.Name,
            IsStatic ? "\"" + PropertyInfo.DeclaringType.Name + "\"" : GetVarName2(PropertyInfo.DeclaringType, null) + ".ToString(RootContainer)");
    }

    protected override void AddResultVariable()
    {
        WriteUbVariableDecleration(PropertyInfo.PropertyType, "Value");
    }

    protected override void WriteCodeMethod(MethodGenerator method)
    {
        method.AF("\tsb.AppendExpression(\"{0}.{1} = #_Value# \");", IsStatic ? PropertyInfo.DeclaringType.Name : "#" + GetVarName2(PropertyInfo.DeclaringType, null) + "#",
         PropertyInfo.Name
         );
    }

    protected override StringBuilder WriteDecleration()
    {
        var sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine("using UBehaviours.Actions;");
        sb.AppendLine();

        //var memberInfo = DocsByReflection.XMLFromMember(MethodInfo)["Summary"];
        var description = PropertyInfo.GetSummary();
        if (!string.IsNullOrEmpty(description))
        {
            sb.AppendFormat("[UBHelp(@\"{0}\")]", description).AppendLine();
        }
        sb.AppendFormat((string)"[UBCategory(\"{0}\")]", PropertyInfo.DeclaringType.Name).AppendLine();
        return sb;
    }
}