using System.CodeDom;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(Location = TemplateLocation.DesignerFile, AutoInherit = true, ClassNameFormat = "{0}Command")]
    public class CommandClassTemplateBase : IClassTemplate<CommandNode>
    {

        private Invert.Core.GraphDesigner.TemplateContext<CommandNode> _Ctx;

        public virtual string OutputPath
        {
            get { return ""; }
        }

        public virtual bool CanGenerate
        {
            get { return true; }
        }

        public Invert.Core.GraphDesigner.TemplateContext<CommandNode> Ctx
        {
            get { return _Ctx; }
            set { _Ctx = value; }
        }

        public virtual void TemplateSetup()
        {
            if (Ctx.IsDesignerFile)
            {
                Ctx.CurrentDeclaration.BaseTypes.Clear();
                Ctx.CurrentDeclaration.BaseTypes.Add(new CodeTypeReference("ViewModelCommand"));
            }
        }
    }
}