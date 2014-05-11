using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UBehaviours.Actions;
using UnityEngine;

public class UBehaviourCSharpGenerator : IBehaviourVisitor, IUBCSharpGenerator
{
    public CodeCompileUnit Unit = new CodeCompileUnit();

    private UBAction _currentAction;
    private UBActionSheet _currentActionSheet;
    private CodeMemberMethod _currentMethod;

    public UBAction CurrentAction
    {
        get { return _currentAction; }
        set
        {
            _currentAction = value;
            if (value != null)
            {
                CurrentStatements.Add(new CodeCommentStatement(value.ToString()));
            }
        }
    }

    public UBActionSheet CurrentActionSheet
    {
        get { return _currentActionSheet; }
        set
        {
            _currentActionSheet = value;
        }
    }

    public CodeMemberMethod CurrentMethod
    {
        get { return _currentMethod; }
        set
        {
            _currentMethod = value;
            if (value != null)
            {
                CurrentStatements = value.Statements;
            }
        }
    }

    public CodeStatementCollection CurrentStatements { get; set; }

    public CodeTypeDeclaration TypeDecleration { get; set; }

    public IBehaviourVisitable Visitable { get; set; }

    public UBehaviourCSharpGenerator(IBehaviourVisitable visitable)
    {
        Visitable = visitable;
    }

    public void AddField(string name, Type type)
    {
        TypeDecleration.Members.Add(new CodeMemberField(type, name));
    }

    public void AddInterface(Type type)
    {
        TypeDecleration.BaseTypes.Add(type);
    }

    public void Append(string text)
    {
        CurrentStatements.Add(new CodeSnippetExpression(text));
    }

    public void AppendExpression(string expressionTemplate)
    {
        Append(Expression(expressionTemplate));
    }

    public void AppendFormat(string text, params object[] args)
    {
        CurrentStatements.Add(new CodeSnippetExpression(string.Format(text, args)));
    }

    public void AssignStatement(UBVariableBase v, string fieldName, string rightExpression)
    {
        AssignStatement(CurrentAction, v, fieldName, rightExpression);
    }

    public void AssignStatement(UBAction action, UBVariableBase v, string fieldName, string rightExpression)
    {
        var assignStatement = new CodeAssignStatement
        {
            Left = new CodeVariableReferenceExpression(VariableName(action, v, fieldName)),
            Right = new CodeSnippetExpression(rightExpression)
        };
        CurrentStatements.Add(assignStatement);
    }

    public void CallSheetStatement(UBActionSheet sheet)
    {
        CurrentStatements.Add(new CodeSnippetStatement(InvokeTriggerSheet(sheet) + ";"));
    }

    public void ConditionStatement(UBConditionAction ubConditionAction, string conditionExpression, UBActionSheet trueSheet, UBActionSheet falseSheet)
    {
        AssignStatement(ubConditionAction._StoreResult, "_StoreResult", conditionExpression);

        var conditionStatement = new CodeConditionStatement(new CodeVariableReferenceExpression(VariableName(ubConditionAction, ubConditionAction._StoreResult, "_StoreResult")));
        if (trueSheet != null && trueSheet.Actions.Count > 0)
        {
            conditionStatement.TrueStatements.Add(new CodeSnippetExpression(InvokeTriggerSheet(trueSheet)));
        }
        if (falseSheet != null && falseSheet.Actions.Count > 0)
        {
            conditionStatement.FalseStatements.Add(new CodeSnippetExpression(InvokeTriggerSheet(falseSheet)));
        }

        CurrentStatements.Add(conditionStatement);
    }

    public string Expression(string expressionTemplate)
    {
        string strRegex = @"#(\w+)#";
        Regex myRegex = new Regex(strRegex, RegexOptions.None);
        return myRegex.Replace(expressionTemplate, OnExpressionReplacement);
    }

    public string GetActionSheetFunctionName(UBActionSheet actionSheet)
    {
        return GetActionSheetFunctionName(actionSheet.FullName);
    }

    public string GetActionSheetFunctionName(string fullName)
    {
        return fullName.Replace(" ", "");
    }

    public UBVariableBase GetActionVariableByName(string name)
    {
        return CurrentAction.GetType().GetField(name).GetValue(CurrentAction) as UBVariableBase;
    }

    public bool HasField(string name)
    {
        foreach (var member in TypeDecleration.Members.OfType<CodeMemberField>())
        {
            if (member.Name == name) return true;
        }
        return false;
    }

    public string InvokeTemplateSheet(string templateName)
    {
        return string.Format("{0}();", templateName.Replace(" ", ""));
    }

    public string InvokeTriggerSheet(UBActionSheet sheet = null)
    {
        return string.Format("{0}()", sheet == null ? GetActionSheetFunctionName(CurrentActionSheet) : GetActionSheetFunctionName(sheet));
    }

    public string MethodCall(UBAction action)
    {
        return "";
    }

    public override string ToString()
    {
        Unit = new CodeCompileUnit();
        TypeDecleration = new CodeTypeDeclaration("MyBehaviour");
        TypeDecleration.BaseTypes.Add(typeof(MonoBehaviour));

        var ns = new CodeNamespace(typeof(UBAction).Namespace);
        ns.Imports.Add(new CodeNamespaceImport("System"));
        ns.Imports.Add(new CodeNamespaceImport("System.Collections"));
        ns.Imports.Add(new CodeNamespaceImport("UnityEngine"));
        ns.Types.Add(TypeDecleration);

        Unit.Namespaces.Add(ns);
        Visitable.Accept(this);
        var provider = new CSharpCodeProvider();

        var sb = new StringBuilder();
        var tw1 = new IndentedTextWriter(new StringWriter(sb), "    ");
        provider.GenerateCodeFromCompileUnit(Unit, tw1, new CodeGeneratorOptions());
        tw1.Close();

        return sb.ToString();
    }

    public string VariableName(UBVariableBase v, string fieldName)
    {
        return VariableName(CurrentAction, v, fieldName);
    }

    public string VariableName(UBAction action, UBVariableBase v, string fieldName)
    {
        if (v.ValueFrom > 1)
        {
            return v.GetVariableReferenceString();
        }
        if (v.ValueFrom == 1)
        {
            return CreateVarName(v.GetReferenceDeclare(action, Visitable as UBSharedBehaviour).Name);
        }
        var index = Array.IndexOf(action.ActionSheet.Actions.Where(p => p != null && p.GetType() == action.GetType()).ToArray(), action);
        return string.Join("_", action.GetPath().Select(p => p.Name.Replace(" ", "")).ToArray()) + fieldName + (index == 0 ? string.Empty : index.ToString());
    }

    public void Visit(IUBFieldInfo actionVariable, UBAction container)
    {
    }

    public void Visit(IUBVariableDeclare variable)
    {
        if (HasField(variable.Name)) return;

        var field = new CodeMemberField(variable.ValueType, variable.Name)
        {
            Attributes = variable is UBVariableDeclare && ((UBVariableDeclare)variable)._Expose ? MemberAttributes.Public : MemberAttributes.Private,
            InitExpression = GetUBVariableInitExpression(variable.ValueType, variable.DefaultValue)
        };
        TypeDecleration.Members.Add(field);
    }

    public void Visit(UBActionSheet actionSheet)
    {
        CurrentActionSheet = actionSheet;
        if (!string.IsNullOrEmpty(actionSheet.TriggerType))
        {
            DoTrigger(Type.GetType(actionSheet.TriggerType), actionSheet, null);
        }
        CurrentMethod = EnsureMethod(actionSheet.Name.Replace(" ", ""));
        CurrentMethod.Comments.Add(new CodeCommentStatement(actionSheet.FullName, true));
    }

    public void Visit(UBAction action)
    {
        CurrentAction = action;
        var variables = action.GetVariableFields();

        foreach (var variable in variables)
        {
            var value = variable.GetValue(action) as UBVariableBase;
            if (value == null) continue;
            var variableDeclare = GetUBVariableDeclerationStatement(action, value, variable.Name);
            if (variableDeclare == null) continue;
            CurrentStatements.Add(variableDeclare);
        }

        // If we have a generator for this type use it instead
        if (UBCodeGenerator.Generators.ContainsKey(action.GetType()))
        {
            var generator = UBCodeGenerator.Generators[action.GetType()];
            generator.Generate(this);
            return;
        }
        DoAction(action);
    }

    public void Visit(TriggerInfo triggerInfo)
    {
        CurrentActionSheet = triggerInfo.Sheet;
        DoTrigger(triggerInfo.TriggerType, triggerInfo.Sheet, triggerInfo);
    }

    private string CreateActionVarName(string arg1)
    {
        var name = arg1.Replace("Name", "");
        var item = CurrentAction.GetType().GetField(name).GetValue(CurrentAction);
        return VariableName(CurrentAction, item as UBVariableBase, name);
    }

    private object[] CreateParameters(MethodInfo codeMethod, Func<string, string> varNameCreator, params object[] additionalParameters)
    {
        var list = new List<object>();
        foreach (var parameter in codeMethod.GetParameters())
        {
            if (parameter.ParameterType == typeof(string) && parameter.Name.EndsWith("Name"))
            {
                list.Add(varNameCreator(parameter.Name.Replace("Name", "")));
            }
            else if (parameter.ParameterType == typeof(IUBCSharpGenerator))
            {
                list.Add(this);
            }
            else
            {
                list.Add(additionalParameters.FirstOrDefault(p => p != null && parameter.ParameterType.IsAssignableFrom(p.GetType())));
            }
        }
        return list.ToArray();
    }

    private string CreateVarName(string replace)
    {
        return replace.Replace("Name", "");
    }

    private void DoAction(UBAction action)
    {
        var actionType = action.GetType();

        var mainCodeGeneratorMethod = FindCodeMethod(actionType, "WriteCode", BindingFlags.Instance | BindingFlags.Public);

        if (mainCodeGeneratorMethod == null) return;
        mainCodeGeneratorMethod.Invoke(action, CreateParameters(mainCodeGeneratorMethod, CreateVarName, action));

        var methods = actionType.GetMethods(BindingFlags.Public | BindingFlags.Instance).ToArray();

        // Create any additional methods the action requires
        foreach (var method in methods)
        {
            var firstParameter = method.GetParameters().FirstOrDefault();
            if (firstParameter != null && firstParameter.ParameterType == typeof(IUBCSharpGenerator)) continue;

            var codeGeneratorMethod = FindCodeMethod(actionType, method.Name);
            if (codeGeneratorMethod == null) continue;

            var parameters = method.GetParameters().Select(p => new CodeParameterDeclarationExpression(p.ParameterType, p.Name));
            var codeMethod = EnsureMethod(method.Name);
            codeMethod.Parameters.AddRange(parameters.ToArray());
            CurrentMethod = codeMethod;
            codeGeneratorMethod.Invoke(null, CreateParameters(codeGeneratorMethod, CreateVarName, action));
        }
    }

    private void DoTrigger(Type triggerType, UBActionSheet sheet, TriggerInfo data)
    {
        var methods = triggerType.GetMethods(BindingFlags.Public | BindingFlags.Instance).ToArray();
        foreach (var method in methods)
        {
            var codeGeneratorMethod = FindCodeMethod(triggerType, method.Name);
            if (codeGeneratorMethod == null) continue;

            var codeMethod = EnsureMethod(method.Name);

            CurrentMethod = codeMethod;

            if (sheet.IsForward)
            {
                CallSheetStatement(sheet.ForwardTo.Sheet);
            }
            else
            {
                if (codeMethod.Parameters.Count < 1)
                {
                    var parameters = method.GetParameters().Select(p => new CodeParameterDeclarationExpression(p.ParameterType, p.Name));
                    codeMethod.Parameters.AddRange(parameters.ToArray());
                }
                codeGeneratorMethod.Invoke(null, CreateParameters(codeGeneratorMethod, CreateVarName, data, Visitable as UBSharedBehaviour));
            }
        }
    }

    private CodeMemberMethod EnsureMethod(string name)
    {
        foreach (var method in TypeDecleration.Members.OfType<CodeMemberMethod>())
        {
            if (method.Name == name)
            {
                return method;
            }
        }
        var m = new CodeMemberMethod()
        {
            Name = name
        };
        TypeDecleration.Members.Add(m);
        return m;
    }

    private MethodInfo FindCodeMethod(Type triggerType, string name, BindingFlags flags = BindingFlags.Public | BindingFlags.Static)
    {
        var methods = triggerType.GetMethods(flags).ToArray();
        foreach (var method in methods)
        {
            var firstParameter = method.GetParameters().FirstOrDefault();
            if (firstParameter == null) continue;

            if (firstParameter.ParameterType == typeof(IUBCSharpGenerator) && method.Name == name) return method;
        }
        return null;
    }

    private CodeVariableDeclarationStatement GetUBVariableDeclerationStatement(UBAction action, UBVariableBase value, string name)
    {
        if (value.ValueFrom != 0) return null;

        var cvds = new CodeVariableDeclarationStatement(value.ValueType, VariableName(action, value, name))
        {
            InitExpression = GetUBVariableInitExpression(value, name)
        };
        return cvds;
    }

    private CodeExpression GetUBVariableInitExpression(Type valueType, object value)
    {
        if (valueType.IsPrimitive || valueType == typeof(string))
        {
            return new CodePrimitiveExpression(value);
        }
        else if (typeof(Vector2) == valueType)
        {
            if (value == null) return null;
            var v = (Vector2)(value ?? Vector2.zero);
            var a = new CodeObjectCreateExpression(typeof(Vector2),
                new CodePrimitiveExpression(v.x),
                new CodePrimitiveExpression(v.y)
                );
            return a;
        }
        else if (typeof(Vector3) == valueType)
        {
            if (value == null) return null;
            var v = (Vector3)(value ?? Vector3.zero);
            var a = new CodeObjectCreateExpression(typeof(Vector3),
                new CodePrimitiveExpression(v.x),
                new CodePrimitiveExpression(v.y),
                new CodePrimitiveExpression(v.z)
                );
            return a;
        }
        else if (typeof(Color) == valueType)
        {
            if (value == null) return null;
            var v = (Color)(value ?? Color.black);
            var a = new CodeObjectCreateExpression(typeof(Color),
                new CodePrimitiveExpression(v.r),
                new CodePrimitiveExpression(v.g),
                new CodePrimitiveExpression(v.b),
                new CodePrimitiveExpression(v.a)
                );
            return a;
        }
        else if (typeof(Enum).IsAssignableFrom(valueType))
        {
            return new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(valueType.Name),
                Enum.GetName(valueType, value).ToString());
        }
        return null;
    }

    private CodeExpression GetUBVariableInitExpression(UBVariableBase variable, string name)
    {
        if (variable.ValueFrom != 0)
        {
            if (variable.ValueFrom == 1)
            {
                return new CodeVariableReferenceExpression(VariableName(variable, name));
            }
        }

        return GetUBVariableInitExpression(variable.ValueType, variable.LiteralObjectValue);
    }

    private string OnExpressionReplacement(Match v)
    {
        var variableName = v.Groups[1].Value;
        var variable = GetActionVariableByName(variableName);
        return VariableName(CurrentAction, variable, variableName);
    }
}