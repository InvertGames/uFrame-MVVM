using System;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using UnityEditor;
using UnityEngine;

public class TheBasics : uFrameMVVMPage<GettingStartedPage>
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
        var project = DoCreateNewProjectStep(_);
        if (project == null) return;

        var graph = DoGraphStep<MVVMGraph>(_);
        if (graph == null) return ;

        var systemA = DoNamedNodeStep<SubsystemNode>(_, "SystemA");
        if (systemA == null) return ;

        var systemB = DoNamedNodeStep<SubsystemNode>(_, "SystemB");
        if (systemB == null) return ;

        var sceneA = DoNamedNodeStep<SceneTypeNode>(_, "SceneA");
        if (sceneA == null) return ;

        var sceneB = DoNamedNodeStep<SceneTypeNode>(_, "SceneB");
        if (sceneB == null) return ;


        _.ShowTutorialStep(SaveAndCompile(sceneA));

        var kernel = EnsureScaffoldKernel(_, project, ExplainKernel);
        if (kernel == null) return;
        
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
        _.ContentByPage<TheKernel>();
    }
}

public class TheKernel : uFrameMVVMPage<uFrameInDepth>
{
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
