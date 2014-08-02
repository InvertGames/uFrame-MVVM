using System;
using System.CodeDom;

public static class CodeDomHelpers
{
    public static CodeTypeDeclaration Base(this CodeTypeDeclaration decleration, string baseType)
    {
        decleration.BaseTypes.Add(baseType);
        return decleration;
    }
    public static CodeTypeDeclaration Base(this CodeTypeDeclaration decleration, Type baseType)
    {
        decleration.BaseTypes.Add(baseType);
        return decleration;
    }

    public static CodeMemberMethod Method(this CodeTypeDeclaration decleration,MemberAttributes attributes,Type returnType, string name)
    {
        var method = new CodeMemberMethod() { Name = name, Attributes = attributes };
        decleration.Members.Add(method);
        return method;
    }

    public static CodeTypeDeclaration CreateViewModelProperty(this CodeTypeDeclaration decl, ElementData element)
    {
        var viewModelProperty = new CodeMemberProperty
        {
            Name = element.Name,
            Attributes = MemberAttributes.Public,
            Type = new CodeTypeReference(element.NameAsViewModel),
            HasGet = true,
            HasSet = false
        };

        viewModelProperty.GetStatements.Add(new CodeMethodReturnStatement(
            new CodeCastExpression(viewModelProperty.Type,
                new CodePropertyReferenceExpression(
                    new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "View"), "ViewModelObject"))
            ));
        decl.Members.Add(viewModelProperty);
        return decl;
    }

    //public static CodeMemberMethod Parameter(this CodeMemberMethod method)
    //{
        
    //}

    public static CodeTypeDeclaration Declare(this CodeNamespace ns, MemberAttributes attributes, string name)
    {
        var decl = new CodeTypeDeclaration
        {
            Name =  name,
            Attributes = MemberAttributes.Public
        };
        ns.Types.Add(decl);
        return decl;
    }

    public static CodeTypeDeclaration Field(this CodeTypeDeclaration decleration, Type fieldType, string name, CodeExpression init = null, params CodeAttributeDeclaration[] customAttributes)
    {
        return Field(decleration, MemberAttributes.Private, fieldType, name, init,customAttributes);
    }

    public static CodeTypeDeclaration Field(this CodeTypeDeclaration decleration,MemberAttributes attributes, Type fieldType, string name, CodeExpression init = null,params CodeAttributeDeclaration[] customAttributes )
    {
        var field = new CodeMemberField(fieldType, name) {InitExpression = init,Attributes = attributes};
        if (customAttributes != null)
        field.CustomAttributes.AddRange(customAttributes);
        decleration.Members.Add(field);
        return decleration;
    }
    public static CodeTypeDeclaration EncapsulatedField(this CodeTypeDeclaration decleration, Type fieldType, string name, string propertyName, CodeExpression lazyValue, bool publicField = false)
    {
        var field = new CodeMemberField(fieldType, name);
        if (publicField)
        {
            field.Attributes = MemberAttributes.Public;
        }
        decleration.Members.Add(field);
        decleration.EncapsulateField( field,propertyName, lazyValue);
        return decleration;
    }
    public static CodeTypeDeclaration EncapsulateField(this CodeTypeDeclaration typeDeclaration, CodeMemberField field, string name, CodeExpression lazyValue, CodeExpression lazyCondition = null)
    {
        var p = new CodeMemberProperty
        {
            Name = name,
            Type = field.Type,
            HasGet = true,
            Attributes = MemberAttributes.Public
        };
        typeDeclaration.Members.Add(p);

       var r = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name);
        var lazyConditionStatement = new CodeConditionStatement();
        CodeExpression finalLazyCondition = lazyCondition;
        if (finalLazyCondition == null)
        {
            var defaultConditionStatement =
               new CodeBinaryOperatorExpression(
                   r,
                   CodeBinaryOperatorType.ValueEquality, new CodeSnippetExpression("null"));
            
            finalLazyCondition = defaultConditionStatement;
        }

        lazyConditionStatement.Condition = finalLazyCondition;
        lazyConditionStatement.TrueStatements.Add(new CodeAssignStatement(r, lazyValue));

        p.GetStatements.Add(lazyConditionStatement);
        p.GetStatements.Add(
            new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name)));
        
        return typeDeclaration;
    }

    public static CodeMemberProperty EncapsulateField(this CodeMemberField field, string name)
    {
        return EncapsulateField(field, name,null,null, true);
    }

    public static CodeMemberProperty EncapsulateField(this CodeMemberField field, string name, CodeExpression lazyValue, CodeExpression lazyCondition = null, bool generateSetter = false)
    {
        var p = new CodeMemberProperty
        {
            Name = name,
            Type = field.Type,
            HasGet = true,
            Attributes = MemberAttributes.Public
        };
        var r = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name);

        if (lazyValue != null)
        {
            var lazyConditionStatement = new CodeConditionStatement();
            CodeExpression finalLazyCondition = lazyCondition;

            if (finalLazyCondition == null)
            {
                var defaultConditionStatement =
                   new CodeBinaryOperatorExpression(
                       r,
                       CodeBinaryOperatorType.ValueEquality, new CodeSnippetExpression("null"));

                finalLazyCondition = defaultConditionStatement;
            }

            lazyConditionStatement.Condition = finalLazyCondition;
            lazyConditionStatement.TrueStatements.Add(new CodeAssignStatement(r, lazyValue));
            p.GetStatements.Add(lazyConditionStatement);
        }
       

       
        p.GetStatements.Add(
            new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name)));
        if (generateSetter)
        {
            p.HasSet = true;
            p.SetStatements.Add(new CodeSnippetExpression(string.Format("{0} = value", field.Name)));
        }
        return p;
    }
}