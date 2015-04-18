using System;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

public class LoadingAndUnloadingScenes : UFrameMvvmTutorial
{
    public override decimal Order
    {
        get { return 1; }
    }
    protected override void TutorialContent(IDocumentationBuilder _)
    {
        DoTutorial(_);
    }

    protected override void Introduction(IDocumentationBuilder _)
    {
        _.Paragraph("The purpose of this tutorial is to teach you the high level aspects of using uFrame, this includes SubSystems, and Scenes");
        _.Break();
    }

    protected override void Ending(IDocumentationBuilder _, InteractiveTutorial tutorial)
    {
        _.ImageByUrl("http://i.imgur.com/iprda4t.png");
        _.Paragraph("Now run the game from SceneA, select the gameobject and hit the LoadB button and UnLoadB button from the GameView's inspector.");
        _.LinkToPage<UsingSceneLoaders>();
    }

    protected virtual void DoTutorial(IDocumentationBuilder _)
    {
        BasicSetup(_);

        //DoNamedNodeStep<SubsystemNode>(_, "SystemB");
        var sceneB = DoNamedNodeStep<SceneTypeNode>(_, "SceneB");

        CreateGameElement(_);

        DoNamedItemStep<CommandsChildItem>(_, "LoadB", TheGame, "a Command",
            b => b.Paragraph("Now we need to add a command to load scene B."));

        DoNamedItemStep<CommandsChildItem>(_, "UnLoadB", TheGame, "a Command",
            b => b.Paragraph("Now we need to add a command to un-load scene B."));


        CreateGameView(_);



        SaveAndCompile(_);

        EnsureKernel(_);

        EnsureCreateScene(_, SceneA);

        EnsureCreateScene(_, sceneB);


        EnsureSceneOpen(_, SceneA);

        EnsureCode(_, TheGame,
            "Now add the code to the game controller to load and unload Scene B.",
            "http://i.imgur.com/Yrg3jNI.png",
            "Controller.cs",
            "LoadSceneCommand");


        EnsureComponentInSceneStep<ViewBase>(_, GameView,
            "Now add the GameView to the scene.",
            b =>
            {
                b.Paragraph("Create an empty gameObject underneath the _SceneARoot game object.  " +
                            "When creating scene types, everything should be a descendent of this root game object, " +
                            "this allows them to be destroyed by uFrame when needed.");
                b.Paragraph("On this empty game object click 'Add Component' in the inspector. Then add the 'GameView' component to it.");

                b.ImageByUrl("http://i.imgur.com/3pKo4yL.png");
            });




    }
}
public class UsingSceneLoaders : LoadingAndUnloadingScenes
{

    public override decimal Order
    {
        get { return 2; }
    }
    protected override void DoTutorial(IDocumentationBuilder _)
    {
        base.DoTutorial(_);
        EnsureCode(_, SceneA, "Now add the following code to SceneALoader.cs", "http://i.imgur.com/dQknvBt.png", "Loader.cs", "CreatePrimitive");
    }

}
public class CreatingServices : UFrameMvvmTutorial
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
        var graph = DoGraphStep<ServiceGraph>(_, b => { });
        var debugService = graph == null ? null : graph.RootFilter as ServiceNode;
        var logEvent = DoNamedNodeStep<SimpleClassNode>(_, "LogEvent");
        var logEventMessage = DoNamedItemStep<PropertiesChildItem>(_, "Message", logEvent, "a property", b => { });
        var logEventHandler = DoNamedItemStep<HandlersReference>(_, "LogEvent", debugService, "a handler", b => { });
        SaveAndCompile(_);
        EnsureCode(_, debugService, "Open DebugService.cs and implement the LogEventHandler method.", "http://i.imgur.com/Vrdqgx4.png", "DebugService", "Debug.Log");
        EnsureKernel(_);
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


public class CreatingViews : UFrameMvvmTutorial
{
    protected override void TutorialContent(IDocumentationBuilder _)
    {
        GameBasicSetup(_);
        var propertyBinding = DoNamedItemStep<BindingsReference>(_, "FirstName Changed", GameView, "a property binding", b => { });
        SaveAndCompile(_);
        EnsureKernel(_);
        //EnsureCode(_, GameView, "Open GameView.cs and implement the FirstName Changed method.", "http://i.imgur.com/Vrdqgx4.png", "", "Debug.Log");
    }

    protected override void Introduction(IDocumentationBuilder _)
    {
  
    }

    protected override void Ending(IDocumentationBuilder _, InteractiveTutorial tutorial)
    {
  
    }
}
public class TheKernel : uFrameMVVMPage
{
    public override decimal Order
    {
        get { return -1; }
    }

    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        _.Paragraph("The uFrame Kernel is an essential piece of uFrame, it handles loading scenes, systems and services.  " +
                    "The kernel is nothing more than a prefab with these types of components attached to it in an organized manner.");



        _.ImageByUrl("http://i.imgur.com/5Rg2X25.png");

        _.Paragraph("In the image above you can see the scene 'BasicsProjectKernelScene'.  This scene will always always contain the 'BasicsProjectKernel' " +
                    "prefab and any other things that need to live throughout the entire lifecycle of your application.");
        _.Paragraph("Important Note: All SystemLoaders, Services, and SceneLoaders are MonoBehaviours attached their corresponding child game-objects in the kernel prefab.");

        _.Note(
            "Whenever a scene begins, uFrame will ensure that the kernel is loaded, if it hasen't been loaded it will delay " +
            "its loading mechanism until the kernel has been loaded. This is necessary because you might initialize ViewModels, deserialize them...etc so the view should not bind until that data is ready.");

        _.Break();

        _.Title2("Scaffolding The Kernel");
        _.Paragraph("For convenience uFrame 1.6 makes the process of creating the kernel very easy and straightforward.  " +
                    "By pressing the Scaffold/Update Kernel button it will create a scene, and a prefab with all of the types created by the uFrame designer.  " +
                    "You can freely modify the kernel, and updating it will only add anything that is not there.");
        _.Break();

        _.Title2("Boot Order");
        _.ImageByUrl("http://i.imgur.com/L5CC8q8.png");


        _.Title2("The Game Ready Event");
        _.Paragraph("Once the kernal has loaded, it will publish the event 'GameReadyEvent'.  This event ensures that everything is loaded and bindings have properly taken place on views.");



    }
}

public class SceneTypePage : SceneTypePageBase
{
    public override decimal Order
    {
        get { return 2; }
    }

    public override string Name
    {
        get { return "Scene Types"; }
    }

    public override Type ParentPage
    {
        get { return typeof(TheKernel); }
    }

    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);

        var graph = new ScaffoldGraph();
        var node = graph.BeginNode<SceneTypeNode>("UIScene").EndNode() as SceneTypeNode;

        _.Title2("Generated Scene Types");
        _.Paragraph("The scene type is a mono behaviour that will go on your root scene object.  This allows uFrame to associate a game object so it can easily be destroyed when you want to unload a scene.  This also allows uFrame to listen for when the scene has actually been loaded.");
        _.Break();
        _.TemplateExample<SceneTemplate, SceneTypeNode>(node, true);
        _.Break();
        _.Title2("Generated Scene Loaders");
        _.Paragraph("A scene loader is generated for every scene type that exists in the graph.");
        _.Paragraph("The scene loader lives as a gameobject on the uFrame Kernel, when the same corresponding 'Scene Type' has been loaded," +
                    " the scene loader will get a reference to the scene type and allow you to load it accordingly.  This gives very fine grained " +
                    "control on how scenes are loaded and unloaded.");
        _.Break();
        _.TemplateExample<SceneLoaderTemplate, SceneTypeNode>(node, true);

    }
}