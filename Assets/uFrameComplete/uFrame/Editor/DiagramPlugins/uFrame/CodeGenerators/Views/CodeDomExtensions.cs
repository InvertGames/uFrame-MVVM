using System.CodeDom;

public static class CodeDomExtensions
{
    //public static CodeMemberProperty EncapsulateField(this CodeMemberField field, string name, CodeExpression lazyValue)
    //{

    //    var p = new CodeMemberProperty
    //    {
    //        Name = name,
    //        Type = field.Type,
    //        HasGet = true
    //    };
    //    var r = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name);
    //    var lazyCondition =
    //        new CodeConditionStatement(
    //            new CodeBinaryOperatorExpression(
    //                r,
    //                CodeBinaryOperatorType.ValueEquality, new CodeSnippetExpression("null")));

    //    lazyCondition.TrueStatements.Add(new CodeAssignStatement(r, lazyValue));

    //    p.GetStatements.Add(lazyCondition);
    //    p.GetStatements.Add(
    //        new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name)));
    //    return p;
    //}
}