using System.CodeDom;
using Invert.uFrame.Editor;

public class ViewComponentGenerator : ViewClassGenerator
{

    public ViewComponentData ViewComponentData
    {
        get; set;
    }

    public override void Initialize(CodeFileGenerator fileGenerator)
    {
        base.Initialize(fileGenerator);
        AddViewComponent(ViewComponentData);
    }

    public void AddViewComponent(ViewComponentData componentData)
    {
        var element = componentData.Element;
        if (element == null) return;

        var baseComponent = componentData.Base;

        var ctr = baseComponent == null
            ? new CodeTypeReference(uFrameEditor.uFrameTypes.ViewComponent)
            : new CodeTypeReference(baseComponent.Name);


        var decl = new CodeTypeDeclaration()
        {
            Name = componentData.Name,
            Attributes = MemberAttributes.Public,
            IsPartial = true
        };
        if (IsDesignerFile)
        {
            decl.BaseTypes.Add(ctr);

            if (baseComponent == null)
            {
                decl.CreateViewModelProperty(element);
                AddExecuteMethods(element, decl, true);
            }
        }
        Namespace.Types.Add(decl);
    }
}