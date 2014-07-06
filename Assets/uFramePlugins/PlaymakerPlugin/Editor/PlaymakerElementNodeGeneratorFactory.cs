using System.Collections.Generic;
using System.IO;
using System.Linq;
using Invert.uFrame.Editor;

public class PlaymakerElementNodeGeneratorFactory : DesignerGeneratorFactory<ElementData>
{
    public override IEnumerable<CodeGenerator> CreateGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, ElementData item)
    {
        if (item["Playmaker"])
        {
            foreach (var property in item.Properties)
            {
                ViewModelPropertyData property1 = property;
                var relatedEnum = diagramData.AllDiagramItems.FirstOrDefault(p => p.Name == property1.RelatedTypeName) as EnumData;
                if (relatedEnum != null)
                {
                    yield return new PlaymakerEnumActionCodeGenerator()
                    {
                        IsDesignerFile = true,
                        ClassName = item.Name + property.Name + "Compare",
                        PropertyData = property,
                        EnumData = relatedEnum,
                        Filename = diagramData.Name + "PlaymakerActions.designer.cs"
                    };
                }

            }
            foreach (var command in item.Commands)
            {
                //yield return new PlaymakerActionCodeGenerator()
                //{
                //    IsDesignerFile = false,
                //    CommandData = command,
                //    DiagramData = diagramData,
                //    Filename = Path.Combine("Playmaker", command.Name + "PlaymakerAction.cs")
                //};
                yield return new PlaymakerActionCodeGenerator()
                {
                    IsDesignerFile = true,
                    CommandData = command,
                    ElementData = item,
                    DiagramData = diagramData,
                    Filename = diagramData.Name + "PlaymakerActions.designer.cs"
                };
            }
            
        }
    }
}