using System.Collections.Generic;
using System.IO;
using Invert.uFrame.Editor;

public class SceneManagerDataGenerator : NodeItemGenerator<SceneManagerData>
{
    public override IEnumerable<CodeGenerator> CreateGenerators(ElementDesignerData diagramData, SceneManagerData item)
    {
        yield return new SceneManagerGenerator()
        {
            Filename = diagramData.ControllersFileName,
            Data = item,
            DiagramData = diagramData,
            IsDesignerFile = true,
        };
        yield return new SceneManagerGenerator()
        {
            Filename = Path.Combine("SceneManagers", item.NameAsSceneManager + ".cs"),
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
            Filename = Path.Combine("SceneManagers",item.NameAsSettings + ".cs")
        };
    }
}