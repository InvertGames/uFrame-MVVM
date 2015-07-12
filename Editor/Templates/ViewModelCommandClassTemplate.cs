using System.Collections.Generic;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.DesignerFile, ClassNameFormat = "{0}Command")]
    public partial class ViewModelCommandClassTemplate : ViewModelCommand, IClassTemplate<CommandsChildItem>,
        IClassRefactorable
    {
        public string OutputPath
        {
            get { return Path2.Combine(Ctx.Data.Node.Graph.Name, "Commands"); }
        }

        public bool CanGenerate
        {
            get { return Ctx.Data.OutputCommand == null; }
        }

        public void TemplateSetup()
        {
            this.Ctx.TryAddNamespace("uFrame.Kernel");
            this.Ctx.TryAddNamespace("uFrame.MVVM");
            this.Ctx.TryAddNamespace("uFrame.Serialization");
            var type = InvertApplication.FindTypeByNameExternal(Ctx.Data.RelatedTypeName);
            if (type != null)
            {
                Ctx.TryAddNamespace(type.Namespace);
            }
            else
            {
                type = InvertApplication.FindType(Ctx.Data.RelatedTypeName);
                if (type != null)
                    Ctx.TryAddNamespace(type.Namespace);
            }
            Ctx.CurrentDeclaration.IsPartial = true;
            Ctx.CurrentDeclaration.Name = Ctx.Data.Name + "Command";
            Ctx.AddCondition("Argument", _ => !string.IsNullOrEmpty(_.RelatedType));
        }

        public TemplateContext<CommandsChildItem> Ctx { get; set; }

        public bool HasArgument
        {
            get { return !string.IsNullOrEmpty(Ctx.Data.RelatedType); }
        }

        [GenerateProperty, WithField, If("HasArgument")]
        public _ITEMTYPE_ Argument { get; set; }

        public IEnumerable<string> ClassNameFormats
        {
            get { yield return "{0}Command"; }
        }
    }
}