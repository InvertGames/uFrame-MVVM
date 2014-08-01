using System;
using System.CodeDom;
using System.Linq;
using Invert.uFrame.Editor;

namespace Invert.uFrame.Editor
{
    public abstract class CodeGenerator
    {
        private CodeNamespace _ns;
        private CodeCompileUnit _unit;

        public virtual Type RelatedType
        {
            get;
            set;
        }

        public virtual string Filename
        {
            get;
            set;
        }

        public virtual void Initialize(CodeFileGenerator fileGenerator)
        {
            _unit = fileGenerator.Unit;
            _ns = fileGenerator.Namespace;
        }

        public CodeNamespace Namespace
        {
            get { return _ns; }
        }

        public CodeCompileUnit Unit
        {
            get { return _unit; }
        }

        public bool IsDesignerFile
        {
            get;
            set;
        }

        public Type GeneratorFor { get; set; }
        public object ObjectData { get; set; }

        public void ProcessModifiers(CodeTypeDeclaration declaration)
        {
            var typeDeclerationModifiers = uFrameEditor.Container.ResolveAll<ITypeGeneratorPostProcessor>().Where(p => p.For.IsAssignableFrom(this.GetType()));
            foreach (var typeDeclerationModifier in typeDeclerationModifiers)
            {
                //typeDeclerationModifier.FileGenerator = codeFileGenerator;
                typeDeclerationModifier.Declaration = declaration;

                typeDeclerationModifier.Generator = this;
                typeDeclerationModifier.Apply();
            }

        }
    }
}

namespace Invert.uFrame.Code.Bindings
{
    public interface IBindingGenerator
    {
        string MethodName { get; }
        IViewModelItem Item { get; set; }
        bool IsApplicable { get; }
        bool IsOverride { get; set; }
        string BindingConditionFieldName { get; }
        void CreateMembers(CodeTypeMemberCollection collection);
        void CreateBindingStatement(CodeTypeMemberCollection collection, CodeConditionStatement bindingCondition);
    }

}