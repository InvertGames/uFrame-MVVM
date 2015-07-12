using System.Collections.Generic;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

namespace uFrame.MVVM.Templates
{


    [TemplateClass(TemplateLocation.Both)]
    public partial class SimpleClassTemplate : IClassTemplate<SimpleClassNode>, IClassRefactorable
    {
        public string OutputPath
        {
            get { return Path2.Combine(Ctx.Data.Graph.Name, "SimpleClasses"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            this.Ctx.TryAddNamespace("uFrame.Kernel");
            this.Ctx.TryAddNamespace("uFrame.MVVM");
            this.Ctx.TryAddNamespace("uFrame.MVVM.Bindings");
            this.Ctx.TryAddNamespace("uFrame.Serialization");
            foreach (var property in Ctx.Data.ChildItems.OfType<ITypedItem>())
            {
                var type = InvertApplication.FindTypeByNameExternal(property.RelatedTypeName);
                if (type == null) continue;

                Ctx.TryAddNamespace(type.Namespace);
            }

            Ctx.AddIterator("Property", node => node.Properties);
            Ctx.AddIterator("Collection", node => node.Collections);
        }

        public TemplateContext<SimpleClassNode> Ctx { get; set; }

        [ForEach("Properties"), GenerateProperty, WithField]
        public _ITEMTYPE_ _PropertyName_ { get; set; }

        [ForEach("Collections"), GenerateProperty, WithField]
        public List<_ITEMTYPE_> _CollectionName_ { get; set; }


        public IEnumerable<string> ClassNameFormats
        {
            get
            {
                yield return "{0}";
                yield return "{0}Base";
            }
        }
    }
}