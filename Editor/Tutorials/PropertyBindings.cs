using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;

//public class PropertyBindings : uFrameMVVMTutorial
//{
//    public override decimal Order
//    {
//        get { return 4; }
//    }

//    protected override void TutorialContent(IDocumentationBuilder _)
//    {
//        BasicSetup(_);
        
//        PlayerSetup(_);

//        DoNamedItemStep<PropertiesChildItem>(_, "FirstName", ThePlayer, "a property", b => { });
//        DoNamedItemStep<CommandsChildItem>(_, "LoadName", ThePlayer, "a command", b => { });

//        DoNamedItemStep<BindingsReference>(_, 
//            "FirstName Changed", ThePlayerView, "a property binding", b => { });

//        SaveAndCompile(_,ThePlayerView);
//        EnsureKernel(_);
//        EnsureCode(_, ThePlayerView, "Open PlayerView.cs and implement the FirstName Changed method.", "http://i.imgur.com/hOKFYMk.png", "View.cs", "Debug.Log");
//        EnsureCode(_, ThePlayer, "Open PlayerController.cs and implement the LoadName method.", "http://i.imgur.com/qVU7Jb4.png", "Controller.cs", ".FirstName");
        
//        AddViewToScene(_, ThePlayerView);


//    }

//    protected override void Introduction(IDocumentationBuilder _)
//    {
        
//    }

//    protected override void Ending(IDocumentationBuilder _, InteractiveTutorial tutorial)
//    {
//        //http://i.imgur.com/wKUMbwc.png
//        _.ImageByUrl("http://i.imgur.com/wKUMbwc.png");
//        _.Paragraph("Now run the game from SceneA, select the gameobject and hit the LoadB button and UnLoadB button from the GameView's inspector.");
//        _.LinkToPage<ViewPage>();
//    }
//}