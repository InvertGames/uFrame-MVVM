using System.Collections.Generic;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

public class InteractiveTutorials : uFrameMVVMPage
{
    public override string Name
    {
        get { return "Interactive Tutorials (Experimental)"; }
    }
    
    public override decimal Order
    {
        get { return -2; }
    }

    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        _.Section("What is interactive tutorial?");
        _.Paragraph("Interactive tutorial is a new experimental type of learning material. Unlike regular static tutorials, interactive tutorials communicate with the environment to perform the following tasks:\n\n" +
                    "* Step completion analysis\n" +
                    "* Just-In-Time hints and troubleshooting\n" +
                    "* Dynamic content adjustments\n");

        _.Paragraph("Each tutorial exposes several steps to go through. It analyses current state of your project and figures out if current step is complete.");
        _.Note("Interactive tutorial re-evaluates the environment every time you focus documentation window with this tutorial opened. So once you think you finished the step, please, focus the documentation window" +
               " and let interactive tutorial check your progress. If conditions are met, the current step will be considered finished and the next step will be revealed");
        _.Paragraph("While following the tutorial, you may accidently break something, that you have done in the previous step. In this case, tutorial goes back to the broken step and you will have to adjust your  ");
        _.Paragraph("Based on your actions, the content of the step may be adjusted to guide you in the right direction. More over, text of each tutorial tends to adjust to your current task. That is why, learning material of each step may change as you progress.");

    }
}