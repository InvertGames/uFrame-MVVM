using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Graphs;
using uFrame.Kernel;

namespace uFrame.MVVM.Templates
{
    [TemplateClass(TemplateLocation.DesignerFile, ClassNameFormat = uFrameFormats.VIEW_MODEL_FORMAT)]
    public partial class ViewModelConstructorTemplate : IClassTemplate<ElementNode>
    {
        public TemplateContext<ElementNode> Ctx { get; set; }

        public string OutputPath
        {
            get { return Path2.Combine(Ctx.Data.Graph.Name, "ViewModels"); }
        }

        [GenerateConstructor("aggregator")]
        public void AggregatorConstructor(IEventAggregator aggregator)
        {

        }

        public bool CanGenerate
        {
            get { return true; }
        }

        public void TemplateSetup()
        {
            this.Ctx.TryAddNamespace("uFrame.Kernel");
            this.Ctx.TryAddNamespace("uFrame.MVVM");
            this.Ctx.TryAddNamespace("uFrame.Serialization");
            // Ensure the namespaces for each property type are property set up
            Ctx.CurrentDeclaration.BaseTypes.Clear();
            Ctx.CurrentDeclaration.IsPartial = true;
            Ctx.CurrentDeclaration.Name = string.Format(uFrameFormats.VIEW_MODEL_FORMAT, Ctx.Data.Name);
        }

    }
}