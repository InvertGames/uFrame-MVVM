using System;
using System.Collections.Generic;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using UnityEngine;

public class HelloWorldTutorial : uFrameMVVMPage
{
    public override string Name
    {
        get { return "Hello World Tutorial"; }
    }

    public override Type ParentPage
    {
        get { return typeof(GettingStarted); }
    }

    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        _.BeginTutorial(this.Name);
        DoTutorial(_);
        DoFinalStep(_);
        _.EndTutorial();
    }

    protected virtual void DoTutorial(IDocumentationBuilder builder)
    {
        var project = DoCreateNewProjectStep(builder);

        if (project == null) return;

        var graph = DoGraphStep<MVVMGraph>(builder,
            _ => _.ImageByUrl("http://i.imgur.com/2WrQCzv.png"));
        
        if (graph == null) return;
        
        var subsystemNode = DoNamedNodeStep<SubsystemNode>(builder, "Core", null,
            _ => { _.ImageByUrl("http://i.imgur.com/yW7CmVr.png"); });

        if (subsystemNode == null) return;


        var sceneManagerNode = DoNamedNodeStep<SceneManagerNode>(
            builder,
            "MainScene",
            null,
            _ => _.ImageByUrl("http://i.imgur.com/v8GehAF.png")
            );

        if (sceneManagerNode == null) return;


        DoCreateConnectionStep(
            builder,
            subsystemNode.ExportOutputSlot,
            sceneManagerNode.SubsystemInputSlot,
            _ => _.ImageByUrl("http://i.imgur.com/q3kchHi.png")
        );

        var elementNode = DoNamedNodeStep<ElementNode>(
            builder,
            "GameContext",
            subsystemNode,
            _ => _.ImageByUrl("http://i.imgur.com/4ioW9EX.png")
            );

        if (elementNode == null) return;

        var firstNameProperty = DoNamedItemStep<PropertiesChildItem>(builder, "FirstName", elementNode,
            "a property", _ => _.ImageByUrl("http://i.imgur.com/qn5GZ6b.png"));

        if (firstNameProperty == null) return;

        var changeNameCommand = DoNamedItemStep<CommandsChildItem>(builder, "ChangeName", elementNode, "a command",
            _ => { _.ImageByUrl("http://i.imgur.com/4lxeQru.png"); });

        if (changeNameCommand == null) return;

        var viewNode = DoNamedNodeStep<ViewNode>(builder, "GameContextView", elementNode,
            _ => { _.ImageByUrl("http://i.imgur.com/g6cs4tw.png"); });
        if (viewNode == null) return;

        DoCreateConnectionStep(builder, elementNode, viewNode.ElementInputSlot,
            _ => { _.ImageByUrl("http://i.imgur.com/W7ZTGjU.png"); });

        var binding = DoNamedItemStep<BindingsReference>(builder, "FirstName Changed", viewNode,
            "a binding by choosing the item", _ => { });
        if (binding == null) return;

        builder.ShowTutorialStep(SaveAndCompile(viewNode));


        builder.ShowTutorialStep(CreateSceneCommand(sceneManagerNode),
            _ => { _.ImageByUrl("http://i.imgur.com/9QSXAPH.png"); });


        var viewBase = EnsureComponentInSceneStep<ViewBase>(builder, viewNode, _ =>
        {
            _.ImageByUrl("http://i.imgur.com/JTIeWL8.png");
            _.ImageByUrl("http://i.imgur.com/1CTHigb.png");
        });

        builder.ShowTutorialStep(
            new TutorialStep("Ctrl+Click on the {0} Node.  This will open up the controller code.",
                () => null),
            _ => { _.ImageByUrl("http://i.imgur.com/jE47U8m.png"); });


        builder.ShowTutorialStep(
            new TutorialStep(
                "Ctrl+Click on the {0} Node.  This will open up the controller code.",
                () => EnsureCodeInEditableFile(elementNode, "Controller.cs", ".FirstName")),
            _ => { _.ImageByUrl("http://i.imgur.com/jE47U8m.png"); }
            );

    }

    protected virtual void DoFinalStep(IDocumentationBuilder builder)
    {
        builder.ShowTutorialStep(
            new TutorialStep(
                "Congratulations",
                () => "You're awesome!"),
            _ =>
            {
                _.Paragraph(
                    "Run the game, click on the view, and then click on the 'ChangeName' button. You'll see the FirstName property showing 'Hello World'.");
                _.ImageByUrl("http://i.imgur.com/dfkOvX1.png");
            }
            );
    }
}

public class ViewBindingsTutorial : HelloWorldTutorial
{
    public override string Name
    {
        get { return "View Bindings Tutorial"; }
    }

    public override decimal Order
    {
        get { return 2; }
    }

    protected override void DoTutorial(IDocumentationBuilder builder)
    {
        base.DoTutorial(builder);

    }
}

public class GettingStarted : uFrameMVVMPage
{
    public override string Name
    {
        get { return "Getting Started"; }
    }
    
    public override decimal Order
    {
        get { return -2; }
    }
}

public abstract class uFrameMVVMPage<TParentPage> : uFrameMVVMPage
{

    public override Type ParentPage
    {
        get { return typeof (TParentPage); }
    }
}
public class ImportantNotes : uFrameMVVMPage<GettingStarted>
{


    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        _.Paragraph("Although uFrame does its best to support and guide you to the creation of better games, it is extremely important to realize that the implementation is still always left to you.  While uFrame tries to separate things clearly, there are still countless ways you can circumvent and break the patterns put in place.  The most common violation is through mixing core game logic with representation logic, where some poor developer has created a nightmare trying to access fields on Views from Controllers or other Views.  These separations exist for a reason, so one puzzle piece can content itself with handling its own functionality and not worry how other pieces are handling themselves.");
        _.Break();
        _.Paragraph("There are also many situations with multiple valid solutions, and it all depends on how you decide to arrange your implementation.  When prototyping, many things are forgivable and easily redesigned as the project evolves and features are settled on.  It's very easy to bind/subscribe to properties in order to initiate logic, but you should always consider from where you want that logic handled.  For example, imagine something odd happens and your RobotEnemy suddenly stops working correctly.  If all of your modifications are done in RobotEnemyController, then you've only got one place to look for the problem.  If you've allowed any parent, or child, or practically anything with a reference to RobotEnemy, to make modifications to it, then you've got more places to check...");
        _.Break();
        _.Paragraph(" Above all, stick to the separation of Controller <-> ViewModel <- View.");
        _.Paragraph(" - Controllers handle the layer of core game logic,");
        _.Paragraph(" - ViewModels are effectively the data layer,");
        _.Paragraph(" - and Views are the presentation layer, handling how the game is displayed and represented in Unity.");
    }
}

public class ChangeLogPage : uFrameMVVMPage
{
    private TextAsset _changeLog;
    private string[] _lines;

    public TextAsset ChangeLog
    {
        get { return _changeLog ?? (_changeLog = Resources.Load("uFrameReadme", typeof(TextAsset)) as TextAsset); }
        set { _changeLog = value; }
    }

    public string[] Lines
    {
        get
        {
            return _lines ?? (_lines = ChangeLog.text.Split(Environment.NewLine.ToCharArray()));
        }
    }
    public override string Name
    {
        get { return "Change Log"; }
    }

    public override decimal Order
    {
        get { return -4; }
    }

    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        foreach (var line in Lines)
        {
            _.Paragraph(line);
        }
    }
}

public class MVCVMInDepth : uFrameMVVMPage
{
    public override decimal Order
    {
        get { return -2; }
    }
}

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

public class HowItWorksTogether : uFrameMVVMPage<MVCVMInDepth>
{
    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        _.Paragraph("So if you've made games in Unity before, you may have noticed how easy " +
                    "it is to end up with a mess of components with heavy dependencies.  Unit " +
                    "testing is impossible.  Adding/removing properties or changing the game " +
                    "logic of one component may break components on one, two, or half a dozen " +
                    "other Gameobjects and/or UI elements.  It can easily become a nightmare.");
        _.Break();
        _.Paragraph("In uFrame, an emphasis is placed on separating out the pieces of your game " +
                    "into certain categories (often referred to as \"layers\"), based on this hybrid" +
                    " MVCVM pattern.  The reasoning behind this is to help enforce separation of concerns, " +
                    "and allow you to quickly split things into these categories and think about them " +
                    "up-front.  These parts are defined as:");
        _.ImageByUrl("http://i.imgur.com/oVunJef.png");
        _.Paragraph(" - ViewModels: The objects, including properties and available commands");
        _.Paragraph(" - Controllers: The rules, where you implement logic of commands");
        _.Paragraph(" - Views: The visual/auditory representation of your objects in a game engine environment");
        _.Break();
        _.Paragraph("It gets a little more complicated with the actual implementation, but the chart above " +
                    "is the core concept.  Ideas are always theoretical.  The idea of your game and everything" +
                    " that defines it should technically be able to exist in any environment, whether it's a " +
                    "game engine, a console app, or a physical board game.  A player takes damage and health " +
                    "is decremented; this concept can be represented any number of ways, as a UI health gauge, " +
                    "a damage printout message in a console, or loss of health tokens from a board game player.");
        _.Break();
        _.Break();
        _.Paragraph(
            "In the previous example the Player would be a ViewModel, an object in your game, with a " +
            "Health property.  There would most likely be a TakeDamage command defined in the " +
            "PlayerController, which would handle the rule of decrementing the playerViewModel's Health." +
            "  When the value changes on the Health property, you may have it trigger a view binding on " +
            "the PlayerHUDView which updates a health gauge according to this new value.  The fun part is " +
            "that all it takes to trigger this chain of events is something like:");

        _.CodeSnippet("ExecuteCommand(playerVM.TakeDamage, 10); // player takes 10 damage");
        _.Paragraph("This command can be executed from any controller, or any view that has access to that " +
                    "particular PlayerViewModel instance, usually through some kind of interaction, such as a " +
                    "collision with spikes or an enemy's weapon.  Furthermore, it is important to make a " +
                    "distinction of game logic (which goes on the Controller layer of the design) and " +
                    "visual/auditory/engine-specific logic (which belongs on the View layer).");
        _.Break();
        _.Paragraph("Instead of an EnemyView detecting that it has hit the PlayerView, taking its PlayerViewModel " +
                    "instance, and executing the TakeDamage command on it directly from that EnemyView, it's " +
                    "important to make the distinction that this is game logic and belongs in the Controller " +
                    "layer.  The correct approach that most follows the MVCVM pattern would be to implement " +
                    "some kind of AttackPlayer command on the Enemy element, and pass the playerViewModel of " +
                    "the PlayerView that the EnemyView has hit.");
        _.ShowGist("2db6ead6cc89deb81a5e", "EnemyView, Hit was detected");
        _.Paragraph("Now that you're handling the game logic properly on the Controller layer, the actual logic " +
                    "is no longer dependent on that specific view, and is available to anything that tells the" +
                    " EnemyViewModel to AttackPlayer.");

        _.ShowGist("f79b3de4126c75d5bb03", "Enemy and Player Controllers Command Logic");
        _.Break();
        _.Paragraph("As you can see, there's a method to the madness.  Separating the core logic and state " +
                    "from the \"Monobehaviour\" side of things allows any number of Views to use this data. " +
                    " Under the hood, uFrame manages the state and helps centralize logic.  Everything else " +
                    "is just an expression of that state, a Player getting attacked or taking damage and " +
                    "losing health.");
    }
}
