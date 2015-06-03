using System;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
using uFrame.MVVM.Templates;

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
        _.Paragraph("Scene Types exist on the root game object of a scene. These components need to live on the root game-object of the scene. This allows uFrame to know what scene has been loaded and to keep a reference for removing this scene when needed.");
 
        
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


        _.AlsoSeePages(typeof(UsingSceneLoaders));


    }
}