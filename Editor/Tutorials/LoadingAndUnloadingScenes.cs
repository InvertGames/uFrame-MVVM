using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

public class LoadingAndUnloadingScenes : uFrameMVVMTutorial
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
        _.Paragraph("The purpose of this tutorial is to teach you how to publish events to the scene service for unloading, and loading of scene types.");
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

        DoNamedItemStep<CommandsChildItem>(_, "LoadB", TheGame, "a Command", null, "Game", "Commands");

        DoNamedItemStep<CommandsChildItem>(_, "UnLoadB", TheGame, "a Command",null, "Game", "Commands");


        CreateGameView(_);

        SaveAndCompile(_);

        EnsureKernel(_);

        EnsureCreateScene(_, SceneA, b =>
        {
            b.ImageByUrl("http://i.imgur.com/FK3MZKy.png");
        }, "SceneA");

        EnsureCreateScene(_, sceneB, b =>
        {
            b.ImageByUrl("http://i.imgur.com/FK3MZKy.png");
        }, "SceneA");

        EnsureSceneOpen(_, SceneA, null,"SceneA");

        EnsureCode(_, TheGame,
            "Now add the code to the game controller to load and unload Scene B.",
            "http://i.imgur.com/Yrg3jNI.png",
            "Controller.cs",
            "LoadSceneCommand");


        AddViewToScene(_,GameView);
    }
}
public class UsingSceneLoaders : LoadingAndUnloadingScenes
{

    public override decimal Order
    {
        get { return 2; }
    }

    public override bool ShowInNavigation
    {
        get { return false; } 
    }

    protected override void DoTutorial(IDocumentationBuilder _)
    {
        base.DoTutorial(_);
        //EnsureCode(_, SceneA, "Now add the following code to SceneALoader.cs", "http://i.imgur.com/dQknvBt.png", "Loader.cs", "CreatePrimitive");
    }

}