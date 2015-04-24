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

public class Controllers : uFrameMVVMPage<ElementPage>
{
    public override decimal Order
    {
        get { return -1; }
    }

    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        _.Title2("What is it?");
        _.Paragraph("Controllers dictate the rules of your game and, like ViewModels, they do not inherit from Unity's Monobehaviour.  As they only handle logic, there only ever needs to be one Controller for each type of ViewModel.  For example, when a PlayerViewModel performs the PickupItem command, the PlayerController would simply need to know which PlayerViewModel to execute that command logic on, and the logic remains the same whether there are 4 players or one.");

        _.Paragraph(" A controller is designed to implement the data-driven logic and rules behind an element " +
                    "and could be considered just a \"group\" of commands for a view-model. " +
                    "The Designer has enough information about an element to implement most of the controller itself " +
                    "In most instances, you only need to apply the logic and rules to each method.  ");

        _.Break();
        


        _.Title2("Best Practices");
        _.Paragraph("When implementing controllers, think of the element as its own little section of your world, " +
                    "if you want your element to interact or be visible to the entire world, publish events as " +
                    "necessary in your command controller methods.");
        _.Paragraph("To understand this idea a bit more, take a look at the following diagram.");
        _.ImageByUrl("http://i.imgur.com/KbPL9bw.png");
        _.Paragraph("So in the diagram above, on the EnemyHit command handler, we publish the command as 'global' event.  This means that services can be your general connection layer that make various elements work together.");
        _.Title3("But why should I do this?");
        _.Paragraph("Imagine you create a Player element, if it lives entirely on its own (no dependencies on other controllers, services..etc), you can re-use the element in another game and implement services to connect them together.");
        _.Break();
        _.Break();

        _.Title2("The Setup Method");
        _.Paragraph("The setup method is an implementation of the ISystemService interface, this means that all controllers are ultimately services, they do not derive from monobehaviour.  This means you can easily listen to any kind of event on a controller, as well as publish them.");
        _.Title3("Important Note");
        _.Paragraph("You must be careful when listening to events in controllers that use inheritance. For instance, if you have an elementA controller, and a derived elementB controller, and in the setup method you are listening to event 'C', then beth element A and element B will be listening to the same event.  In some cases this may be wanted behaviour, but its important to understand.");
        _.Break();
        _.Break();
        _.Title2("The Initialize Method");
        _.Paragraph("The initialize Method in a controller can be used to initialize an Element's ViewModel (similar to how it might be initialized in the inspector of a View). It is a great place to subscribe to 'Scene Properties'.");
        _.Title2("Command Handlers");
        _.Paragraph("Every command that is outlined on an element, in the Element Designer, will have a corresponding method in a controller (assuming the diagram is saved).  ");
        _.Paragraph("There are a few things to notice when looking at the code example below:");
        _.Paragraph(" - The \"FPSWeaponController\" is derived from \"FPSWeaponControllerBase\", which is generated by the designer.");
        _.Paragraph(" - All of these methods are overrides, because the base class has implemented an empty \"virtual method\" for each command.");
        _.Paragraph(" - The reload method has an IEnumerator result type, because is it is marked as a yield command (indicated by the yellow marker).  This can be achieved by right-clicking on a command and checking \"Is Yield Command\". It is used for simulating a co-routine.");
        _.Paragraph(" - In each method we are simply processing rules by reading and modifying the FPSWeaponViewModel's instance data .");
        _.ShowGist("7bba5439faf0b46efa61","FPSWeaponController.cs");
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
        _.Paragraph("By default uFrame keeps up with viewmodels for us.  It maintains a manager for each type of viewmodel you create.");
        _.Paragraph("To access these managers, you can inject them into controllers and services using the following code.");
        _.Break();
        _.CodeSnippet("[Inject] public IViewModelManager<MyViewModel> MyViewModelsManager { get; set; }");
        _.Break();
        _.Note("Important Note: By default every element controller already generates manager properties just like the above example for the associated element's viewmodel.");

    }
}