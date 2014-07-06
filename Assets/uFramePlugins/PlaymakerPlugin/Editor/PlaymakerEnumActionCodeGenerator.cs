using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HutongGames.PlayMaker;
using Invert.uFrame.Editor;

public class PlaymakerEnumActionCodeGenerator : CodeGenerator
{
    public CodeMemberField ViewModelPropertyField { get; set; }
    public override void Initialize(CodeFileGenerator fileGenerator)
    {
        base.Initialize(fileGenerator);
        Namespace.Name = string.Format("{0}.PlaymakerActions", EnumData.Data.Name);
        Namespace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
        Namespace.Imports.Add(new CodeNamespaceImport("HutongGames.PlayMaker"));
        Decleration = new CodeTypeDeclaration(ClassName)
        {
            TypeAttributes = TypeAttributes.Public
        }; 
        Decleration.BaseTypes.Add("FsmStateAction");

        ViewModelPropertyField = new CodeMemberField(typeof(FsmString), "_" + PropertyData.Name)
        {
            Attributes = MemberAttributes.Public
        };

        Decleration.Members.Add(ViewModelPropertyField);
        Decleration.Members.AddRange(CreateEventFields().ToArray());
        Decleration.Members.Add(CreateUpdateMethod());
        Namespace.Types.Add(Decleration);

    }

    public CodeTypeDeclaration Decleration { get; set; }

    private IEnumerable<CodeTypeMember> CreateEventFields()
    {
        foreach (var item in EnumData.Items)
        {
            yield return new CodeMemberField(typeof(FsmEvent),string.Format("_{0}Event", item.Name))
            {
                Attributes = MemberAttributes.Public,
                InitExpression = new CodeObjectCreateExpression(typeof(FsmEvent),new CodePrimitiveExpression(item.Name))
            };
        }
    }

    private CodeMemberMethod CreateUpdateMethod()
    {
        var updateMethod = new CodeMemberMethod()
        {
            Name = "OnUpdate",
            Attributes = MemberAttributes.Public | MemberAttributes.Override
        };

        updateMethod.Statements.Add(new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), "OnUpdate"));

        foreach (var item in EnumData.Items)
        {
            var conditionExpression =
                new CodeConditionStatement(new CodeSnippetExpression(
                    string.Format("{0}.Value == {1}.{2}.ToString()", 
                        ViewModelPropertyField.Name, EnumData.Name, item.Name)));

            conditionExpression.TrueStatements.Add(new CodeSnippetExpression(string.Format("Fsm.Event(_{0}Event)",
                item.Name)));

            conditionExpression.TrueStatements.Add(new CodeMethodReturnStatement());
            updateMethod.Statements.Add(conditionExpression);
        }
        return updateMethod;
    }


    public string ClassName { get; set; }
    public ViewModelPropertyData PropertyData { get; set; }
    public EnumData EnumData { get; set; }
}