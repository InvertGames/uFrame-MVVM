using System.IO;
using Invert.Core.GraphDesigner;
using Invert.StateMachine;
using Invert.uFrame.MVVM;
using uFrame.Graphs;

[TemplateClass(MemberGeneratorLocation.Both)]
public class ViewComponentTemplate : ViewComponent,IClassTemplate<ViewComponentNode>
{
    public string OutputPath
    {
        get { return "ViewComponents"; }
    }

    public bool CanGenerate
    {
        get { return true; }
    }

    public void TemplateSetup()
    {
        
    }

    public TemplateContext<ViewComponentNode> Ctx { get; set; }
}

