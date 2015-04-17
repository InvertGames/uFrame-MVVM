using System;
using System.IO;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using UnityEditor;
using UnityEngine;

public abstract class uFrameMVVMTutorial : uFrameMVVMPage<InteractiveTutorials>
{
    public SceneTypeNode SceneA { get; set; }

    public SubsystemNode SystemA { get; set; }

    public MVVMGraph TheGraph { get; set; }

    public IProjectRepository TheProject { get; set; }
    public void EnsureSceneOpen(IDocumentationBuilder _, SceneTypeNode scene, Action<IDocumentationBuilder> stepContent = null)
    {


        _.ShowTutorialStep(new TutorialStep(scene == null ? "Open The Scene" : string.Format("Open the scene '{0}'", scene.Name), () =>
        {
            return EditorApplication.currentScene.EndsWith(scene.Name + ".unity")
                ? null
                : string.Format(
                    "You have not opened the scene '{0}'. It is located in the 'Scenes' folder of your project.", scene.Name);
        })
        {
            StepContent = b =>
            {
                if (stepContent != null)
                {
                    stepContent(b);
                }
            }
        });

    }
    public void EnsureCreateScene(IDocumentationBuilder _, SceneTypeNode scene, Action<IDocumentationBuilder> stepContent = null)
    {

        _.ShowTutorialStep(new TutorialStep(scene == null ? "Create the scene" : string.Format("Create the scene for scene type '{0}'", scene.Name), () =>
        {
            var paths = scene.Graph.CodePathStrategy;
            var scenePath = System.IO.Path.Combine(paths.ScenesPath, scene.Name + ".unity");
            var exists = File.Exists(scenePath);
            return exists
                ? null
                : string.Format(
                    "The scene does not exist yet. Right click on the '{0}' node and select 'Create Scene'", scene.Name);
        })
        {
            StepContent = b =>
            {
                if (stepContent != null)
                {
                    stepContent(b);
                }
            }
        });

    }
    public uFrameMVVMKernel EnsureScaffoldKernel(IDocumentationBuilder builder, IProjectRepository projectRepository, Action<IDocumentationBuilder> stepContent = null)
    {

        var project = projectRepository as DefaultProjectRepository;

        var path = AssetDatabase.GetAssetPath(project);
        var prefabName = project.Name + "Kernel.prefab";
        var prefabNameWithPath = path.Replace(project.name + ".asset", prefabName);


        var go = AssetDatabase.LoadAssetAtPath(prefabNameWithPath, typeof(GameObject)) as GameObject;
        var component = go == null ? null : go.GetComponent<uFrameMVVMKernel>();

        builder.ShowTutorialStep(new TutorialStep("Now create the we need to create the kernel.", () =>
        {

            if (component == null)
            {
                return "The Kernel Prefab has not been created yet.  Please press 'Scaffold/Update Kernel'.";
            }
            return null;
        })
        {
            StepContent = stepContent
        });
        return component;
    }

    protected bool EnsureKernel(IDocumentationBuilder _)
    {
        var kernel = this.EnsureScaffoldKernel(_, TheProject, ExplainKernel);
        if (kernel == null) return false;
        return true;
    }
    private void ExplainKernel(IDocumentationBuilder _)
    {
        _.ToggleContentByPage<TheKernel>("The Kernel");
    }
    public ElementNode TheGame { get; set; }
    public ViewNode GameView { get; set; }
    protected void SaveAndCompile(IDocumentationBuilder _)
    {
        _.ShowTutorialStep(SaveAndCompile(SceneA));
    }

    public void EnsureCode(IDocumentationBuilder _, DiagramNode codeFor, string description, string imageUrl, string filenameSearch, string codeSearch)
    {

        _.ShowTutorialStep(new TutorialStep(description,
            () => EnsureCodeInEditableFile(codeFor, filenameSearch, codeSearch)),
            b => { _.ImageByUrl(imageUrl); });

    }

    protected bool BasicSetup(IDocumentationBuilder _, string systemName = "SystemA", string sceneName = "SystemB")
    {
        TheProject = DoCreateNewProjectStep(_);
        if (TheProject == null) return false;

        TheGraph = DoGraphStep<MVVMGraph>(_);
        if (TheGraph == null) return false;

        SystemA = DoNamedNodeStep<SubsystemNode>(_, "SystemA");
        if (SystemA == null) return false;
        SceneA = DoNamedNodeStep<SceneTypeNode>(_, "SceneA");
        if (SceneA == null) return false;
        return true;
    }
    protected bool CreateGameElement(IDocumentationBuilder _)
    {
        TheGame = DoNamedNodeStep<ElementNode>(_, "Game", SystemA, b => { });
        if (TheGame == null) return false;
        return true;
    }

    protected void CreateGameView(IDocumentationBuilder _)
    {
        GameView = DoNamedNodeStep<ViewNode>(_, "GameView", TheGame); 
        DoCreateConnectionStep(_, TheGame, GameView == null ? null : GameView.ElementInputSlot);
    }
}

public class TheBasics : uFrameMVVMTutorial
{
    public sealed override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        _.Paragraph("The purpose of this tutorial is to teach you the high level aspects of using uFrame, this includes SubSystems, and Scenes");
        _.Break();
        _.BeginTutorial("The Basics : Systems, Scenes, and The Kernel");
        DoTutorial(_);
         var tutorial = _.EndTutorial();

        if (tutorial.LastStepCompleted)
        {
            _.ImageByUrl("http://i.imgur.com/iprda4t.png");
            _.Paragraph("Now run the game from SceneA, select the gameobject and hit the LoadB button and UnLoadB button from the GameView's inspector.");
        }
        

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

public class CreatingServices : uFrameMVVMTutorial
{
    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        _.BeginTutorial("Creating Services");
        
        BasicSetup(_);

        CreateGameElement(_);
        CreateGameView(_);
        
       // var debugService = DoNamedNodeStep<ServiceNode>(_, "DebugService");
        var graph = DoGraphStep<ServiceGraph>(_, b => { });
        var debugService = graph.RootFilter as ServiceNode;
        var logEvent = DoNamedNodeStep<SimpleClassNode>(_, "LogEvent");
        var logEventMessage = DoNamedItemStep<PropertiesChildItem>(_, "Message", logEvent, "a property", b => { });
        var logEventHandler = DoNamedItemStep<HandlersReference>(_, "LogEvent", debugService, "a handler", b => { });



        var tutorial = _.EndTutorial();
        if (tutorial.LastStepCompleted)
        {
            _.ImageByUrl("http://i.imgur.com/iprda4t.png");
            _.Paragraph("Now run the game from SceneA, select the gameobject and hit the LoadB button and UnLoadB button from the GameView's inspector.");
        }
        

        
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
        get { return typeof (TheKernel); }
    }

    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);

        var graph = new ScaffoldGraph();
        var node = graph.BeginNode<SceneTypeNode>("UIScene").EndNode() as SceneTypeNode;
        _.Title2("Scene Loader Designer File");
        _.TemplateExample<SceneTemplate, SceneTypeNode>(node, true);
        _.Break();
        _.Title2("Scene Loader File");
        _.TemplateExample<SceneTemplate, SceneTypeNode>(node, false);
    }
}

public class SceneLoaders : uFrameMVVMPage<SceneTypePage>
{
    public override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);

    }
}