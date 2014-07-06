using HutongGames.PlayMaker;
using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class PlaymakerFSMBindingSyncExtender : ViewBindingExtender
{
    private Assembly _pmAssembly;

    public ElementData Element { get; set; }

    public Assembly PlaymakerAssembly
    {
        get { return _pmAssembly ?? (_pmAssembly = Assembly.GetAssembly(typeof(PlayMakerFSM))); }
    }

    public override void ExtendPropertyBinding(ElementData element, CodeStatementCollection trueStatements, ViewModelPropertyData property, ElementDataBase relatedElement)
    {
        if (!element["Playmaker"]) return;
        bool isEnum = false;

        var fsmType = property.GetFsmType( element.Data,true, out isEnum);
        if (fsmType == null) return;
        var propertyName = "Fsm" + property.Name;
        var nullCondition = new CodeConditionStatement(new CodeBinaryOperatorExpression(
            new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), propertyName),
            CodeBinaryOperatorType.ValueEquality, new CodeSnippetExpression("null")
            ));
        nullCondition.FalseStatements.Add(new CodeSnippetExpression(string.Format("{0}.Each(v=>v.Value = value{1})", propertyName, isEnum ? ".ToString()" : "")));
        trueStatements.Add(nullCondition);
        trueStatements.Add(new CodeSnippetExpression(string.Format("FSMS.Each(f=>f.Fsm.Event(\"{0}\"))", property.NameAsChangedMethod)));
    }

    /// <summary>
    /// Initializes the binding extender. Must return true if this extender is valid. False if its not valid.
    /// </summary>
    /// <param name="viewClassGenerator"></param>
    /// <returns></returns>
    public override bool Initialize(ViewClassGenerator viewClassGenerator)
    {
        var viewClass = viewClassGenerator as ViewBaseGenerator;
        if (viewClass == null)
        {
            return false;
        }
        if (!viewClass.ElementData["Playmaker"]) return false;
        viewClass.Namespace.Imports.Add(new CodeNamespaceImport("HutongGames.PlayMaker"));
        return viewClass.ElementData["Playmaker"];
    }

    /// <summary>
    /// Extend the ViewBase class. Add properties, methods...etc
    /// </summary>
    /// <param name="decl">The decleration to add to</param>
    /// <param name="elementData"></param>
    public override void ExtendViewBase(CodeTypeDeclaration decl, ElementData elementData)
    {
        Element = elementData;
        if (!Element["Playmaker"]) return;

        var fsmField = new CodeMemberField(typeof(PlayMakerFSM[]), "_FSMS");
        fsmField.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(SerializeField))));
        var fsmProperty = fsmField.EncapsulateField("FSMS",
            new CodeSnippetExpression("this.GetComponents<PlayMakerFSM>()"),new CodeSnippetExpression("_FSMS == null || _FSMS.Length < 1"));
        if (!elementData.IsControllerDerived)
        {
            decl.Members.Add(fsmField);
            decl.Members.Add(fsmProperty);
        }
        foreach (var property in elementData.Properties)
        {
            bool isEnum = false;
            var fsmType = property.GetFsmType( elementData.Data,true, out isEnum);
            if (fsmType != null)
            {
                ViewModelPropertyData property1 = property;
                var def = decl.Members.OfType<CodeMemberField>().FirstOrDefault(p => p.Name == property1.NameAsBindingOption);
                if (def != null) def.CustomAttributes.RemoveAt(def.CustomAttributes.Count - 1);
                var propertyFsmField = new CodeMemberField(fsmType, "_Fsm" + property.Name);
                var fsmPropertyMember = propertyFsmField.EncapsulateField("Fsm" + property.Name,
                    new CodeSnippetExpression(string.Format("{0}.GetVariables<{1}>((fsm) => fsm.FsmVariables.Find{1}(\"{2}\")).ToArray()", fsmProperty.Name,
                        fsmType.Name.Replace("[]", ""), property.Name)));
                //{0}.GetVariables<{1}>((fsm) => fsm.FsmVariables.Find{1}(\"{2}\")).ToArray();
                decl.Members.Add(fsmPropertyMember);
                decl.Members.Add(propertyFsmField);
            }
        }
    }

   
}