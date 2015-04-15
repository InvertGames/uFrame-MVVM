using System;
using System.IO;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using UnityEditor;
using UnityEngine;

public class TheBasics : uFrameMVVMPage<InteractiveTutorials>
{
    public sealed override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        _.Paragraph("The purpose of this tutorial is to teach you the high level aspects of using uFrame, this includes SubSystems, and Scenes");
        _.Break();
        _.BeginTutorial("The Basics : Systems, Scenes, and The Kernel");
        DoTutorial(_);

        _.EndTutorial();
    }

    protected virtual void DoTutorial(IDocumentationBuilder _)
    {
        if (BasicSetup(_)) return;

        var systemB = DoNamedNodeStep<SubsystemNode>(_, "SystemB");
        if (systemB == null) return ;

        var sceneB = DoNamedNodeStep<SceneTypeNode>(_, "SceneB");
        if (sceneB == null) return ;
        
        if (CreatePlayerElement(_)) return;

        var loadSceneB = DoNamedItemStep<CommandsChildItem>(_, "LoadB", ThePlayer, "a Command",
            b => b.Paragraph("Now we need to add a command to load the scene B."));
        if (loadSceneB == null) return;

        var unloadSceneB = DoNamedItemStep<CommandsChildItem>(_, "UnLoadB", ThePlayer, "a Command",
            b => b.Paragraph("Now we need to add a command to un-load the scene B."));
        if (unloadSceneB == null) return;

        ThePlayerView = DoNamedNodeStep<ViewNode>(_, "PlayerView", ThePlayer);
        if (ThePlayerView == null) return;

        var connection = DoCreateConnectionStep(_, ThePlayer, ThePlayerView.ElementInputSlot);
        if (connection == null) return;

        SaveAndCompile(_);
         
        if (EnsureKernel(_)) return;

        if (!EnsureCreateScene(_, SceneA))
            return;

        if (!EnsureCreateScene(_, sceneB))
            return;


        if (EnsureSceneOpen(_, SceneA)) return;
        _.ShowTutorialStep(new TutorialStep("Now edit the 'PlayerController''s 'LoadB' and 'UnLoadB' command.",
            ()=> EnsureCodeInEditableFile(ThePlayer, "Controller", "LoadSceneCommand")), b =>
            {
                _.ImageByUrl("http://i.imgur.com/oSvu1Ay.png");
            });
        
        
        EnsureComponentInSceneStep<ViewBase>(_, ThePlayerView, b =>
        {
            b.Paragraph("Create an empty gameObject underneath the _SceneARoot game object.  " +
                        "When creating scene types, everything should be a descendent of this root game object, " +
                        "this allows them to be destroyed by uFrame when needed.");
            b.Paragraph("On this empty game object click 'Add Component' in the inspector. Then add the 'PlayerView' component to it.");

        });
    }

    public ViewNode ThePlayerView { get; set; }

    protected bool CreatePlayerElement(IDocumentationBuilder _)
    {
        ThePlayer = DoNamedNodeStep<ElementNode>(_, "Player", SystemA, b => { });
        if (ThePlayer == null) return true;
        return false;
    }

    public ElementNode ThePlayer { get; set; }

    protected bool EnsureKernel(IDocumentationBuilder _)
    {
        var kernel = this.EnsureScaffoldKernel(_, TheProject, ExplainKernel);
        if (kernel == null) return true;
        return false;
    }

    protected void SaveAndCompile(IDocumentationBuilder _)
    {
        _.ShowTutorialStep(SaveAndCompile(SceneA));
    }

    protected bool BasicSetup(IDocumentationBuilder _, string systemName = "SystemA", string sceneName = "SystemB")
    {
        TheProject = DoCreateNewProjectStep(_);
        if (TheProject == null) return true;

        TheGraph = DoGraphStep<MVVMGraph>(_);
        if (TheGraph == null) return true;

        SystemA = DoNamedNodeStep<SubsystemNode>(_, "SystemA");
        if (SystemA == null) return true;
        SceneA = DoNamedNodeStep<SceneTypeNode>(_, "SceneA");
        if (SceneA == null) return true;
        return false;
    }

    public SceneTypeNode SceneA { get; set; }

    public SubsystemNode SystemA { get; set; }

    public MVVMGraph TheGraph { get; set; }

    public IProjectRepository TheProject { get; set; }
    public bool EnsureSceneOpen(IDocumentationBuilder _, SceneTypeNode scene, Action<IDocumentationBuilder> stepContent = null)
    {
        
        var exists =EditorApplication.currentScene.EndsWith(scene.Name + ".unity");
        _.ShowTutorialStep(new TutorialStep(string.Format("Open the scene '{0}'", scene.Name), () =>
        {
            return exists
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
        return exists;
    }
    public bool EnsureCreateScene(IDocumentationBuilder _, SceneTypeNode scene, Action<IDocumentationBuilder> stepContent = null)
    {
        var paths = scene.Graph.CodePathStrategy;
        var scenePath = System.IO.Path.Combine(paths.ScenesPath, scene.Name + ".unity");
        var exists = File.Exists(scenePath);
        _.ShowTutorialStep(new TutorialStep(string.Format("Create the scene for scene type '{0}'", scene.Name), () =>
        {
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
        return exists;
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
    private void ExplainKernel(IDocumentationBuilder _)
    {
        _.ToggleContentByPage<TheKernel>("The Kernel");
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

        _.Note(
            "Whenever a scene begins, uFrame will ensure that the kernel is loaded, if it hasen't been loaded it will delay " +
            "its loading mechanism until the kernel has been loaded. This is necessary because you might initialize ViewModels, deserialize them...etc so the view should not bind until that data is ready.");

        _.Break();

        _.Title2("Scaffolding The Kernel");
        _.Paragraph("For convenience uFrame 1.6 makes the process of creating the kernel very easy and straightforward.");
        _.Break();

        _.Title2("Boot Order");
        _.Paragraph(" - First it will load every system loader that has been added to it. uFrame generates loaders for " +
                    "each subsystem, so in most cases this is where your controllers will be instantiated and registered.");
        _.Paragraph(" - Secondly, it will load each and every service you have added to the kernel's 'Services' child gameobject.");
        _.Paragraph(" - Next it will invoke Setup on every controller that has been registered in the first step.");

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
   
}

public class SceneLoaders : uFrameMVVMPage<SceneTypePage>
{
    
}