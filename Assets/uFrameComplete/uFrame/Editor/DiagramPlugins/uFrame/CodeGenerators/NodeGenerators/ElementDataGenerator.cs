using System.Collections.Generic;
using System.IO;
using Invert.uFrame.Editor;

public class ElementDataGenerator : NodeItemGenerator<ElementData>
{
    public override IEnumerable<CodeGenerator> CreateGenerators(ElementDesignerData diagramData, ElementData item)
    {
        yield return CreateDesignerControllerGenerator(diagramData,item);
        yield return CreateEditableControllerGenerator(diagramData, item);
        yield return CreateDesignerViewModelGenerator(diagramData, item);
        yield return CreateEditableViewModelGenerator(diagramData, item);
        yield return CreateViewBaseGenerator(diagramData, item);
    }

    public virtual CodeGenerator CreateDesignerControllerGenerator(ElementDesignerData diagramData,ElementData item)
    {
        return new ControllerGenerator()
        {
            ElementData = item,
            DiagramData = diagramData,
            Filename = diagramData.ControllersFileName,
            IsDesignerFile = true
        };
    }

    public virtual CodeGenerator CreateEditableControllerGenerator(ElementDesignerData diagramData, ElementData item)
    {
        return new ControllerGenerator()
        {
            ElementData = item,
            RelatedType = item.ControllerType,
            DiagramData = diagramData,
            Filename = Path.Combine("Controllers", item.ControllerName + ".cs"),
            IsDesignerFile = false
        };
    }

    public virtual CodeGenerator CreateDesignerViewModelGenerator(ElementDesignerData diagramData, ElementData item)
    {
        return new ViewModelGenerator(true, item)
        {
            Data = item,
            IsDesignerFile = true,
            DiagramData = diagramData,
            Filename = diagramData.ViewModelsFileName
        };
    }

    public virtual CodeGenerator CreateEditableViewModelGenerator(ElementDesignerData diagramData, ElementData item)
    {
        return new ViewModelGenerator(false, item)
        {
            IsDesignerFile = false,
            Data = item,
            RelatedType = item.CurrentViewModelType,
            DiagramData = diagramData,
            Filename = Path.Combine("ViewModels", item.NameAsViewModel + ".cs")
        };
    }

    public virtual CodeGenerator CreateViewBaseGenerator(ElementDesignerData diagramData, ElementData item)
    {
        return new ViewBaseGenerator()
        {
            ElementData = item,
            DiagramData = diagramData,
            IsDesignerFile = true,
            
            Filename = diagramData.ViewsFileName
        };
    }
}