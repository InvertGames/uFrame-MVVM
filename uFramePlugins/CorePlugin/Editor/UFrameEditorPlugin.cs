using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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

        container.Register<IBindingGenerator, PropertyBindingGenerator>("PropertyBinding");
        container.Register<IBindingGenerator, CollectionItemAddedBindingGenerator>("Added");
        container.Register<IBindingGenerator, CollectionItemRemovedBindingGenerator>("Removed");
        container.Register<IBindingGenerator, CollectionItemCreateBindingGenerator>("Create");



        //container.RegisterInstance<IBindingGenerator>(new PropertyBindingGenerator(){},"PropertyBinding");
        //container.RegisterInstance<IBindingGenerator>(new CollectionItemAddedBindingGenerator() { IsViewModelBinding = true }, "AddedVMBinding");
        //container.RegisterInstance<IBindingGenerator>(new CollectionItemRemovedBindingGenerator() { IsViewModelBinding = true }, "RemovedVMBinding");
        //container.RegisterInstance<IBindingGenerator>(new CollectionItemCreateBindingGenerator() { IsViewModelBinding = true }, "CreateVMBinding");

        //container.RegisterInstance<IBindingGenerator>(new CollectionItemAddedBindingGenerator() { IsViewModelBinding = false }, "AddedBinding");
        //container.RegisterInstance<IBindingGenerator>(new CollectionItemRemovedBindingGenerator() { IsViewModelBinding = false }, "RemovedBinding");



        container.RegisterInstance<IUFrameTypeProvider>(new uFrameTypeProvider());

        // Import is no longer needed
        //container.RegisterInstance<IToolbarCommand>(new ImportCommand(),"Import");
    }

    public class uFrameTypeProvider : IUFrameTypeProvider
    {
        public Type ViewModel
        {
            get { return typeof (ViewModel); }
        }

        public Type Controller
        {
            get { return typeof (Controller); }
        }

        public Type SceneManager
        {
            get { return typeof (SceneManager); }
        }

        public Type GameManager
        {
            get { return typeof (GameManager); }
        }

        public Type ViewComponent
        {
            get { return typeof (ViewComponent); }
        }

        public Type ViewBase
        {
            get { return typeof (ViewBase); }
        }

        public Type UFToggleGroup
        {
            get { return typeof (UFToggleGroup); }
        }

        public Type UFGroup
        {
            get { return typeof (UFGroup); }
        }

        public Type UFRequireInstanceMethod
        {
            get { return typeof (UFRequireInstanceMethod); }
        }

        public Type DiagramInfoAttribute
        {
            get { return typeof (DiagramInfoAttribute); }
        }

        public Type GetModelCollectionType<T>()
        {
            return typeof (ModelCollection<T>);
        }

        public Type UpdateProgressDelegate
        {
            get { return typeof(UpdateProgressDelegate); }
        }

        public Type CommandWithSenderT
        {
            get { return typeof (CommandWithSender<>); }
        }

        public Type CommandWith
        {
            get { return typeof (CommandWith<>); }
        }

        public Type CommandWithSenderAndArgument
        {
            get { return typeof (CommandWithSenderAndArgument<,>); }
        }

        public Type YieldCommandWithSenderT
        {
            get { return typeof (YieldCommandWithSender<>); }
        }

        public Type YieldCommandWith
        {
            get { return typeof (YieldCommandWith<>); }
        }

        public Type YieldCommandWithSenderAndArgument
        {
            get { return typeof (YieldCommandWithSenderAndArgument<,>); }
        }

        public Type YieldCommand
        {
            get { return typeof (YieldCommand); }
        }

        public Type Command
        {
            get { return typeof (Command); }
        }

        public Type ICommand
        {
            get { return typeof (ICommand); }
        }

        public Type ListOfViewModel
        {
            get {  return typeof (List<ViewModel>); }
        }

        public Type ISerializerStream
        {
            get { return typeof (ISerializerStream); }
        }

        public Type P
        {
            get { return typeof (P<>); }
        }

        public Type ModelCollection
        {
            get { return typeof (ModelCollection<>); }
        }

        public Type Computed
        {
            get { return typeof (Computed<>); }
        }

        public override string ToString()
        {
            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var sb = new StringBuilder();
            sb.AppendFormat("public class uFrameStringTypeProvider : IUFrameTypeProvider {{");
                sb.AppendLine();
            foreach (var property in properties)
            {
                if (property.PropertyType != typeof(Type))
                continue;
                var type = property.GetValue(this, null) as Type;
                if (type == null) continue;
                sb.AppendFormat("\tprivate Type _{0};", property.Name);
                sb.AppendLine();
                sb.AppendFormat("\tpublic Type {0} {{ get {{ return _{0} ?? (_{0} = uFrameEditor.FindType(\"{1}\")); }} }}", property.Name,
                    type.FullName);
                sb.AppendLine();
            }
            sb.Append("}");
            sb.AppendLine();
            return sb.ToString();
        }
    }

}


public class MyCustomPlugin : DiagramPlugin
{
    public override decimal LoadPriority
    {
        get { return -1; }
    }

    public override bool Enabled
    {
        get { return true; }
        set { }
    }

    public override void Initialize(uFrameContainer container)
    {
        container.RegisterInstance<IEditorCommand>(
            new SelectItemTypeCommand() {AllowNone = false, PrimitiveOnly = false}, "ViewModelPropertyTypeSelection");
        container.RegisterInstance<IEditorCommand>(
            new SelectItemTypeCommand() {AllowNone = true, PrimitiveOnly = false}, "ViewModelCommandTypeSelection");
        container.RegisterInstance<IEditorCommand>(
            new SelectItemTypeCommand() {AllowNone = false, PrimitiveOnly = false}, "ViewModelCollectionTypeSelection");
    }
}

public class ElementAssetsPlugin
{

}