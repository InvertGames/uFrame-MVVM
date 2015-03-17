using System.Collections.Generic;
using System.Linq;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

[TemplateClass(MemberGeneratorLocation.Both)]
public class SimpleClassTemplate : IClassTemplate<SimpleClassNode>
{
    public string OutputPath
    {
        get { return "SimplesClasses"; }
    }

    public bool CanGenerate { get { return true; } }

    public void TemplateSetup()
    {
        foreach (var property in Ctx.Data.ChildItems.OfType<ITypedItem>())
        {
            var type = InvertApplication.FindTypeByName(property.RelatedTypeName);
            if (type == null) continue;

            Ctx.TryAddNamespace(type.Namespace);
        }

        Ctx.AddIterator("Property", node => node.Properties);
        Ctx.AddIterator("Collection", node => node.Collections);
    }

    public TemplateContext<SimpleClassNode> Ctx { get; set; }

    [TemplateProperty(MemberGeneratorLocation.DesignerFile, AutoFillType.NameAndTypeWithBackingField)]
    public string Property { get; set; }

    [TemplateProperty(MemberGeneratorLocation.DesignerFile, AutoFillType.NameAndTypeWithBackingField)]
    public List<string> Collection { get; set; }


}