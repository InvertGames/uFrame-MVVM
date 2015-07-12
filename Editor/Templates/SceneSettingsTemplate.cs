using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

namespace uFrame.MVVM.Templates
{



    [TemplateClass(TemplateLocation.Both, "{0}Settings")]
    public partial class SceneSettingsTemplate : IClassTemplate<SceneTypeNode>
    {
        public void TemplateSetup()
        {
            this.Ctx.TryAddNamespace("uFrame.Kernel");
            this.Ctx.TryAddNamespace("uFrame.Serialization");
            if (Ctx.IsDesignerFile)
            {
                Ctx.CurrentDeclaration.BaseTypes.Clear();
                Ctx.SetBaseType("SceneSettings<{0}>", Ctx.Data.Name);
            }
        }

        public string OutputPath
        {
            get { return Path2.Combine(Ctx.Data.Graph.Name, "ScenesSettings"); }
        }

        public bool CanGenerate
        {
            get { return true; }
        }



        public TemplateContext<SceneTypeNode> Ctx { get; set; }
    }
}