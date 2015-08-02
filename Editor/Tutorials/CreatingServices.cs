using Invert.Core.GraphDesigner;
using Invert.StateMachine;
using Invert.uFrame.MVVM;
using uFrame.Kernel;

public class CreatingServices : uFrameMVVMTutorial
{
    public override decimal Order
    {
        get { return 3; }
    }


    protected override void TutorialContent(IDocumentationBuilder _)
    {
        BasicSetup(_);

        CreateGameElement(_);
        CreateGameView(_);

        // var debugService = DoNamedNodeStep<ServiceNode>(_, "DebugService");
        var graph = DoGraphStep<ServiceGraph>(_,"DebugService", b => { });
        var debugService = graph == null ? null : graph.RootFilter as ServiceNode;
        var logEvent = DoNamedNodeStep<SimpleClassNode>(_, "LogEvent");
        DoNamedItemStep<PropertiesChildItem>(_, "Message", logEvent, "a property", b => { },"LogEvent","Properties");
        DoNamedItemStep<HandlersReference>(_, "LogEvent", debugService, "a handler", b => { },"DebugService","Handlers");
        DoNamedItemStep<CommandsChildItem>(_, "Log", TheGame, "a command", null,"Game","Commands");
        SaveAndCompile(_);
        EnsureKernel(_);
        EnsureCode(_, debugService, "Open DebugService.cs and implement the LogEventHandler method.", "http://i.imgur.com/Vrdqgx4.png", "DebugService", "Debug.Log");
        EnsureCode(_, TheGame, "Open GameController.cs and implement the Log method.", "http://i.imgur.com/t2zwBZv.png", "GameController", "new LogEvent");
        
    }

    protected override void Introduction(IDocumentationBuilder _)
    {
        
    }

    protected override void Ending(IDocumentationBuilder _, InteractiveTutorial tutorial)
    {
        _.ImageByUrl("http://i.imgur.com/iprda4t.png");
        _.Paragraph("Now run the game from SceneA, select the gameobject and hit the LoadB button and UnLoadB button from the GameView's inspector.");
    }
}

//public class UsingStateMachines : uFrameMVVMTutorial
//{
//    public override decimal Order
//    {
//        get { return 10; }
//    }

//    protected override void TutorialContent(IDocumentationBuilder _)
//    {
//        BasicSetup(_);
//        CreateGameElement(_);
//        CreateGameView(_);

//        //StateGraph = DoGraphStep<StateMachineGraph>(_, "GameFlow");
//        StateMachineNode = DoNamedNodeStep<StateMachineNode>(_, "GameFlow", TheGame);

//        BeginGameTransition = DoNamedItemStep<TransitionsChildItem>(_, "BeginGame", StateMachineNode, "a transition", b =>
//        {
            
//            b.Paragraph("We need to register a transition to the state machine node.");
//            b.ImageByUrl("http://i.imgur.com/swPtoys.png");
//        });

//        PlayCommand = DoNamedItemStep<CommandsChildItem>(_, "Play", TheGame, "a command", b =>
//        {
//            b.Paragraph("Now we need to add a command that will trigger the transition.");
//        });

//        DoCreateConnectionStep(_, PlayCommand, BeginGameTransition, b =>
//        {

//            b.Paragraph("Creating this connection means that when the command is invoked it will trigger the transition automatically.");
//            b.ImageByUrl("http://i.imgur.com/LEMP06i.png");
//        });

//        StateMachineProperty = DoNamedItemStep<PropertiesChildItem>(_, "GameFlow", TheGame, "a property", b =>
//        {
//            b.Paragraph("State machines live on the view-model. So we need to create a property on our element node for the state machine.");
//        });
//        DoCreateConnectionStep(_, StateMachineProperty, StateMachineNode, b =>
//        {
//            b.ImageByUrl("http://i.imgur.com/LEMP06i.png");
//        });
//        //StateGraph == null ? null : StateGraph.RootFilter as StateMachineNode;
//        MainMenuState = DoNamedNodeStep<StateNode>(_, "MainMenu",StateMachineNode);
//        PlayingGameState = DoNamedNodeStep<StateNode>(_, "PlayingGame", StateMachineNode);

      
//        BeginGameStateTransition = DoNamedItemStep<StateTransitionsReference>(_, "BeginGame", MainMenuState, "a state transition", b =>
//        {
//            b.Paragraph("Now we need to add the transition to the MainMenu state.");
//        });
//        DoCreateConnectionStep(_, StateMachineNode == null ? null : StateMachineNode.StartStateOutputSlot, MainMenuState);

//        DoCreateConnectionStep(_, BeginGameStateTransition, 
//            PlayingGameState
//            );

//        DoNamedItemStep<BindingsReference>(_, "GameFlow State Changed", GameView, "a binding", b =>
//        {
//            b.ImageByUrl("http://i.imgur.com/uHAfHEM.png");

//        });

//        SaveAndCompile(_);
//        EnsureKernel(_);
//        AddViewToScene(_,GameView);
//    }

//    public StateTransitionsReference BeginGameStateTransition { get; set; }

//    public PropertiesChildItem StateMachineProperty { get; set; }

//    public CommandsChildItem PlayCommand { get; set; }

//    public TransitionsChildItem BeginGameTransition { get; set; }

//    public StateMachineNode StateMachineNode { get; set; }

//    public StateNode PlayingGameState { get; set; }

//    public StateNode MainMenuState { get; set; }

//    public StateMachineGraph StateGraph { get; set; }

//    protected override void Introduction(IDocumentationBuilder _)
//    {
        
//    }

//    protected override void Ending(IDocumentationBuilder _, InteractiveTutorial tutorial)
//    {
        
//    }
//}

public class TheBasics : uFrameMVVMTutorial
{
    public override decimal Order
    {
        get { return -1; }
    }

    protected override void TutorialContent(IDocumentationBuilder _)
    {

        //Step 1
        TheProject = DoCreateNewProjectStep(_, "TheBasicsProject");
        
        //Step 2
        EnsureNamespace(_,"TheBasicsProject");

        //Step 3
        SubsystemGraph = DoGraphStep<SubsystemGraph>(_, "BasicsSystem");
        
        if (SubsystemGraph != null)
        {
            SystemA = SubsystemGraph.RootFilter as SubsystemNode;
        }
        
        //Step 4
        CreatePlayerElement(_);
        
        //Step 5
        CreatePlayerView(_);
        NameProperty = DoNamedItemStep<PropertiesChildItem>(_, "Name", ThePlayer, "a property", b =>
        {
            b.Paragraph("After you finish this step, your node should look like this:");
            b.ImageByUrl("http://i.imgur.com/wJi2IZP.png","This picture shows the state of Player node after you finish current step.");
        }, "Player","Properties");

        ResetCommand = DoNamedItemStep<CommandsChildItem>(_, "Reset", ThePlayer, "a command", b =>
        {
            b.ImageByUrl("http://i.imgur.com/ZktA9FP.png");
        }, "Player" ,"Commands");

        NameChangedBinding = DoNamedItemStep<BindingsReference>(_, "Name Changed", ThePlayerView, "a binding", b =>
        {
            b.ImageByUrl("http://i.imgur.com/9K08Woe.png");

        },"PlayerView","Bindings");
        SaveAndCompile(_, ThePlayerView);
        EnsureKernel(_);
        CreateDefaultScene(_);
        AddViewToScene(_, ThePlayerView);
        EnsureInitializeView(_,ScenePlayerView);
        EnsureCode(_, ThePlayer, "Open the player controller by right-clicking on the 'Player' node and choosing Open->PlayerController.cs and add the following code.", "http://i.imgur.com/gjFLEeD.png","PlayerController.cs", ".Name");
        EnsureCode(_, ThePlayerView, "Open the player view and add the following code.", "http://i.imgur.com/rV97qSC.png", "PlayerView.cs", "Debug.Log");
    }

    public void CreateDefaultScene(IDocumentationBuilder builder)
    {
        EnsureComponentInSceneStep<Scene>(builder, null,
            "Create an empty scene, create an empty game object and add the 'Scene' component to it.",
            b =>
            {
                b.ImageByUrl("http://i.imgur.com/5Pnd9Xf.png");
                b.Paragraph("In this example we are using \"Scene\" component, which is in fact a common SceneType. It does not belong to any project, that is why you have to manually specify the kernel scene." +
                       " Later you will learn how to define scene types for a specific project. Those will get kernel scene name automatically, and you won't have to specify it manually.");
                b.Note("Make sure to set KernelScene to \""+TheProject.Name+"KernelScene\" and do not forget to add this scene into the build settings!");
                    
            });
        
    }
    public BindingsReference NameChangedBinding { get; set; }

    public CommandsChildItem ResetCommand { get; set; }

    public PropertiesChildItem NameProperty { get; set; }

    public SubsystemGraph SubsystemGraph { get; set; }

    protected override void Introduction(IDocumentationBuilder _)
    {
        
    }

    protected override void Ending(IDocumentationBuilder _, InteractiveTutorial tutorial)
    {
        _.Paragraph("Now run the game, select the PlayerView gameobject, and type 'Hello World' into the Name field, then press the reset button.  You should also see the console log each change to the property 'Name'");
        _.ImageByUrl("http://i.imgur.com/zrg6r7q.png");

        _.AlsoSeePages(typeof(LoadingAndUnloadingScenes), typeof(CreatingServices));
        

    }
}