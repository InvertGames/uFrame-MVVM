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
