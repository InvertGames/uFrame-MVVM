using System;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

//public class ConditionActionBuilder : InstanceMethodActionBuilder
//{
//    public ConditionActionBuilder(MethodInfo methodInfo) :
//        base(methodInfo.Name, "public class {0} : UBConditionAction", methodInfo)
//    {
//    }

//    public override void AddVariables()
//    {
//        base.AddVariables();
//    }

//    protected override void AddDeclaringVariable()
//    {
//        base.AddDeclaringVariable();
//    }

//    protected override void AddResultVariable()
//    {
//        //base.AddResultVariable();
//    }

//    public override MethodGenerator AddExecuteMethod()
//    {
//        return AddMethod("PerformCondition", "public override bool {0}(UBContext context)");
//    }

//    public override void WriteExecuteMethod(MethodGenerator mg)
//    {
//        //base.WriteExecuteMethod(mg);
//        mg.A("return ");
//        WriteExecute(mg.Body);
//        mg.AL(";");
//    }
//}

public class UBGeneratorHelper
{
    public static Type[] AllowedParameterTypes = new Type[]
    {
        typeof(string),
        typeof(Vector3),
        typeof(Quaternion),
        typeof(Material),
        typeof(Color),
        typeof(Vector2),
        typeof(Texture),
        typeof(Animation),
        typeof(bool),
        typeof(Rect),
        typeof(UnityEngine.Object),
        typeof(UnityEngine.GameObject),
        typeof(float),
        typeof(int),
        typeof(Transform),
        typeof(Enum)
    };
}

//public class InstanceMethodActionBuilder : ActionClassGenerator
//{
//    public MethodInfo MethodInfo { get; set; }

//    public InstanceMethodActionBuilder(string name, string decleration, MethodInfo methodInfo)
//        : base(decleration, name)
//    {
//        MethodInfo = methodInfo;
//    }

//    public InstanceMethodActionBuilder(MethodInfo methodInfo)
//        : base("public class {0} : UBAction", methodInfo.Name)
//    {
//        MethodInfo = methodInfo;
//    }

//    //public bool IsUbType(Type t)
//    //{
//    //    return UBGeneratorHelper.AllowedParameterTypes.Contains(t);
//    //}

//    //public string GetValueString(Type t)
//    //{
//    //    return string.Format("_{0}{1}", char.ToUpper(t.Name[0]) + t.Name.Substring(1), IsUbType(t) ? ".GetValue(context)" : string.Empty);
//    //}

//    public string GetParameterVarName(ParameterInfo p)
//    {
//        return string.Format("_{0}{1}", char.ToUpper(p.Name[0]) + p.Name.Substring(1), IsUbType(p.ParameterType) ? ".GetValue(context)" : string.Empty);
//    }

//    public override void WriteExecute(StringBuilder builder)
//    {
//        var parameters = MethodInfo.GetParameters().Select(p => GetValueString(p.ParameterType)).ToArray();
//        builder.Append(MethodInfo.IsStatic ? MethodInfo.DeclaringType.Name : GetValueString(MethodInfo.DeclaringType));
//        builder.Append(".");
//        builder.Append(MethodInfo.Name);
//        builder.Append("(");
//        builder.Append(string.Join(",", parameters));
//        builder.Append(")");
//    }

//    public override void WriteExecuteMethod(MethodGenerator mg)
//    {
//        if (MethodInfo.ReturnType == typeof(void))
//        {
//            mg.Tab();
//            WriteExecute(mg.Body);
//            mg.A(";");
//            return;
//        }
//        mg.Enclose("if (_Result != null)", g =>
//        {
//            g.Tab();
//            g.A("_Result.SetValue( context, ");
//            WriteExecute(g.Body);
//            g.A(");");
//        });
//    }

//    public override void AddActionSheetVariable(string name)
//    {
//        AddVariable(name, "[HideInInspector] public UBActionSheet _{0}");
//    }

//    public override MethodGenerator AddExecuteMethod()
//    {
//        return AddMethod("PerformExecute", "protected override void PerformExecute(UBContext context)");
//        //WriteExecuteMethod(method);
//        //return method;
//    }

//    public override string ToString()
//    {
//        var sb = WriteDecleration();

//        AddDeclaringVariable();
//        AddVariables();
//        AddResultVariable();
//        var method = AddExecuteMethod();
//        method.TabCount++;
//        WriteExecuteMethod(method);
//        method.TabCount--;
//        sb.Append(base.ToString());
//        return sb.ToString();
//    }

//    protected override StringBuilder WriteDecleration()
//    {
//        var sb = new StringBuilder();
//        sb.AppendLine("using System;");
//        sb.AppendLine("using System.Collections.Generic;");
//        sb.AppendLine("using UnityEngine;");
//        sb.AppendLine();
//        sb.AppendFormat("[UBCategory(\"{0}\")]", MethodInfo.DeclaringType.Name).AppendLine();
//        return sb;
//    }

//    protected override void AddResultVariable()
//    {
//        WriteUbVariableDecleration(MethodInfo.ReturnType, "Result", true);
//    }

//    protected override void AddDeclaringVariable()
//    {
//        if (!MethodInfo.IsStatic)
//            WriteUbVariableDecleration(MethodInfo.DeclaringType, MethodInfo.DeclaringType.Name);
//    }

//    public override void AddVariables()
//    {
//        var parameters = MethodInfo.GetParameters();
//        foreach (var parameter in parameters)
//        {
//            WriteUbVariableDecleration(parameter.ParameterType, parameter.Name);
//        }
//    }
//}