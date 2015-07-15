using System.Collections.Generic;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.DesignerFile, ClassNameFormat = "{0}Command")]
    [RequiresNamespace("UnityEngine")]
    public partial class CommandClassTemplate : CommandClassTemplateBase, IClassRefactorable
    {
        public IEnumerable<string> ClassNameFormats
        {
            get { yield return "{0}Command"; }
        }

        public override string OutputPath
        {
            get { return Path2.Combine(Ctx.Data.Node.Graph.Name, "Commands"); }
        }


        public override void TemplateSetup()
        {
            base.TemplateSetup();
            this.Ctx.TryAddNamespace("uFrame.Kernel");
            this.Ctx.TryAddNamespace("uFrame.MVVM");
            this.Ctx.TryAddNamespace("uFrame.Serialization");
            foreach (var property in Ctx.Data.PersistedItems.OfType<ITypedItem>())
            {
                var type = InvertApplication.FindTypeByNameExternal(property.RelatedTypeName);
                if (type == null) continue;

                Ctx.TryAddNamespace(type.Namespace);
            }

            Ctx.CurrentDeclaration.IsPartial = true;
        }


        [ForEach("Properties"), GenerateProperty, WithField]
        public _ITEMTYPE_ _Name_ { get; set; }

        [ForEach("Collections"), GenerateProperty, WithField]
        public List<_ITEMTYPE_> _CollectionName_ { get; set; }
    }
}