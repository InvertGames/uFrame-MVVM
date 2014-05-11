using System.Linq;
using System.Reflection;
using System.Text;

public class ConditionMethodActionBuilder : MethodActionClassGenerator
{
    public ConditionMethodActionBuilder(MethodInfo methodInfo) :
        base(methodInfo.Name, "public class {0} : UBConditionAction", methodInfo)
    {
    }

    public override MethodGenerator AddExecuteMethod()
    {
        return AddMethod("PerformCondition", "public override bool {0}(IUBContext context)");
    }

    public override void AddVariables()
    {
        base.AddVariables();
    }

    public override void WriteExecuteMethod(MethodGenerator mg)
    {
        //base.WriteExecuteMethod(mg);
        mg.A("return ");
        WriteExecute(mg.Body, GetValueString);
        mg.AL(";");
    }

    protected override void AddDeclaringVariable()
    {
        base.AddDeclaringVariable();
    }

    protected override void AddMethods()
    {
        var method = AddMethod("WriteExpressionCode", "public override string {0}(IUBCSharpGenerator sb)");
        WriteCodeMethod(method);
    }

    protected override void WriteToStringMethod(MethodGenerator method)
    {
        base.WriteToStringMethod(method);
    }

    protected override void AddResultVariable()
    {
        //base.AddResultVariable();
    }

    protected override void WriteCodeMethod(MethodGenerator method)
    {
        var parameters =
            MethodInfo.GetParameters().Select(p => string.Format("#{0}#", GetVarName2(p.ParameterType, p.Name)));
        method.AF("\treturn sb.Expression(\"{0}.{1}({2})\");", MethodInfo.IsStatic ? MethodInfo.DeclaringType.Name : "#" + GetVarName2(MethodInfo.DeclaringType, null) + "#",
            MethodInfo.Name,
            string.Join(", ", parameters.ToArray())
            );
    }
}