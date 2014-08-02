using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Invert.uFrame.Code.Bindings
{
    public abstract class CollectionBindingGenerator : BindingGenerator
    {
        public string NameAsListField
        {
            get { return string.Format("_{0}List", Item.Name); }
        }
        public string NameAsSceneFirstField
        {
            get { return string.Format("_{0}SceneFirst", Item.Name); }
        }
        public string NameAsContainerField
        {
            get { return string.Format("_{0}Container", Item.Name); }
        }
        public override bool IsApplicable
        {
            get { return CollectionProperty != null; }
        }

        public ViewModelCollectionData CollectionProperty
        {
            get
            {
                return Item as ViewModelCollectionData;
            }
        }

        public bool HasField(CodeTypeMemberCollection collection, string name)
        {
            return collection.OfType<CodeMemberField>().Any(item => item.Name == name);
        }
        public override void CreateMembers(CodeTypeMemberCollection collection)
        {
            base.CreateMembers(collection);


        }

        public override void CreateBindingStatement(CodeTypeMemberCollection collection, CodeConditionStatement bindingCondition)
        {
            if (bindingCondition.TrueStatements.Count > 0) return;
            if (RelatedElement != null && GenerateDefaultImplementation)
            {
                if (!HasField(collection, NameAsListField))
                {
                    var listField = CreateBindingField(typeof(List<ViewModel>).FullName.Replace("ViewModel", RelatedElement.NameAsViewBase), CollectionProperty.Name, "List", true);
                    collection.Add(listField);
                }

                if (!HasField(collection, NameAsSceneFirstField))
                {
                    var sceneFirstField = CreateBindingField(typeof(bool).FullName, CollectionProperty.Name,
                        "SceneFirst");
                    collection.Add(sceneFirstField);
                }
                if (!HasField(collection, NameAsContainerField))
                {
                    var containerField = CreateBindingField(typeof(Transform).FullName, CollectionProperty.Name,
                        "Container");
                    collection.Add(containerField);
                }
            }
            if (RelatedElement != null && GenerateDefaultImplementation)
            {
                bindingCondition.TrueStatements.Add(
                    new CodeSnippetExpression(string.Format("var binding = this.BindToViewCollection(() => {0}.{1})", ElementData.Name, CollectionProperty.FieldName)));

                var containerNullCondition =
                    new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(NameAsContainerField),
                            CodeBinaryOperatorType.ValueEquality, new CodeSnippetExpression("null")));

                containerNullCondition.FalseStatements.Add(new CodeSnippetExpression(string.Format("binding.SetParent({0})", NameAsContainerField)));
                bindingCondition.TrueStatements.Add(containerNullCondition);

                var sceneFirstCondition =
                    new CodeConditionStatement(
                        new CodeVariableReferenceExpression(NameAsSceneFirstField));

                sceneFirstCondition.TrueStatements.Add(new CodeSnippetExpression("binding.ViewFirst()"));
                bindingCondition.TrueStatements.Add(sceneFirstCondition);
            }
            else
            {
                bindingCondition.TrueStatements.Add(
                    new CodeSnippetExpression(string.Format("var binding = this.BindCollection(() => {0}.{1})", ElementData.Name,
                        CollectionProperty.FieldName)));
            }
        }

        public string VarTypeName
        {
            get { return RelatedElement == null ? CollectionProperty.RelatedTypeName : RelatedElement.NameAsViewModel; }
        }

        public string ParameterTypeName
        {
            get { return RelatedElement == null ? CollectionProperty.RelatedTypeName : RelatedElement.NameAsViewBase; }
        }

        public virtual string VarName
        {
            get { return "value"; }
        }

    }
}