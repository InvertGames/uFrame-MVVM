using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class ClassGenerator
{
    private Dictionary<string, MethodGenerator> _methods = new Dictionary<string, MethodGenerator>();

    private Dictionary<string, string> _variables = new Dictionary<string, string>();

    public string Decleration { get; set; }

    public virtual string Filename
    {
        get
        {
            return Name;
        }
    }

    public string FilePath { get; set; }

    public Dictionary<string, MethodGenerator> Methods
    {
        get { return _methods; }
        set { _methods = value; }
    }

    public string Name { get; set; }

    public Dictionary<string, string> Variables
    {
        get { return _variables; }
        set { _variables = value; }
    }

    protected ClassGenerator(string decleration, string name)
    {
        Decleration = decleration;
        Name = name.Replace("get_", "").Replace("set_", "");
    }

    public MethodGenerator AddMethod(string name, string decleration)
    {
        var mg = new MethodGenerator { Name = name, Decleration = decleration, Body = new StringBuilder(), TabCount = 1 };
        Methods.Add(name, mg);
        return mg;
    }

    public void AddVariable(string name, string decleration)
    {
        if (_variables.ContainsKey(name))
            _variables.Remove(name);
        _variables.Add(name, decleration);
    }

    public string GetTypeVarName(Type t)
    {
        if (t == typeof(int))
            return "UBInt";
        if (t == typeof(float))
            return "UBFloat";
        if (t == typeof(Color))
            return "UBColor";
        if (t == typeof(Quaternion))
            return "UBQuaternion";
        if (t == typeof(Rect))
            return "UBRect";
        if (t == typeof(Texture))
            return "UBTexture";
        if (t == typeof(bool))
            return "UBBool";
        if (t == typeof(Vector2))
            return "UBVector2";
        if (t == typeof(Vector3))
            return "UBVector3";
        if (t == typeof(GameObject))
            return "UBGameObject";
        if (t == typeof(UnityEngine.Object))
            return "UBObject";
        if (t == typeof(Material))
            return "UBMaterial";
        if (t == typeof(Animation))
            return "UBAnimation";
        if (t == typeof(string))
            return "UBString";
        if (t == typeof(Transform))
            return "UBTransform";
        if (t == typeof(Enum))
            return "UBEnum";

        return t.Name;
    }

    public bool IsUbType(Type t)
    {
        return UBGeneratorHelper.AllowedParameterTypes.Contains(t);
    }

    public string ToFile()
    {
        return ToFile(FilePath);
    }

    public string ToFile(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        var fullName = Path.Combine(directory, Filename + ".cs");
        var content = ToString();
        File.WriteAllText(fullName, content);
        return content;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendFormat(Decleration, Filename).Append(" {").AppendLine().AppendLine();
        foreach (var variable in Variables)
            sb.Append("\t").AppendFormat(variable.Value, variable.Key).Append(";").AppendLine();

        foreach (var methodDecl in Methods)
        {
            sb.Append(methodDecl.Value.ToString()).AppendLine();
        }

        sb.Append("}");
        var result = sb.ToString();

        Methods.Clear();
        Variables.Clear();
        return result;
    }

    protected void WriteUbVariableDecleration(Type type, string name, bool isResult = false, string description = null)
    {
        if (type == typeof(void)) return;
        var isresultstring = isResult ? "[UBRequireVariable] " : string.Empty;
        var helpstring = description == null ? "" : string.Format("[UBHelp(@\"{0}\")]{1}", description, Environment.NewLine + "\t");
        if (IsUbType(type))
        {
            AddVariable(Char.ToUpper(name[0]) + name.Substring(1),
                string.Format("{0}[UBRequired] public {1} _{{0}} = new {1}()", helpstring + isresultstring, GetTypeVarName(type)));
        }
        else if (typeof(Enum).IsAssignableFrom(type))
        {
            AddVariable(Char.ToUpper(name[0]) + name.Substring(1),
                string.Format("{0}[UBRequired] public UBEnum _{{0}} = new UBEnum(typeof({1}))", helpstring + isresultstring, GetTypeVarName(type)));
        }
        else
        {
            AddVariable(Char.ToUpper(name[0]) + name.Substring(1),
                string.Format("{0}[UBRequired] public UBObject _{{0}} = new UBObject(typeof({1}))", helpstring + isresultstring, GetTypeVarName(type)));
        }
    }
}