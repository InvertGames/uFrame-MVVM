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

        builder.ShowTutorialStep(new TutorialStep("Now we need to scaffold/update the kernel.", () =>
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
            b.Paragraph("Saving and compiling is the process of taking the information you've created by graphs and putting it into code form.  The generated code consists of both designer files (always regenerated), and editable files (only generated if it doesnt exist so it doesn't destroy your implementations).");
            b.Break();
            b.ImageByUrl("http://i.imgur.com/HY5sD9D.png");
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

    protected void EnsureNamespace(IDocumentationBuilder _)
    {
        _.ShowTutorialStep(new TutorialStep("Set the project namespace to something unique", () =>
        {
            if (string.IsNullOrEmpty(TheProject.Namespace))
            {
                return
                    "The current namespace is not set yet.  Navigate to the project repository, and set the namespace property.";

            }
            return null;
        }), b =>
        {
            b.ImageByUrl("http://i.imgur.com/CBXSJbZ.png");
        });
   
    }
    protected bool BasicSetup(IDocumentationBuilder _, string systemName = "SystemA", string sceneName = "SystemB")
    {
        TheProject = DoCreateNewProjectStep(_);
        if (TheProject == null) return false;

        TheGraph = DoGraphStep<MVVMGraph>(_);
        if (TheGraph == null) return false;

        EnsureNamespace(_);

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
        DoCreateConnectionStep(_, TheGame, GameView == null ? null : GameView.ElementInputSlot);
    }
    protected bool CreatePlayerElement(IDocumentationBuilder _)
    {
        ThePlayer = DoNamedNodeStep<ElementNode>(_, "Player", SystemA, b =>
        {
            b.ImageByUrl("http://i.imgur.com/lEnVVQj.png");
            b.ImageByUrl("http://i.imgur.com/SJ0zI8w.png");
        });
        if (TheGame == null) return false;
        return true;
    }

    public ElementNode ThePlayer { get; set; }

    protected void CreatePlayerView(IDocumentationBuilder _)
    {
        ThePlayerView = DoNamedNodeStep<ViewNode>(_, "PlayerView", ThePlayer, b =>
        {
            b.ImageByUrl("http://i.imgur.com/H1OzzfC.png");
        });
        DoCreateConnectionStep(_, ThePlayer, ThePlayerView == null ? null : ThePlayerView.ElementInputSlot, b =>
        {
            
            b.Paragraph("Creating this connection means that the view will visually present the data on the player to the user in the 3d world.");
            b.Paragraph("You can always create multiple views to seperate different presentations of the same data.  For instance, you could also create a PlayerUIView, which deals strictly with showing inventory, stats,...etc");

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
                b.Paragraph("Create an empty gameObject underneath the _SceneARoot game object.  " +
                            "When creating scene types, everything should be a descendent of this root game object, " +
                            "this allows them to be destroyed by uFrame when needed.");

                b.Paragraph(
                    string.Format("On this empty game object click 'Add Component' in the inspector. Then add the '{0}' component to it.", view == null ? "view" : view.Name));

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