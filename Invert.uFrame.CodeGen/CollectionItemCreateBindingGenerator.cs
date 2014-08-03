using System.CodeDom;
using Invert.uFrame.Editor;

namespace Invert.uFrame.Code.Bindings
{
    public class CollectionItemCreateBindingGenerator : CollectionBindingGenerator
    {
        public override bool IsApplicable
        {
            get { return base.IsApplicable && RelatedElement != null; }
        }
        public override string MethodName
        {
            get { return string.Format("Create{0}View", Item.Name); }
        }
        public override void CreateBindingStatement(CodeTypeMemberCollection collection, CodeConditionStatement bindingCondition)
        {
            base.CreateBindingStatement(collection, bindingCondition);

            bindingCondition.TrueStatements.Add(
                new CodeSnippetExpression(
                    string.Format("binding.SetCreateHandler(viewModel=>{{ return {0}(viewModel as {1}); }}); ",
                        MethodName, RelatedElement.NameAsViewModel)));
        }

        public override void CreateMembers(CodeTypeMemberCollection collection)
        {
            var createHandlerMethod = CreateMethodSignature(new CodeTypeReference(uFrameEditor.uFrameTypes.ViewBase),
                new CodeParameterDeclarationExpression(VarTypeName, VarName));

            if (GenerateDefaultImplementation)
            {
                createHandlerMethod.Statements.Clear();
                createHandlerMethod.Statements.Add(
                    new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(),
                        "InstantiateView", new CodeVariableReferenceExpression(VarName))));
            }
            else if (!IsOverride)
            {

                createHandlerMethod.Statements.Add(
                   new CodeMethodReturnStatement(new CodeSnippetExpression("null")));
            }
            collection.Add(createHandlerMethod);
        }
    }
}