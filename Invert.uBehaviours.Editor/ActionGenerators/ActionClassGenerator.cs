using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public abstract class ActionClassGenerator : ClassGenerator
{
    private static Dictionary<string, string> _nameOverrides = new Dictionary<string, string>()
    {
        {typeof(UnityEngine.Transform).GetMethod("Find").ToString(),"FindChildTransform"},
        {typeof(UnityEngine.Vector3).GetMethod("Set").ToString(),"SetVector3"},
    };
    public const BindingFlags METHOD_BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly;
    public const BindingFlags PROPERTY_BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly;


    public string GetName(Type type)
    {

        return this.Filename;
    }

    public static Dictionary<string, string> NameOverrides
    {
        get { return _nameOverrides; }
        set { _nameOverrides = value; }
    }

    protected ActionClassGenerator(string decleration, string name)
        : base(decleration, name)
    {
    }

    public virtual void AddActionSheetVariable(string name)
    {
        AddVariable(name, "[HideInInspector] public UBActionSheet _{0}");
    }

    public virtual MethodGenerator AddExecuteMethod()
    {
        return AddMethod("PerformExecute", "protected override void PerformExecute(IUBContext context)");
        //WriteExecuteMethod(method);
        //return method;
    }

    public abstract void AddVariables();

    public string GetValueString(Type t, string varName = null)
    {
        string name;
        if (varName != null)
            name = char.ToUpper(varName[0]) + varName.Substring(1);
        else name = char.ToUpper(t.Name[0]) + t.Name.Substring(1);
        if (typeof(Enum).IsAssignableFrom(t))
        {
            return string.Format("(({0})_{1}.GetIntValue(context))", t.Name, name);
        }
        if (typeof(UnityEngine.Object).IsAssignableFrom(t) && !IsUbType(t))
        {
            return string.Format("_{0}.GetValueAs<{1}>(context)", name, t.Name);
        }
        return string.Format("_{0}{1}", name, IsUbType(t) ? ".GetValue(context)" : string.Empty);
    }

    public string GetVarName(Type t, string varName)
    {
        string name;
        if (varName != null)
            name = char.ToUpper(varName[0]) + varName.Substring(1);
        else name = char.ToUpper(t.Name[0]) + t.Name.Substring(1);
        if (typeof(Enum).IsAssignableFrom(t))
        {
            return string.Format("(({0})_{1}", t.Name, name);
        }
        if (typeof(UnityEngine.Object).IsAssignableFrom(t) && !IsUbType(t))
        {
            return string.Format("_{0}", name);
        }
        return string.Format("_{0}", name);
    }

    public string GetVarName2(Type t, string varName)
    {
        string name;
        if (varName != null)
            name = char.ToUpper(varName[0]) + varName.Substring(1);
        else name = char.ToUpper(t.Name[0]) + t.Name.Substring(1);

        return string.Format("_{0}", name);
    }

    public override string ToString()
    {
        var sb = WriteDecleration();

        AddDeclaringVariable();
        AddVariables();
        AddResultVariable();

        var method = AddExecuteMethod();
        method.TabCount++;
        WriteExecuteMethod(method);
        method.TabCount--;
        AddMethods();
        sb.Append(base.ToString());
        return sb.ToString();
    }

    public virtual void WriteCodeExecuteString(StringBuilder builder)
    {
        WriteExecute(builder, GetVarName);
    }

    public abstract void WriteExecute(StringBuilder builder, Func<Type, string, string> variableAccessor);

    public abstract void WriteExecuteMethod(MethodGenerator mg);

    protected abstract void AddDeclaringVariable();

    protected virtual void AddMethods()
    {
        var toStringMethod = AddMethod("ToString", "public override string {0}()");
        WriteToStringMethod(toStringMethod);
        var method = AddMethod("WriteCode", "public override void {0}(IUBCSharpGenerator sb)");
        WriteCodeMethod(method);
    }

    protected abstract void WriteToStringMethod(MethodGenerator method);

    protected abstract void AddResultVariable();

    protected abstract void WriteCodeMethod(MethodGenerator method);

    protected abstract StringBuilder WriteDecleration();
}