using System.CodeDom;

namespace Invert.uFrame.Code.Bindings
{
    public class CollectionItemRemovedBindingGenerator : CollectionBindingGenerator
    {
        public override string MethodName
        {
            get { return string.Format("{0}Removed", Item.Name); }
        }

        public override void CreateBindingStatement(CodeTypeMemberCollection collection, CodeConditionStatement bindingCondition)
        {
            base.CreateBindingStatement(collection, bindingCondition);
            if (RelatedElement != null)
            {
                bindingCondition.TrueStatements.Add(
                    new CodeSnippetExpression(string.Format("binding.SetRemoveHandler(item=>{0}(item as {1}))",
                        MethodName, RelatedElement.NameAsViewBase)));
            }
            else
            {

                bindingCondition.TrueStatements.Add(
                    new CodeSnippetExpression(string.Format("binding.SetRemoveHandler({0})",
                        MethodName)));
            }

        }

        public override string VarName
        {
            get { return "item"; }
        }

        public override void CreateMembers(CodeTypeMemberCollection collection)
        {
            base.CreateMembers(collection);
            //decl.Members.Add(addHandlerMethod);
            var removeHandlerMethod = CreateMethodSignature(null,
                new CodeParameterDeclarationExpression(ParameterTypeName, VarName));

            if (GenerateDefaultImplementation)
            {
                if (CollectionProperty != null && RelatedElement != null)
                {
                    removeHandlerMethod.Statements.Add(new CodeMethodInvokeExpression(
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), NameAsListField), "Remove",
                        new CodeVariableReferenceExpression("item")));

                    removeHandlerMethod.Statements.Add(new CodeSnippetExpression("if (item != null && item.gameObject != null) UnityEngine.Object.Destroy(item.gameObject)"));
                }
            }
            collection.Add(removeHandlerMethod);
        }
    }
}