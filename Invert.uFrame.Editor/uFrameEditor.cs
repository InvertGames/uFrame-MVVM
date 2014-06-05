using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using Invert.uFrame.Editor.ElementDesigner.Data;
using UnityEditor;
using UnityEngine;

namespace Invert.uFrame.Editor
{
    public static class uFrameEditor
    {
        private static uFrameContainer _container;

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

        private static IEditorCommand[] _commands;
        private static IToolbarCommand[] _toolbarCommands;
        private static IDiagramPlugin[] _plugins;

        private static IEnumerable<CodeGenerator> _generators;

        public static IDiagramPlugin[] Plugins
        {
            get
            {
                return _plugins ?? (_plugins = Container.ResolveAll<IDiagramPlugin>().ToArray());
            }
            set { _plugins = value; }
        }

        //public static IEditorCommand[] Commands
        //{
        //    get
        //    {
        //        return _commands ?? (_commands = Container.ResolveAll<IEditorCommand>().ToArray());
        //    }
        //}

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

        //public static IToolbarCommand[] ToolbarCommands
        //{
        //    get { return _toolbarCommands ?? (_toolbarCommands = Commands.OfType<IToolbarCommand>().ToArray()); }
        //}

        private static void InitializeContainer(uFrameContainer container)
        {
            container.Register<DiagramItemHeader, DiagramItemHeader>();

            container.RegisterInstance<IUFrameContainer>(container);
            container.RegisterInstance<uFrameContainer>(container);

            // Command Drawers
            container.RegisterInstance(new ToolbarUI());
            container.RegisterInstance(new ContextMenuUI());

            container.Register<ElementDataCommand,AddElementCollectionCommand>("Collection");
            container.Register<ElementDataCommand,AddElementCollectionCommand>("Property");
            container.Register<ElementDataCommand,AddElementCollectionCommand>("Command");
            


            // Drawers
            container.RegisterAdapter<ViewData, IElementDrawer, ViewDrawer>();
            container.RegisterAdapter<ViewComponentData, IElementDrawer, ViewComponentDrawer>();
            container.RegisterAdapter<ElementData, IElementDrawer, ElementDrawer>();
            container.RegisterAdapter<ElementDataBase, IElementDrawer, ElementDrawer>();
            container.RegisterAdapter<ImportedElementData, IElementDrawer, ElementDrawer>();
            container.RegisterAdapter<SubSystemData, IElementDrawer, SubSystemDrawer>();
            container.RegisterAdapter<SceneManagerData, IElementDrawer, SceneManagerDrawer>();
            container.RegisterAdapter<EnumData, IElementDrawer, DiagramEnumDrawer>();

            foreach (var diagramPlugin in GetDerivedTypes<DiagramPlugin>(false, false))
            {
                container.RegisterInstance(Activator.CreateInstance(diagramPlugin) as IDiagramPlugin, diagramPlugin.Name, false);
            }

            foreach (var commandType in GetDerivedTypes<EditorCommand>(false, false))
            {

                var command = Activator.CreateInstance(commandType) as EditorCommand;
                if (command != null)
                {
                    container.RegisterInstance<IEditorCommand>(command, command.Name, false);
                    container.RegisterInstance(command.GetType(), command, null, false);
                }
            }


            container.InjectAll();
            foreach (var diagramPlugin in Plugins)
            {
#if DEBUG
                UnityEngine.Debug.Log("Loaded Plugin: " + diagramPlugin);
#endif
                diagramPlugin.Initialize(Container);

            }


        }

        public static IElementDrawer CreateDrawer(IDiagramItem data, ElementsDiagram diagram)
        {
            if (data == null)
            {
                UnityEngine.Debug.Log("Data is null.");
            }
            var drawer = Container.ResolveAdapter<IElementDrawer>(data.GetType());
            if (drawer == null)
            {
                UnityEngine.Debug.Log("Couldn't Create drawer for.");
            }
            drawer.Diagram = diagram;
            drawer.Model = data;
            return drawer;
        }

        public static IEnumerable<IEditorCommand> CreateCommandsFor<T>()
        {
            var commands = Container.ResolveAll<T>();

            return Commands.Where(p => typeof(T).IsAssignableFrom(p.For));

        }
        public static IEnumerable<IEditorCommand> GetContextCommandsFor<T>()
        {
            return Commands.Where(p => p is IContextMenuItemCommand && typeof(T).IsAssignableFrom(p.For));
        }

        public static IEnumerable<CodeGenerator> GetAllCodeGenerators(ElementDesignerData diagramData)
        {
            // Grab all the code generators
            var diagramItemGenerators = Container.ResolveAll<DiagramItemGenerator>().ToArray();

            foreach (var diagramItemGenerator in diagramItemGenerators)
            {

                DiagramItemGenerator generator = diagramItemGenerator;
                var items = diagramData.AllDiagramItems.Where(p => p.GetType() == generator.DiagramItemType);

                foreach (var item in items)
                {
                    var codeGenerators = generator.GetGenerators(diagramData, item);
                    foreach (var codeGenerator in codeGenerators)
                    {
                        yield return codeGenerator;
                    }
                }
            }

        }

        public static IEnumerable<CodeFileGenerator> GetAllFileGenerators(ElementDesignerData diagramData)
        {
            var codeGenerators = GetAllCodeGenerators(diagramData).ToArray();
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

        public static void ShowCommandContextMenu()
        {
            GenericMenu menu = new GenericMenu();


        }

        public static void ExecuteCommand(IEditorCommand command, params object[] objects)
        {
            foreach (var o in objects)
            {
                if (o == null) continue;

                if (command.For.IsAssignableFrom(o.GetType()))
                {
                    command.Execute(o);
                }
            }

        }

        public static TCommandUI DoCommands<TCommandUI>(params object[] contextObjects) where TCommandUI : class, ICommandUI
        {
            return DoCommands<TCommandUI>(null, contextObjects);
        }

        public static TCommandUI DoCommands<TCommandUI>(Predicate<IEditorCommand> filter, params object[] contextObjects) where TCommandUI : class, ICommandUI
        {
            var commandUI = Container.Resolve<TCommandUI>();
            commandUI.Initialize();

            var commands = new List<IEditorCommand>();
            foreach (var contextObject in contextObjects)
            {
                if (contextObject == null) continue;

                var objCommands = Commands.Where(p => p.For.IsAssignableFrom(contextObject.GetType()) && !(p is IChildCommand));
                if (filter != null)
                {
                    objCommands = objCommands.Where(p => filter(p));
                }
                foreach (var command in objCommands)
                {
                    if (!commands.Contains(command))
                    {
                        //if (command.CanPerform(contextObject) == null)
                        //{
                        commands.Add(command);
                        //}
                    }
                }
            }

            // Loop through each command
            foreach (var editorCommand in commands)
            {
                if (editorCommand is IParentCommand)
                {

                }
                else if (editorCommand is IChildCommand)
                {

                }
                else if (editorCommand is IDynamicOptionsCommand)
                {
                    var cmd = editorCommand as IDynamicOptionsCommand;
                    var options = cmd.GetOptions(contextObjects.FirstOrDefault(p => cmd.For.IsAssignableFrom(p.GetType())));
                    //if (cmd.OptionsType == MultiOptionType.Buttons)
                    //{
                    //    foreach (var contextMenuItem in options)
                    //    {
                    //        contextMenuItem.Name
                    //    }
                    //}

                }
                else
                {
                    commandUI.DoSingleCommand(editorCommand, contextObjects);
                }
            }
            return commandUI;
        }

        public static void DoToolbar(IEnumerable<IToolbarCommand> commands, params object[] contextObjects)
        {
            foreach (var command in commands.OrderBy(p => p.Order))
            {
                var dynamicOptionsCommand = command as IDynamicOptionsCommand;
                var parentCommand = command as IParentCommand;
                if (dynamicOptionsCommand != null && dynamicOptionsCommand.OptionsType == MultiOptionType.Buttons)
                {
                    foreach (var multiCommandOption in dynamicOptionsCommand.GetOptions(Container.Resolve(command.For)))
                    {
                        if (GUILayout.Button(multiCommandOption.Name, EditorStyles.toolbarButton))
                        {
                            dynamicOptionsCommand.SelectedOption = multiCommandOption;

                            ExecuteCommand(command, contextObjects);
                        }
                    }
                }
                else if (dynamicOptionsCommand != null && dynamicOptionsCommand.OptionsType == MultiOptionType.DropDown)
                {
                    if (GUILayout.Button(command.Name, EditorStyles.toolbarButton))
                    {
                        foreach (var multiCommandOption in dynamicOptionsCommand.GetOptions(Container.Resolve(command.For)))
                        {
                            var genericMenu = new GenericMenu();
                            Invert.uFrame.Editor.ElementDesigner.ContextMenuItem option = multiCommandOption;
                            var closureSafeCommand = command;
                            genericMenu.AddItem(new GUIContent(multiCommandOption.Name), multiCommandOption.Checked,
                                () =>
                                {
                                    dynamicOptionsCommand.SelectedOption = option;
                                    ExecuteCommand(closureSafeCommand, contextObjects);
                                    closureSafeCommand.Execute(Container.Resolve(closureSafeCommand.For));
                                });
                            genericMenu.ShowAsContext();
                        }
                    }
                }
                else if (parentCommand != null)
                {
                    var contextCommands = Commands.OfType<IChildCommand>().Where(p => p.ChildCommandFor == parentCommand.GetType()).Cast<IContextMenuItemCommand>();
                    //DoContextOptions(contextCommands);

                }
                else
                {
                    if (GUILayout.Button(command.Name, EditorStyles.toolbarButton))
                    {
                        ExecuteCommand(command, contextObjects);
                    }
                }
            }
        }

        //private static void DoContextOptions(IEnumerable<IContextMenuItemCommand> contextCommands)
        //{
        //    GenericMenu menu = new GenericMenu();
        //    foreach (var contextMenuItemCommand in contextCommands)
        //    {
        //        contextMenuItemCommand.Path = 
        //    }
        //}
    }
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandName : Attribute
    {
        public string Name { get; set; }

        public CommandName(string name)
        {
            Name = name;
        }
    }
    public interface ICommandUI
    {
        void Initialize();
        void DoSingleCommand(IEditorCommand command, object[] contextObjects);
        void DoMultiCommand(IEditorCommand parentCommand, IEditorCommand[] childCommand, object[] contextObjects);
    }

    public class ToolbarUI : ICommandUI
    {

        public void Initialize()
        {

        }
        public void DoSingleCommand(IEditorCommand command, object[] contextObjects)
        {
            if (GUILayout.Button(command.Title, EditorStyles.toolbarButton))
            {
                uFrameEditor.ExecuteCommand(command, contextObjects);
            }
        }

        public void DoMultiCommand(IEditorCommand parentCommand, IEditorCommand[] childCommand, object[] contextObjects)
        {
            throw new NotImplementedException();
        }
    }

    public class ContextMenuUI : ICommandUI
    {
        private GenericMenu _contextMenu;

        public GenericMenu ContextMenu
        {
            get { return _contextMenu ?? (_contextMenu = new GenericMenu()); }
            set { _contextMenu = value; }
        }

        public void Initialize()
        {
            _contextMenu = new GenericMenu();
        }
        public void DoSingleCommand(IEditorCommand command, object[] contextObjects)
        {

        }

        public void DoMultiCommand(IEditorCommand parentCommand, IEditorCommand[] childCommand, object[] contextObjects)
        {

        }
    }
}
