using System.CodeDom;
using HutongGames.PlayMaker;
using Invert.uFrame.Editor;

public class PlaymakerActionCodeGenerator : ViewClassGenerator
{
    public ViewModelCommandData CommandData { get; set; }
   
    public ElementData ElementData { get; set; }

    public override void Initialize(CodeFileGenerator fileGenerator)
    {
        base.Initialize(fileGenerator);
        Namespace.Name = DiagramData.Name + ".PlaymakerActions";
        Namespace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
        Namespace.Imports.Add(new CodeNamespaceImport("HutongGames.PlayMaker"));


        var classDecleration = Namespace.Declare(MemberAttributes.Public, "Execute" + ElementData.Name + CommandData.Name).Base(typeof(FsmStateAction))
            .CreateViewModelProperty(ElementData)
            .EncapsulatedField(typeof (ViewBase),"_view", "View",
                new CodeSnippetExpression("this.Owner.GetComponent<ViewBase>()"),true);
        ElementDataBase relatedElement;
        var executeMethod = CreateExecuteMethod(ElementData, true, CommandData,out relatedElement);
        var methodCall = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(),
            CommandData.NameAsExecuteMethod);

        if (relatedElement == null)
        {
            if (CommandData.RelatedTypeName != null)
            {
                bool isEnum = false;
                classDecleration.Field(MemberAttributes.Public, CommandData.GetFsmType(DiagramData, false, out isEnum), "_Arg", null, new CodeAttributeDeclaration(new CodeTypeReference(typeof(HutongGames.PlayMaker.RequiredFieldAttribute))));
                methodCall.Parameters.Add(new CodeSnippetExpression("_Arg.Value"));
            }
        }
        else
        {
            classDecleration.Field(MemberAttributes.Public, typeof (ViewBase), "_Arg",null,new CodeAttributeDeclaration(new CodeTypeReference(typeof(HutongGames.PlayMaker.RequiredFieldAttribute))));
            methodCall.Parameters.Add(new CodeSnippetExpression(string.Format("_Arg.ViewModelObject as {0}", relatedElement.NameAsViewModel)));
        }

        classDecleration.Members.Add(executeMethod);
        var onenter = classDecleration.Method(MemberAttributes.Public | MemberAttributes.Override, typeof (void),
            "OnEnter");
        onenter.Statements.Add(new CodeSnippetExpression("base.OnEnter();"));
        
        onenter.Statements.Add(methodCall);
        onenter.Statements.Add(new CodeSnippetExpression("Finish()"));
    }
}