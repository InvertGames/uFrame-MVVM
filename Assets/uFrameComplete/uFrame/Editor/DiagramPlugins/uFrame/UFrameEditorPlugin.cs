using Invert.uFrame;
using Invert.uFrame.Code.Bindings;
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

    public override bool Enabled
    {
        get { return true; }
        set
        {
            
        }
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
        container.Register<ICodePathStrategy,SubSystemPathStrategy>("By Subsystem");
        // The code generators
        container.Register<DesignerGeneratorFactory, ElementDataGeneratorFactory>("ElementData");
        container.Register<DesignerGeneratorFactory, EnumDataGeneratorFactory>("EnumData");
        container.Register<DesignerGeneratorFactory, ViewDataGeneratorFactory>("ViewData");
        container.Register<DesignerGeneratorFactory, ViewComponentDataGeneratorFactory>("ViewComponentData");
        container.Register<DesignerGeneratorFactory, SceneManagerDataGeneratorFactory>("SceneManagerData");

        container.Register<IBindingGenerator,PropertyBindingGenerator>("PropertyBinding");
        container.Register<IBindingGenerator,CollectionItemAddedBindingGenerator>("Added");
        container.Register<IBindingGenerator, CollectionItemRemovedBindingGenerator>("Removed");
        container.Register<IBindingGenerator, CollectionItemCreateBindingGenerator>("Create");

        // Import is no longer needed
        //container.RegisterInstance<IToolbarCommand>(new ImportCommand(),"Import");
    }


}


