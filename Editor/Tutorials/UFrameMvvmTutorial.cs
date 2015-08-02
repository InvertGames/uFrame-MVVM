using System;
using System.IO;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.Kernel;
using uFrame.MVVM;
using UnityEditor;
using UnityEngine;

public abstract class uFrameMVVMTutorial : uFrameMVVMPage<InteractiveTutorials>
{
    public sealed override void GetContent(IDocumentationBuilder _)
    {
        base.GetContent(_);
        Introduction(_);
        _.BeginTutorial("Creating Services");
        TutorialContent(_);
        var tutorial = _.EndTutorial();
        if (tutorial.LastStepCompleted)
        {
            Ending(_,tutorial);
        }
    }

    protected abstract void TutorialContent(IDocumentationBuilder _);

    protected abstract void Introduction(IDocumentationBuilder _);

    protected abstract void Ending(IDocumentationBuilder _, InteractiveTutorial tutorial);

    public SceneTypeNode SceneA { get; set; }

    public SubsystemNode SystemA { get; set; }

    public MVVMGraph TheGraph { get; set; }

    public IProjectRepository TheProject { get; set; }
    public void EnsureSceneOpen(IDocumentationBuilder _, SceneTypeNode scene, Action<IDocumentationBuilder> stepContent = null, string sceneName = null)
    {


        _.ShowTutorialStep(new TutorialStep(scene == null ? "Open The Scene " + sceneName: string.Format("Open the scene '{0}'", scene.Name), () =>
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
    public void EnsureCreateScene(IDocumentationBuilder _, SceneTypeNode scene, Action<IDocumentationBuilder> stepContent = null, string sceneName = null)
    {

        _.ShowTutorialStep(new TutorialStep(scene == null ? "Create the scene " + sceneName : string.Format("Create the scene for scene type '{0}'", scene.Name), () =>
        {
            var paths = scene.Graph.Project.SystemDirectory;
            var scenesPath = System.IO.Path.Combine(paths, "Scenes");
            var scenePath = System.IO.Path.Combine(scenesPath, scene.Name + ".unity");
            var exists = File.Exists(scenePath);
            return exists
                ? null
                : string.Format(
                    "The scene does not exist yet. Right click on the '{0}' node and select 'Create Scene'", scene.Name);
        })
        {
            StepContent = b =>
            {

                b.Paragraph("In this step you need to created an instance of a scene, based on a SceneType. This means, that uFrame will automatically generate a scene with a basic setup.");
                b.Paragraph("To create an instance of a scene, you first have to locate the desired SceneType node. Right-click on the node and select 'Create Scene' ooption.");

                b.Note("There are several things that may happen while you are creating a scene:\n" +
                       "* If your current scene is not saved, uFrame will ask if you want to save it, before doing anything. Make sure you save any important data, as during this step you will be transfered to a different scene.\n" +
                       "* uFrame will first try to save scene with the name {SceneTypeName}.unity. If such scene does not exist, it will be created automatically and no further dialogs will appear. You then will be transfered to this scene.\n" +
                       "* If {SceneTypeName}.unity scene already exists, you will be prompted for the name and location for the new scene. You will then be transfered to the newly created scene."+
                       "");

                b.Note("uFrame automatically adds all the scene you create into the build!");
                if (stepContent != null)
                {
                    stepContent(b);
                }
            }
        });

    }
    public uFrameKernel EnsureScaffoldKernel(IDocumentationBuilder builder, IProjectRepository projectRepository, Action<IDocumentationBuilder> stepContent = null)
    {

        var project = projectRepository as DefaultProjectRepository;
        
        var path = project == null ? string.Empty : AssetDatabase.GetAssetPath(project);
        var prefabName = project == null ? "Kernel.prefab" : project.Name + "Kernel.prefab";
        var prefabNameWithPath = project == null ? prefabName : path.Replace(project.name + ".asset", prefabName);


        var go = AssetDatabase.LoadAssetAtPath(prefabNameWithPath, typeof(GameObject)) as GameObject;
        var component = go == null ? null : go.GetComponent<uFrameKernel>();

        builder.ShowTutorialStep(new TutorialStep("Scaffold Kernel", () =>
        {
            if (component == null)
            {
                return "The Kernel Prefab has not been created yet.  Please press 'Scaffold/Update Kernel'.";
            }
            return null;
        })
        {
            StepContent = _ =>
            {
                _.Paragraph("In this step we need to scaffold/update kernel. You can read more about kernel on the kernel page." +
                            "This step will modify/create the kernel of your project. This kernel is then used to load all the dependencies for your game.");

                _.Note("When kernel is being updated, you may get the following exception:\n" +
                       "\"InvalidOperationException: Operation is not valid due to the current state of the object\"\n" +
                        "This error is harmless and will be fixed in one of upcoming updates");

                if (stepContent != null) stepContent(_);
            }
        });
        return component;
    }

    public uFrameKernel EnsureUpdateKernelWithTypes(IDocumentationBuilder builder, IProjectRepository projectRepository, string[] types_section, Action<IDocumentationBuilder> stepContent = null)
    {

        var project = projectRepository as DefaultProjectRepository;

        var path = project == null ? string.Empty : AssetDatabase.GetAssetPath(project);
        var prefabName = project == null ? "Kernel.prefab" : project.Name + "Kernel.prefab";
        var prefabNameWithPath = project == null ? prefabName : path.Replace(project.name + ".asset", prefabName);


        var go = AssetDatabase.LoadAssetAtPath(prefabNameWithPath, typeof(GameObject)) as GameObject;
        var component = go == null ? null : go.GetComponent<uFrameKernel>();

        builder.ShowTutorialStep(new TutorialStep("Update Kernel", () =>
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
        Kernel = this.EnsureScaffoldKernel(_, TheProject, ExplainKernel);
        if (Kernel == null) return false;

        return true;
    }

    public uFrameKernel Kernel { get; set; }

    private void ExplainKernel(IDocumentationBuilder _)
    {
        _.ImageByUrl("http://i.imgur.com/349glV8.png");
        _.ToggleContentByPage<TheKernel>("The Kernel");
    }
    public ElementNode TheGame { get; set; }
    public ViewNode GameView { get; set; }
    protected void SaveAndCompile(IDocumentationBuilder _, DiagramNode node = null)
    {
        _.ShowTutorialStep(SaveAndCompile(node ?? SceneA), b =>

        {
            b.Paragraph("In this step you need to initiate the process of code generation. This is done using Save and Compile button in the top right corner of the graph designer window. Saving and compiling is the process of taking the information you've created by graphs and putting it into code form.  The generated code consists of both designer files (always regenerated), and editable files (only generated if it doesnt exist so it doesn't destroy your implementations).");

            b.Note(" It does not matter, what graph you are currently in. Save and Compile procedure is Project-specific and is performed for all the graphs in the project at once. So pay attention to what project is currently selected.");
       
            b.Note(" If you face with compiling issues after you save and compile, first of all, determine where the issue comes from. If it is an editable file: you can fix it manually and uFrame will not erase your fix on next code generation." +
                   " If the issue comes from designer files (which gets regenerated each time), you need to understand if an issue is related to your diagram. Sometimes, type references may have invalid types, or elements may have incorrectly named properties." +
                   " Such diagram issues may result into generated code not compiling.");

            b.ImageByUrl("http://i.imgur.com/QhfMGSq.png","This picture shows \"Save and Compile\" button");
           
        
        });
    }

    public void EnsureCode(IDocumentationBuilder _, DiagramNode codeFor, string description, string imageUrl, string filenameSearch, string codeSearch)
    {

        _.ShowTutorialStep(new TutorialStep(description,
            () => EnsureCodeInEditableFile(codeFor, filenameSearch, codeSearch)),
            b =>
            {
                _.Paragraph(string.Format("Right-Click on the Node and choose Open->{0}", filenameSearch));
                _.ImageByUrl(imageUrl);
            });
    }

    protected void EnsureNamespace(IDocumentationBuilder _, string namespaceName)
    {
        _.ShowTutorialStep(new TutorialStep(string.Format("Set the project namespace to \"{0}\".", namespaceName), () =>
        {
            if (TheProject.Namespace != namespaceName)
            {
                return
                    "The current namespace is not set yet.  Navigate to the project repository, and set the namespace property.";

            }
            return null;
        }), b =>
        {

            b.Paragraph("uFrame allows you to set specific namespace for each project. Any generated code will then belong to this namespace." +
                        "In this step we need to change namespace of {0}. For this, select your project repository asset file and refer to Unity inspector window." +
                        "You will find namespace setting. Type in the namespace you want for your project. ",TheProject.Name);
            
            b.ImageByUrl("http://i.imgur.com/JlgCZGD.png");

            b.Note("Remember, that project namespace setting will be used for generated code, so avoid any keywords or illegal characters.");

        });
   
    }
    protected bool BasicSetup(IDocumentationBuilder _, string systemName = "SystemA", string sceneName = "SystemB")
    {
        TheProject = DoCreateNewProjectStep(_, this.GetType().Name + "Project");
        if (TheProject == null) return false;

        TheGraph = DoGraphStep<MVVMGraph>(_,"MainDiagram");
        if (TheGraph == null) return false;

        EnsureNamespace(_, this.GetType().Name + "Project");

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
        GameView = DoNamedNodeStep<ViewNode>(_, "GameView", TheGame, b =>
        {
            _.Paragraph("You must enter the 'Game' element context to create its view, you can do this by double-clicking on it.");
        });
        DoCreateConnectionStep(_, TheGame, GameView == null ? null : GameView.ElementInputSlot,"Game output", "GameView element input");
    }
    protected bool CreatePlayerElement(IDocumentationBuilder _)
    {
        ThePlayer = DoNamedNodeStep<ElementNode>(_, "Player", SystemA, b =>
        {
            b.ImageByUrl("http://i.imgur.com/lEnVVQj.png","This picture shows how to create Element Node");
            b.ImageByUrl("http://i.imgur.com/SJ0zI8w.png","This picture shows the final state of a newly created node, renamed to \"Player\"");
        });
        if (ThePlayer == null) return false;
        return true;
    }

    public ElementNode ThePlayer { get; set; }

    protected void CreatePlayerView(IDocumentationBuilder _)
    {
        ThePlayerView = DoNamedNodeStep<ViewNode>(_, "PlayerView", ThePlayer, b =>
        {
            b.ImageByUrl("http://i.imgur.com/H1OzzfC.png");
        });
        DoCreateConnectionStep(_, ThePlayer, ThePlayerView == null ? null : ThePlayerView.ElementInputSlot,"Player node output","PlayerView node Element input","Player node","PlayerView node", b =>
        {
            
            b.Paragraph("Creating this connection means that the view will visually present the data on the player to the user in the 3d world.");
            b.Paragraph("You can always create multiple views to seperate different presentations of the same data. For instance, you could also create a PlayerUIView, which deals strictly with showing inventory, stats,...etc");

            b.Break();
            b.ImageByUrl("http://i.imgur.com/scVtfd9.png");
        });
    }

    public ViewNode ThePlayerView { get; set; }

    protected void GameBasicSetup(IDocumentationBuilder _)
    {
        BasicSetup(_);
       GameSetup(_);
    }

    protected void GameSetup(IDocumentationBuilder _)
    {
        CreateGameElement(_);
        CreateGameView(_);
    }
    protected void PlayerSetup(IDocumentationBuilder _)
    {
    
        CreatePlayerElement(_);
        CreatePlayerView(_);
    }

    protected void AddViewToScene(IDocumentationBuilder _, ViewNode view)
    {
        ScenePlayerView = EnsureComponentInSceneStep<ViewBase>(_, view ?? GameView,
        string.Format("Now add the {0} to the scene.", view == null ? "view" : view.Name),
        b =>
        {
            b.Paragraph("Create an empty gameObject underneath the _SceneARoot game object.  ");

            b.Paragraph(
                string.Format("On this empty game object click 'Add Component' in the inspector. Then add the '{0}' component to it.", view == null ? "view" : view.Name));

            b.Note("When creating scene types, everything should be a descendent of this root game object, " +
                        "this allows them to be destroyed by uFrame when needed.");

            b.ImageByUrl("http://i.imgur.com/3pKo4yL.png");
        });
    }

    protected void EnsureInitializeView(IDocumentationBuilder _, ViewBase view, string additionalMessage = null)
    {
        _.ShowTutorialStep(new TutorialStep("Now check 'Initialize View Model' on the view.", () =>
        { 
            return view.OverrideViewModel ? null : "You haven't checked the checkbox yet";
        }), b =>
        {
            
            b.Paragraph("'Initialize View Model' will allow you to set properties on the view-model before its bound.");
            b.ImageByUrl("http://i.imgur.com/8yCZNLA.png");
            if (additionalMessage != null)
                b.Paragraph(additionalMessage);
        });


    }
    public ViewBase ScenePlayerView { get; set; }
}