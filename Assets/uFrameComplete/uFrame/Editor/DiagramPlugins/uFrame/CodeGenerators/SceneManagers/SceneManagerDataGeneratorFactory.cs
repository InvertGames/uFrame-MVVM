using System.Collections.Generic;
using System.IO;
using Invert.uFrame.Editor;

public class SceneManagerDataGeneratorFactory : DesignerGeneratorFactory<SceneManagerData>
{
    public override IEnumerable<CodeGenerator> CreateGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData, SceneManagerData item)
    {
        yield return new SceneManagerGenerator()
        {
            Filename = pathStrategy.GetControllersFileName(diagramData.Name),
            Data = item,
            DiagramData = diagramData,
            IsDesignerFile = true,
        };
        yield return new SceneManagerGenerator()
        {
            Filename = pathStrategy.GetEditableSceneManagerFilename(item),
            Data = item,
            DiagramData = diagramData,
            IsDesignerFile = false,
        };
        yield return new SceneManagerSettingsGenerator()
        {
            IsDesignerFile = false,
            Data = item,
            DiagramData = diagramData,
            RelatedType = item.CurrentSettingsType,
            Filename = pathStrategy.GetEditableSceneManagerSettingsFilename(item)
        };
    }
}