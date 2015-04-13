using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

public class CreationAndInitialization : uFrameMVVMPage<ElementPage>
{
    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        _.CodeSnippet("var player = new PlayerViewModel(EventAggregator);");
        _.Title2("OR inside of the PlayerController");
        _.CodeSnippet("var player = this.CreatePlayer()");

        _.Title2("Initialization Inside Controllers");
        _.Paragraph("Typically you will use the relevant Controller's Initialize{ElementName} function to initialize a newly created ViewModel with default values and references.  It's a great place to subscribe to state changes and \"scene property\" changes, or possibly track a list of ViewModel instances when acting similarly to a manager.");
        _.Break();
        _.Title2("Initialization Inside Views");
        _.Paragraph("For convenience, you also have the option of Initializing a ViewModel from a particular View, by checking Initialize ViewModel on the View.  This is particularly useful when setting up a scene before runtime or creating prefabs.");
    }
}