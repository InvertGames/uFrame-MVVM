using Invert.Core.GraphDesigner;
using Invert.uFrame.Editor;
using uFrame.Graphs;

[TemplateClass("ViewComponents","{0}",MemberGeneratorLocation.Both)]
public class ViewComponentTemplate : ViewComponent,IClassTemplate<ElementViewComponentNode>
{
    public void TemplateSetup()
    {
        
    }

    public TemplateContext<ElementViewComponentNode> Ctx { get; set; }
}