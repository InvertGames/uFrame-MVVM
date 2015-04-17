using System;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

public class CreationAndInitialization : uFrameMVVMPage<ElementPage>
{
    public override decimal Order
    {
        get { return -1; }
    }

    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);

        _.CodeSnippet("var player = this.CreatePlayer()");
        _.Title2("Creating a ViewModel with the Extension Method");
        _.Paragraph("In uframe 1.6 there is an extension method for creating any viewmodel that will use the correct controller to create it.");
        _.CodeSnippet("this.CreateViewModel<PlayerViewModel>()");
        _.Paragraph("This method will resolve the controller and invoke the associated controller's 'Create' method.");
        _.Paragraph("It is important that you use this method because the controller will initialize the commands on the viewmodel to point to the correct handlers on itself.");
        _.Break();

        _.Title2("Initialization Inside Controllers");
        _.Paragraph("Typically you will use the relevant Controller's Initialize{ElementName} function to initialize " +
                    "a newly created ViewModel with default values and references.  It's a great place to subscribe to " +
                    "state changes and \"scene property\" changes, or possibly track a list of ViewModel instances when " +
                    "acting similarly to a manager.");

        

        _.Break();
        _.Title2("Initialization Inside Views");
        _.Paragraph("For convenience, you also have the option of Initializing a ViewModel from a particular View, by checking Initialize ViewModel on the View.  This is particularly useful when setting up a scene before runtime or creating prefabs.");

        _.Break();
        _.AlsoSeePages(typeof(ViewModelManagers));
    }
}

public class ViewModelManagers : uFrameMVVMPage<ElementPage>
{
    public override decimal Order
    {
        get { return -1; }
    }



    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
    }
}