using Invert.uFrame.Code.Bindings;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Invert.uFrame.Editor
{
    public interface IKeyBinding
    {
        bool RequireShift { get; set; }
        bool RequireAlt { get; set; }
        bool RequireControl { get; set; }
        KeyCode Key { get; set; }
        string Name { get; set; }
        Type CommandType { get; }
    }

    public class KeyBinding<TCommandType> : IKeyBinding where TCommandType : class
    {
        public bool RequireShift { get; set; }
        public bool RequireAlt { get; set; }
        public bool RequireControl { get; set; }
        public KeyCode Key { get; set; }

        public string Name { get; set; }

        public KeyBinding(KeyCode key, bool requireControl = false, bool requireAlt = false, bool requireShift = false)
            : this(null, key, requireControl, requireAlt, requireShift)
        {

        }

        public KeyBinding(string name, KeyCode key, bool requireControl = false, bool requireAlt = false, bool requireShift = false)
        {
            Name = name;
            Key = key;
            RequireControl = requireControl;
            RequireAlt = requireAlt;
            RequireShift = requireShift;
        }

        public Type CommandType
        {
            get { return typeof(TCommandType); }
        }

        public TCommandType Command
        {
            get
            {
                return uFrameEditor.Container.Resolve<TCommandType>(Name);
            }
        }
    }

    public interface IUFrameTypeProvider
    {
        Type ViewModel { get; }
        Type Controller { get; }
        Type SceneManager { get; }
        Type GameManager { get; }
        Type ViewComponent { get; }
        Type ViewBase { get; }

        Type UFToggleGroup { get; }
        Type UFGroup { get; }
        Type UFRequireInstanceMethod { get; }
        
        Type DiagramInfoAttribute { get; }

        Type UpdateProgressDelegate { get; }

        Type CommandWithSenderT { get; }
        Type CommandWith { get; }
        Type CommandWithSenderAndArgument { get; }
        Type YieldCommandWithSenderT { get; }
        Type YieldCommandWith { get; }
        Type YieldCommandWithSenderAndArgument { get; }
        Type YieldCommand { get; }
        Type Command { get; }
        Type ICommand { get; }
        Type ListOfViewModel { get; }

        Type ISerializerStream { get; }

        Type P { get; }
        Type ModelCollection { get; }

    }
    public static class uFrameEditor
    {
        public static IUFrameTypeProvider uFrameTypes { get; set; }

        public static Type FindType(string name)
        {
            //if (string.IsNullOrEmpty(name)) return null;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var t = assembly.GetType(name);
                if (t != null)
                {
                    return t;
                }
            }
            return null;
        }
        public static IKeyBinding[] KeyBindings { get; set; }
        private static IEditorCommand[] _commands;
        private static uFrameContainer _container;

        private static IEnumerable<CodeGenerator> _generators;

        private static IDiagramPlugin[] _plugins;

        private static IToolbarCommand[] _toolbarCommands;

        public static IEditorCommand[] Commands
        {
            get
            {
                return _commands ?? (_commands = Container.ResolveAll<IEditorCommand>().ToArray());
            }
        }

        public static uFrameContainer Container
        {
            get
            {
                if (_container != null) return _container;
                _container = new uFrameContainer();
                InitializeContainer(_container);
                return _container;
            }
            set { _container = value; }
        }

        public static IDiagramPlugin[] Plugins
        {
            get
            {
                return _plugins ?? (_plugins = Container.ResolveAll<IDiagramPlugin>().ToArray());
            }
            set { _plugins = value; }
        }

        private static IBindingGenerator[] BindingGenerators { get; set; }

        public static IEnumerable<IBindingGenerator> GetBindingGeneratorsFor(ElementData element, bool isOverride = true, bool generateDefaultBindings = true,bool includeBaseItems = true)
        {
            IEnumerable<IViewModelItem> items = element.ViewModelItems;
            if (includeBaseItems)
            {
                items = new[] {element}.Concat(element.AllBaseTypes).SelectMany(p => p.ViewModelItems);
            }
            //var vmItems = new[] {element}.Concat(element.AllBaseTypes).SelectMany(p => p.ViewModelItems);
            foreach (var viewModelItem in items)
            {
                var bindingGenerators = Container.ResolveAll<IBindingGenerator>();
                foreach (var bindingGenerator in bindingGenerators)
                {
                    bindingGenerator.IsOverride = isOverride;
                    bindingGenerator.Item = viewModelItem;
                    bindingGenerator.GenerateDefaultImplementation = generateDefaultBindings;
                    if (bindingGenerator.IsApplicable)
                        yield return bindingGenerator;
                }
            }
        }
        public static IEnumerable<IEditorCommand> CreateCommandsFor<T>()
        {
            var commands = Container.ResolveAll<T>();

            return Commands.Where(p => typeof(T).IsAssignableFrom(p.For));
        }

        public static TCommandUI CreateCommandUI<TCommandUI>(ICommandHandler handler, params Type[] contextTypes) where TCommandUI : class,ICommandUI
        {
            var ui = Container.Resolve<TCommandUI>() as ICommandUI;
            ui.Handler = handler;
            foreach (var contextType in contextTypes)
            {
                var commands = Container.ResolveAll(contextType).Cast<IEditorCommand>().ToArray();

                foreach (var command in commands)
                {
                    ui.AddCommand(command);
                }
            }
            return (TCommandUI)ui;
        }

        public static INodeDrawer CreateDrawer(IDiagramNode data, ElementsDiagram diagram)
        {
            if (data == null)
            {
                Debug.Log("Data is null.");
            }
            var drawer = Container.ResolveRelation<INodeDrawer>(data.GetType());
            if (drawer == null)
            {
                Debug.Log("Couldn't Create drawer for.");
            }
            drawer.Diagram = diagram;
            drawer.Model = data;
            return drawer;
        }

        public static void ExecuteCommand(this ICommandHandler handler, IEditorCommand command)
        {
            var objs = handler.ContextObjects.ToArray();
            foreach (var o in objs)
            {
                if (o == null) continue;

                if (command.For.IsAssignableFrom(o.GetType()))
                {
                    if (command.CanPerform(o) != null) continue;
                    handler.CommandExecuting(command);
                    command.Execute(o);
                    if (command.Hooks != null)
                        command.Hooks.ForEach(p => ExecuteCommand(handler, p));
                    handler.CommandExecuted(command);
                }
            }
        }


        public static IEnumerable<CodeGenerator> GetAllCodeGenerators(ICodePathStrategy pathStrategy, IElementDesignerData diagramData)
        {
            // Grab all the code generators
            var diagramItemGenerators = Container.ResolveAll<DesignerGeneratorFactory>().ToArray();

            foreach (var diagramItemGenerator in diagramItemGenerators)
            {
                DesignerGeneratorFactory generator = diagramItemGenerator;
                // If its a generator for the entire diagram
                if (typeof(IElementDesignerData).IsAssignableFrom(generator.DiagramItemType))
                {
                    var codeGenerators = generator.GetGenerators(pathStrategy, diagramData, diagramData);
                    foreach (var codeGenerator in codeGenerators)
                    {
                        codeGenerator.ObjectData = diagramData;
                        codeGenerator.GeneratorFor = diagramItemGenerator.DiagramItemType;
                        yield return codeGenerator;
                    }

                }
                // If its a generator for a specific node type
                else
                {
                    var items = diagramData.AllDiagramItems.Where(p => p.GetType() == generator.DiagramItemType);

                    foreach (var item in items)
                    {
                        var codeGenerators = generator.GetGenerators(pathStrategy, diagramData, item);
                        foreach (var codeGenerator in codeGenerators)
                        {
                            codeGenerator.ObjectData = item;
                            codeGenerator.GeneratorFor = diagramItemGenerator.DiagramItemType;
                            yield return codeGenerator;
                        }
                    }
                }

            }
        }

        public static IEnumerable<CodeFileGenerator> GetAllFileGenerators(IElementDesignerData diagramData, ICodePathStrategy strategy = null)
        {
            var codeGenerators = GetAllCodeGenerators(strategy ?? diagramData.Settings.CodePathStrategy, diagramData).ToArray();
            var groups = codeGenerators.GroupBy(p => p.Filename);
            foreach (var @group in groups)
            {
                var generator = new CodeFileGenerator()
                {
                    Filename = @group.Key,
                    Generators = @group.ToArray()
                };
                yield return generator;
            }
        }

        public static IEnumerable<IEditorCommand> GetContextCommandsFor<T>()
        {
            return Commands.Where(p => p is IContextMenuItemCommand && typeof(T).IsAssignableFrom(p.For));
        }

        public static IEnumerable<Type> GetDerivedTypes<T>(bool includeAbstract = false, bool includeBase = true)
        {
            var type = typeof(T);
            if (includeBase)
                yield return type;
            if (includeAbstract)
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in assembly
                        .GetTypes()
                        .Where(x => x.IsSubclassOf(type)))
                    {
                        yield return t;
                    }
                }
            }
            else
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var t in assembly
                        .GetTypes()
                        .Where(x => x.IsSubclassOf(type) && !x.IsAbstract))
                    {
                        yield return t;
                    }
                }
            }
        }

        private static void InitializeContainer(uFrameContainer container)
        {
            container.Register<NodeItemHeader, NodeItemHeader>();

            container.RegisterInstance<IUFrameContainer>(container);
            container.RegisterInstance<uFrameContainer>(container);

            // Register the diagram type
            container.Register<ElementsDiagram, ElementsDiagram>();

            // Repositories
            container.RegisterInstance<IElementsDataRepository>(new DefaultElementsRepository(), ".asset");

            container.RegisterInstance<IElementsDataRepository>(new JsonRepository(), ".json");

            // Command Drawers
            container.Register<ToolbarUI, ToolbarUI>();
            container.Register<ContextMenuUI, ContextMenuUI>();


            container.RegisterInstance(new AddElementCommandCommand());
            container.RegisterInstance(new AddElementCollectionCommand());
            container.RegisterInstance(new AddElementPropertyCommand());

            container.RegisterInstance(new AddEnumItemCommand());

            container.RegisterInstance(new AddViewPropertyCommand());
            container.RegisterInstance(new AddBindingCommand());

            container.RegisterInstance<IEditorCommand>(new RemoveNodeItemCommand(), "RemoveNodeItem");

            // Toolbar commands
            container.RegisterInstance<IToolbarCommand>(new PopToFilterCommand(), "PopToFilterCommand");
            container.RegisterInstance<IToolbarCommand>(new SaveCommand(), "SaveCommand");
            container.RegisterInstance<IToolbarCommand>(new AutoLayoutCommand(), "AutoLayoutCommand");
            container.RegisterInstance<IToolbarCommand>(new AddNewCommand(), "AddNewCommand");
            container.RegisterInstance<IToolbarCommand>(new DiagramSettingsCommand() { Title = "Settings" }, "SettingsCommand");
#if DEBUG
            container.RegisterInstance<IToolbarCommand>(new ConvertToJSON(), "SaveAsJson");
            container.RegisterInstance<IToolbarCommand>(new PrintPlugins(), "PrintJson");

#endif
            // For the add new menu
            container.RegisterInstance<AddNewCommand>(new AddNewSceneManagerCommand(), "AddNewSceneManagerCommand");
            container.RegisterInstance<AddNewCommand>(new AddNewSubSystemCommand(), "AddNewSubSystemCommand");
            container.RegisterInstance<AddNewCommand>(new AddNewElementCommand(), "AddNewElementCommand");
            container.RegisterInstance<AddNewCommand>(new AddNewEnumCommand(), "AddNewEnumCommand");
            container.RegisterInstance<AddNewCommand>(new AddNewViewCommand(), "AddNewViewCommand");
            container.RegisterInstance<AddNewCommand>(new AddNewViewComponentCommand(), "AddNewViewComponentCommand");


            // For no selection diagram context menu
            container.RegisterInstance<IDiagramContextCommand>(new AddNewSceneManagerCommand(), "AddNewSceneManagerCommand");
            container.RegisterInstance<IDiagramContextCommand>(new AddNewSubSystemCommand(), "AddNewSubSystemCommand");
            container.RegisterInstance<IDiagramContextCommand>(new AddNewElementCommand(), "AddNewElementCommand");
            container.RegisterInstance<IDiagramContextCommand>(new AddNewEnumCommand(), "AddNewEnumCommand");
            container.RegisterInstance<IDiagramContextCommand>(new AddNewViewCommand(), "AddNewViewCommand");
            container.RegisterInstance<IDiagramContextCommand>(new AddNewViewComponentCommand(), "AddNewViewComponentCommand");
            container.RegisterInstance<IDiagramContextCommand>(new ShowItemCommand(), "ShowItem");

            // For node context menu
            container.RegisterInstance<IDiagramNodeCommand>(new OpenCommand(), "OpenCode");
            container.RegisterInstance<IDiagramNodeCommand>(new DeleteCommand(), "Delete");
            container.RegisterInstance<IDiagramNodeCommand>(new RenameCommand(), "Reanme");
            container.RegisterInstance<IDiagramNodeCommand>(new HideCommand(), "Hide");
            container.RegisterInstance<IDiagramNodeCommand>(new RemoveLinkCommand(), "RemoveLink");
            container.RegisterInstance<IDiagramNodeCommand>(new SelectViewBaseElement(), "SelectView");
            container.RegisterInstance<IDiagramNodeCommand>(new MarkIsTemplateCommand(), "MarkAsTemplate");
            container.RegisterInstance<IDiagramNodeCommand>(new MarkIsMultiInstanceCommand(), "MarkAsMulti");

            // For node item context menu
            container.RegisterInstance<IDiagramNodeItemCommand>(new MarkIsYieldCommand(), "MarkIsYield");
            container.RegisterInstance<IDiagramNodeItemCommand>(new DeleteItemCommand(), "Delete");

            container.RegisterInstance<IDiagramNodeItemCommand>(new MoveUpCommand(), "MoveItemUp");
            container.RegisterInstance<IDiagramNodeItemCommand>(new MoveDownCommand(), "MoveItemDown");

            // Drawers
            container.RegisterRelation<ViewData, INodeDrawer, ViewDrawer>();
            container.RegisterRelation<ViewComponentData, INodeDrawer, ViewComponentDrawer>();
            container.RegisterRelation<ElementData, INodeDrawer, ElementDrawer>();
            container.RegisterRelation<ElementDataBase, INodeDrawer, ElementDrawer>();
            //container.RegisterRelation<ImportedElementData, INodeDrawer, ElementDrawer>();
            container.RegisterRelation<SubSystemData, INodeDrawer, SubSystemDrawer>();
            container.RegisterRelation<SceneManagerData, INodeDrawer, SceneManagerDrawer>();
            container.RegisterRelation<EnumData, INodeDrawer, DiagramEnumDrawer>();



#if DEBUG
            //External Nodes
            //container.RegisterInstance<IDiagramContextCommand>(new ShowExternalItemCommand(), "AddNewExternalItemCommand");
            //container.RegisterRelation<ExternalSubsystem, INodeDrawer, ExternalNodeDrawer>();
#endif
            foreach (var diagramPlugin in GetDerivedTypes<DiagramPlugin>(false, false))
            {
                container.RegisterInstance(Activator.CreateInstance((Type)diagramPlugin) as IDiagramPlugin, diagramPlugin.Name, false);
            }

            container.InjectAll();
            foreach (var diagramPlugin in Plugins.OrderBy(p => p.LoadPriority))
            {
                //#if DEBUG
                //                Debug.Log("Loaded Plugin: " + diagramPlugin);
                //#endif
                if (diagramPlugin.Enabled)
                    diagramPlugin.Initialize(Container);
            }
            KeyBindings = Container.ResolveAll<IKeyBinding>().ToArray();
            BindingGenerators = Container.ResolveAll<IBindingGenerator>().ToArray();
            uFrameTypes = Container.Resolve<IUFrameTypeProvider>();

#if DEBUG
            Debug.Log(uFrameTypes.ToString());
#endif
            uFrameTypes = new uFrameStringTypeProvider();
        }


        public static void HookCommand<TFor>(string name, IEditorCommand hook) where TFor : class, IEditorCommand
        {
            var command = Container.Resolve<TFor>(name);
            command.Hooks.Add(hook);
        }
    }
}