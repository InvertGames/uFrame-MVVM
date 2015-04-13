using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

public class ExamplePage : uFrameMVVMPage<ElementPropertiesPage>
{


    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        var ele = new ScaffoldGraph()
            .BeginNode<ElementNode>("Player")
            .AddItem<PropertiesChildItem>("FirstName")
            .AddItem<PropertiesChildItem>("LastName")
            .EndNode();

        _.Title2("Subscribable Properties");
        _.TemplateExample<ViewModelTemplate, ElementNode>(ele as ElementNode, true, "ViewModelProperty");

        _.Title2("Value Wrapper Properties");
        _.TemplateExample<ViewModelTemplate, ElementNode>(ele as ElementNode, true, "ViewModelValueProperty");

        _.Title2("The Bind Method");
        _.TemplateExample<ViewModelTemplate, ElementNode>(ele as ElementNode, true, "Bind");
    }
}