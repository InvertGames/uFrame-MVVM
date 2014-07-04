using System.Collections.Generic;
using Invert.uFrame.Editor;

public class EnumDataGeneratorFactory : DesignerGeneratorFactory<EnumData>
{
    public override IEnumerable<CodeGenerator> CreateGenerators(ICodePathStrategy pathStrategy,IElementDesignerData diagramData, EnumData item)
    {
        yield return new EnumCodeGenerator()
        {
            EnumData = item,
            Filename = pathStrategy.GetEnumsFilename(item),
        };

    }
}