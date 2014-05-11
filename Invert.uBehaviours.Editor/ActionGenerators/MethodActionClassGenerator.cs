using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

public class MethodActionClassGenerator : ActionClassGenerator
{
    public IEnumerable<MethodInfo> AllOverloads
    {
        get
        {
            return MethodInfo.DeclaringType.GetMethods(METHOD_BINDING_FLAGS)
                .Where(p => p.Name == MethodInfo.Name);
        }
    }

    public override string Filename
    {
        get
        {
            if (NameOverrides.ContainsKey(MethodInfo.ToString()))
            {
                return NameOverrides[MethodInfo.ToString()];
            }
            if (AllOverloads.Count() > 1)
            {
                return GetMethodName(MethodInfo);
            }
            return base.Filename;
        }
    }

    public MethodInfo MethodInfo { get; set; }

    public MethodActionClassGenerator(string name, string decleration, MethodInfo methodInfo)
        : base(decleration, name)
    {
        MethodInfo = methodInfo;
    }

    public MethodActionClassGenerator(MethodInfo methodInfo)
        : base("public class {0} : UBAction", methodInfo.Name)
    {
        MethodInfo = methodInfo;
    }

    public static string GetMethodName(MethodInfo info)
    {
        if (info.Name.StartsWith("op_"))
        {
            return info.Name.Replace("op_", "Compare" + info.DeclaringType.Name);
        }
        var parameterNames = info.GetParameters().Select(p => char.ToUpper(p.Name[0]) + p.Name.Substring(1)).ToArray();
        var firstNames = parameterNames.Take(parameterNames.Length - 1).ToArray();

        if (parameterNames.Length > 1)
            return info.Name + "By" + string.Join("", firstNames) + "And" + parameterNames.Last();
        else if (parameterNames.Length == 1)
        {
            return info.Name + "By" + parameterNames.First();
        }
        else
        {
            return info.Name;
        }
    }

    public override void AddVariables()
    {
        var parameters = MethodInfo.GetParameters();
        foreach (var parameter in parameters)
        {
            WriteUbVariableDecleration(parameter.ParameterType, parameter.Name, false, parameter.GetSummary());
        }
    }

    public void GetInstanceValueString()
    {
    }

    public string GetParameterVarName(ParameterInfo p)
    {
        return string.Format("_{0}{1}", char.ToUpper(p.Name[0]) + p.Name.Substring(1), IsUbType(p.ParameterType) ? ".GetValue(context)" : string.Empty);
    }

    public override void WriteExecute(StringBuilder builder, Func<Type, string, string> variableAccessor)
    {
        var parameters = MethodInfo.GetParameters().Select(p => variableAccessor(p.ParameterType, p.Name)).ToArray();
        builder.Append(MethodInfo.IsStatic ? MethodInfo.DeclaringType.Name : variableAccessor(MethodInfo.DeclaringType, null));
        builder.Append(".");
        builder.Append(MethodInfo.Name);
        builder.Append("(");
        builder.Append(string.Join(",", parameters));
        builder.Append(")");
    }

    public override void WriteExecuteMethod(MethodGenerator mg)
    {
        if (MethodInfo.ReturnType == typeof(void))
        {
            mg.Tab();
            WriteExecute(mg.Body, GetValueString);
            mg.A(";");
            return;
        }
        mg.Enclose("if (_Result != null)", g =>
        {
            g.Tab();
            g.A("_Result.SetValue( context, ");
            WriteExecute(g.Body, GetValueString);
            g.A(");");
        });
    }

    protected override void AddDeclaringVariable()
    {
        if (!MethodInfo.IsStatic)
            WriteUbVariableDecleration(MethodInfo.DeclaringType, MethodInfo.DeclaringType.Name);
    }

    protected override void AddMethods()
    {
        base.AddMethods();
    }

    protected override void WriteToStringMethod(MethodGenerator method)
    {
        var parameters = MethodInfo.GetParameters().ToArray();
        var strFormat = "";
        var strFormatArgs = "";
        for (int index = 0; index < parameters.Length; index++)
        {
            var parameterInfo = parameters[index];

            strFormatArgs += ", ";
            if (index > 0 && index < parameters.Length - 1)
            {
                strFormat += ", ";
                
                
            } else if (index > 0)
            {
                strFormat += " and ";
                
            }

            strFormat += "{" + (index + 1).ToString() + "}";
            strFormatArgs += GetVarName2(parameterInfo.ParameterType, parameterInfo.Name) + ".ToString(RootContainer)";

        }

        var call = MethodInfo.IsStatic
            ? "\"" + MethodInfo.DeclaringType.Name + "\""
            : GetVarName2(MethodInfo.DeclaringType, null) + ".ToString(RootContainer)";

        method.AF("return string.Format(\"Call {{0}}'s {0} w/ {1}\", {2});", MethodInfo.Name,strFormat, call + strFormatArgs);
    }

    protected override void AddResultVariable()
    {
        WriteUbVariableDecleration(MethodInfo.ReturnType, "Result", true);
    }

    protected override void WriteCodeMethod(MethodGenerator method)
    {
        var parameters =
            MethodInfo.GetParameters().Select(p => string.Format("#{0}#", GetVarName2(p.ParameterType, p.Name)));
        method.AF("\tsb.AppendExpression(\"{0}.{1}({2})\");", MethodInfo.IsStatic ? MethodInfo.DeclaringType.Name : "#" + GetVarName2(MethodInfo.DeclaringType, null) + "#",
            MethodInfo.Name,
            string.Join(", ", parameters.ToArray())
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
        var description = MethodInfo.GetSummary();
        if (!string.IsNullOrEmpty(description))
        {
            sb.AppendFormat("[UBHelp(@\"{0}\")]", description).AppendLine();
        }
        sb.AppendFormat((string)"[UBCategory(\"{0}\")]", MethodInfo.DeclaringType.Name).AppendLine();
        return sb;
    }
}