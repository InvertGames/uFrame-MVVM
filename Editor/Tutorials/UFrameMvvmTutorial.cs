using System;
using System.IO;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using UnityEditor;
using UnityEngine;

public abstract class UFrameMvvmTutorial : uFrameMVVMPage<InteractiveTutorials>
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
        Kernel = this.EnsureScaffoldKernel(_, TheProject, ExplainKernel);
        if (Kernel == null) return false;
        return true;
    }

    public uFrameMVVMKernel Kernel { get; set; }

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

    protected void GameBasicSetup(IDocumentationBuilder _)
    {
        BasicSetup(_);
        CreateGameElement(_);
        CreateGameView(_);
    }
}