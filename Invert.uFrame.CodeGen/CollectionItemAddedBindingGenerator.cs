using System.CodeDom;

namespace Invert.uFrame.Code.Bindings
{
    public class CollectionItemAddedBindingGenerator : CollectionBindingGenerator
    {
        public override string VarName
        {
            get { return "item"; }
        }

        public override string MethodName
        {
            get { return string.Format("{0}Added", Item.Name); }
        }

        public override void CreateBindingStatement(CodeTypeMemberCollection collection, CodeConditionStatement bindingCondition)
        {
            base.CreateBindingStatement(collection, bindingCondition);

            if (RelatedElement != null)
            {
                bindingCondition.TrueStatements.Add(
                    new CodeSnippetExpression(string.Format("binding.SetAddHandler(item=>{0}(item as {1}))",
                        MethodName, RelatedElement.NameAsViewBase)));
            }
            else
            {
                bindingCondition.TrueStatements.Add(
                    new CodeSnippetExpression(string.Format("binding.SetAddHandler({0})",
                        MethodName)));
            }
        }

        public override void CreateMembers(CodeTypeMemberCollection collection)
        {
            base.CreateMembers(collection);
            var addHandlerMethod = CreateMethodSignature(null, new CodeParameterDeclarationExpression(ParameterTypeName, VarName));
            if (GenerateDefaultImplementation && RelatedElement != null)
            {
                addHandlerMethod.Statements.Add(new CodeMethodInvokeExpression(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), NameAsListField), "Add",
                    new CodeVariableReferenceExpression("item")));
            }

            collection.Add(addHandlerMethod);
        }
    }
}