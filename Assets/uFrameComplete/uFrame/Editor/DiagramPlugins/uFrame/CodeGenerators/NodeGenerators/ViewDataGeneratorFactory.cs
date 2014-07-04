using System.Collections.Generic;
using System.IO;
using Invert.uFrame.Editor;

public class ViewDataGeneratorFactory : DesignerGeneratorFactory<ViewData>
{
    public override IEnumerable<CodeGenerator> CreateGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, ViewData item)
    {
        yield return CreateEditableGenerator(pathStrategy, diagramData, item);
        yield return CreateDesignerGenerator(pathStrategy, diagramData, item);
    }

    protected virtual ViewGenerator CreateEditableGenerator(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, ViewData item)
    {
        return new ViewGenerator()
        {
            IsDesignerFile = false,
            DiagramData = diagramData,
            View = item,
            Filename = pathStrategy.GetEditableViewFilename(item),
            RelatedType = item.CurrentViewType
        };
    }

    protected virtual ViewGenerator CreateDesignerGenerator(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, ViewData item)
    {
        return new ViewGenerator()
        {
            IsDesignerFile = true,
            DiagramData = diagramData,
            View = item,
            Filename = pathStrategy.GetViewsFileName(diagramData.Name),
        };
    }
}
public class ViewComponentDataGeneratorFactory : DesignerGeneratorFactory<ViewComponentData>
{
    public override IEnumerable<CodeGenerator> CreateGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, ViewComponentData item)
    {
        yield return CreateEditableGenerator(pathStrategy, diagramData, item);
        yield return CreateDesignerGenerator(pathStrategy, diagramData, item);
    }

    protected virtual ViewComponentGenerator CreateEditableGenerator(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, ViewComponentData item)
    {
        return new ViewComponentGenerator()
        {
            IsDesignerFile = false,
            DiagramData = diagramData,
            ViewComponentData = item,
            Filename = pathStrategy.GetEditableViewComponentFilename(item),
            RelatedType = item.CurrentType
        };
    }

    protected virtual ViewComponentGenerator CreateDesignerGenerator(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, ViewComponentData item)
    {
        return new ViewComponentGenerator()
        {
            IsDesignerFile = true,
            DiagramData = diagramData,
            ViewComponentData = item,
            Filename = pathStrategy.GetViewsFileName(diagramData.Name)
          
        };
    }
}