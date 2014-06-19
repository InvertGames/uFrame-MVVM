using Invert.uFrame;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using UnityEngine;

public class UFrameEditorPlugin : DiagramPlugin
{
    public override decimal LoadPriority
    {
        get { return -1; }
    }


    public override void Initialize(uFrameContainer container)
    {
#if DEBUG
        Debug.Log("Registering " + "UFrameEditorPlugin");
#endif

        container.RegisterInstance<IEditorCommand>(new FindInSceneCommand(), "ViewDoubleClick");
        container.RegisterInstance<IEditorCommand>(new SelectItemTypeCommand() { AllowNone = false, PrimitiveOnly = false }, "ViewModelPropertyTypeSelection");
        container.RegisterInstance<IEditorCommand>(new SelectItemTypeCommand() { AllowNone = true, PrimitiveOnly = false }, "ViewModelCommandTypeSelection");
        container.RegisterInstance<IEditorCommand>(new SelectItemTypeCommand() {AllowNone = false,PrimitiveOnly = false}, "ViewModelCollectionTypeSelection");
        container.RegisterInstance<IDiagramNodeCommand>(new CreateSceneCommand(), "CreateScene");
        container.RegisterInstance<IDiagramNodeCommand>(new AddManagerToSceneCommand(), "AddToScene");
        container.RegisterInstance<IDiagramNodeCommand>(new AddManagerToSceneSelectionCommand(), "AddToSceneSelection");
        container.RegisterInstance<IDiagramNodeCommand>(new AddViewToSceneCommand(), "AddViewToScene");
        container.RegisterInstance<IDiagramNodeCommand>(new AddViewToSceneSelectionCommand(), "AddViewToSceneSelection");

        // Where the generated code files are placed
        container.Register<ICodePathStrategy,DefaultCodePathStrategy>("Default");
        // The code generators
        container.Register<NodeItemGenerator,ElementDataGenerator>("ElementData");
        container.Register<NodeItemGenerator,EnumDataGenerator>("EnumData");
        container.Register<NodeItemGenerator,ViewDataGenerator>("ViewData");
        container.Register<NodeItemGenerator,ViewComponentDataGenerator>("ViewComponentData");
        container.Register<NodeItemGenerator,SceneManagerDataGenerator>("SceneManagerData");

        container.RegisterInstance<IToolbarCommand>(new ImportCommand(),"Import");
    }
}


