using System.Collections.Generic;
using System.IO;
using Invert.uFrame.Editor;

public class ElementDataGenerator : NodeItemGenerator<ElementData>
{
    public override IEnumerable<CodeGenerator> CreateGenerators(ICodePathStrategy codePathStrategy, IElementDesignerData diagramData, ElementData item)
    {
        
        yield return CreateDesignerControllerGenerator(codePathStrategy,diagramData,item);
        yield return CreateEditableControllerGenerator(codePathStrategy, diagramData, item);
        yield return CreateDesignerViewModelGenerator(codePathStrategy, diagramData, item);
        yield return CreateEditableViewModelGenerator(codePathStrategy, diagramData, item);
        yield return CreateViewBaseGenerator(codePathStrategy, diagramData, item);
    }

    public virtual CodeGenerator CreateDesignerControllerGenerator(ICodePathStrategy codePathStrategy, IElementDesignerData diagramData,ElementData item)
    {
        return new ControllerGenerator()
        {
            ElementData = item,
            DiagramData = diagramData,
            Filename = codePathStrategy.GetControllersFileName(diagramData.Name),
            IsDesignerFile = true
        };
    }

    public virtual CodeGenerator CreateEditableControllerGenerator(ICodePathStrategy codePathStrategy, IElementDesignerData diagramData, ElementData item)
    {
        return new ControllerGenerator()
        {
            ElementData = item,
            RelatedType = item.ControllerType,
            DiagramData = diagramData,
            Filename = codePathStrategy.GetEditableControllerFilename(item.NameAsController),
            IsDesignerFile = false
        };
    }

    public virtual CodeGenerator CreateDesignerViewModelGenerator(ICodePathStrategy codePathStrategy, IElementDesignerData diagramData, ElementData item)
    {
        return new ViewModelGenerator(true, item)
        {
            Data = item,
            IsDesignerFile = true,
            DiagramData = diagramData,
            Filename = codePathStrategy.GetViewModelsFileName(diagramData.Name)
        };
    }

    public virtual CodeGenerator CreateEditableViewModelGenerator(ICodePathStrategy codePathStrategy, IElementDesignerData diagramData, ElementData item)
    {
        return new ViewModelGenerator(false, item)
        {
            IsDesignerFile = false,
            Data = item,
            RelatedType = item.CurrentViewModelType,
            DiagramData = diagramData,
            Filename = codePathStrategy.GetEditableViewModelFilename(item.NameAsViewModel)
        };
    }

    public virtual CodeGenerator CreateViewBaseGenerator(ICodePathStrategy codePathStrategy, IElementDesignerData diagramData, ElementData item)
    {
        return new ViewBaseGenerator()
        {
            ElementData = item,
            DiagramData = diagramData,
            IsDesignerFile = true,

            Filename = codePathStrategy.GetViewsFileName(diagramData.Name)
        };
    }
}