using System.Linq;
using System.Reflection;
using System.Text;

public class ConditionPropertyActionBuilder : GetPropertyActionClassGenerator
{
    public ConditionPropertyActionBuilder(PropertyInfo propertyInfo) :
        base(propertyInfo.Name, "public class {0} : UBConditionAction", propertyInfo)
    {
    }

    public override void AddVariables()
    {
        base.AddVariables();
    }
    public override MethodGenerator AddExecuteMethod()
    {
        return AddMethod("PerformCondition", "public override bool {0}(IUBContext context)");
    }

    public override void WriteCodeExecuteString(StringBuilder builder)
    {
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
        throw new System.NotImplementedException();
    }

    protected override void AddResultVariable()
    {
        //base.AddResultVariable();
    }

    protected override void WriteCodeMethod(MethodGenerator method)
    {
        method.AF("\treturn sb.Expression(\"{0}.{1}\");", IsStatic ? PropertyInfo.DeclaringType.Name : "#" + GetVarName2(PropertyInfo.DeclaringType, null) + "#", PropertyInfo.Name);
    }
}