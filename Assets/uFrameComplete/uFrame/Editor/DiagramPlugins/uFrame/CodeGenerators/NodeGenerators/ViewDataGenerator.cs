using System.Collections.Generic;
using System.IO;
using Invert.uFrame.Editor;

public class ViewDataGenerator : NodeItemGenerator<ViewData>
{
    public override IEnumerable<CodeGenerator> CreateGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, ViewData item)
    {
        yield return new ViewGenerator()
        {
            IsDesignerFile = false,
            DiagramData = diagramData,
            View = item,
            Filename = pathStrategy.GetEditableViewFilename(item.NameAsView),
            RelatedType = item.CurrentViewType
        };
        yield return new ViewGenerator()
        {
            IsDesignerFile = true,
            DiagramData = diagramData,
            View = item,
            Filename = pathStrategy.GetViewsFileName(diagramData.Name),
        };
    }
}
public class ViewComponentDataGenerator : NodeItemGenerator<ViewComponentData>
{
    public override IEnumerable<CodeGenerator> CreateGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, ViewComponentData item)
    {
        yield return new ViewComponentGenerator()
        {
            IsDesignerFile = false,
            DiagramData = diagramData,
            ViewComponentData = item,
            Filename = pathStrategy.GetEditableViewComponentFilename(item.Name),
            RelatedType = item.CurrentType
        };
        yield return new ViewComponentGenerator()
        {
            IsDesignerFile = true,
            DiagramData = diagramData,
            ViewComponentData = item,
            Filename = pathStrategy.GetViewsFileName(diagramData.Name)
          
        };
    }
}