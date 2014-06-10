using System.Collections.Generic;
using System.IO;
using Invert.uFrame.Editor;

public class ViewDataGenerator : NodeItemGenerator<ViewData>
{
    public override IEnumerable<CodeGenerator> CreateGenerators(ElementDesignerData diagramData, ViewData item)
    {
        yield return new ViewGenerator()
        {
            IsDesignerFile = false,
            DiagramData = diagramData,
            View = item,
            Filename = Path.Combine("Views",item.NameAsView + ".cs"),
            RelatedType = item.CurrentViewType
        };
        yield return new ViewGenerator()
        {
            IsDesignerFile = true,
            DiagramData = diagramData,
            View = item,
            Filename = diagramData.ViewsFileName,
        };
    }
}
public class ViewComponentDataGenerator : NodeItemGenerator<ViewComponentData>
{
    public override IEnumerable<CodeGenerator> CreateGenerators(ElementDesignerData diagramData, ViewComponentData item)
    {
        yield return new ViewComponentGenerator()
        {
            IsDesignerFile = false,
            DiagramData = diagramData,
            ViewComponentData = item,
            Filename = Path.Combine("ViewComponents", item.Name + ".cs"),
            RelatedType = item.CurrentType
        };
        yield return new ViewComponentGenerator()
        {
            IsDesignerFile = true,
            DiagramData = diagramData,
            ViewComponentData = item,
            Filename = diagramData.ViewsFileName,
          
        };
    }
}