using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Data;

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

        private static IDiagramCommand[] _commands;
        private static ToolbarCommand[] _toolbarCommands;
        private static IDiagramPlugin[] _plugins;

        public static IDiagramPlugin[] Plugins
        {
            get
            {
                return _plugins ?? (_plugins = Container.ResolveAll<IDiagramPlugin>().ToArray());
            }
            set { _plugins = value; }
        }

        public static IDiagramCommand[] Commands
        {
            get
            {
                return _commands ?? (_commands = Container.ResolveAll<IDiagramCommand>().ToArray());
            }
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

        public static ToolbarCommand[] ToolbarCommands
        {
            get { return _toolbarCommands ?? (_toolbarCommands = Commands.OfType<ToolbarCommand>().ToArray()); }
        }

        private static void InitializeContainer(uFrameContainer container)
        {
            container.Register<DiagramItemHeader,DiagramItemHeader>();

            container.RegisterInstance<IUFrameContainer>(container);
            container.RegisterInstance<uFrameContainer>(container);

            container.RegisterAdapter<ViewData,IElementDrawer,ViewDrawer>();
            container.RegisterAdapter<ViewComponentData, IElementDrawer, ViewComponentDrawer>();
            container.RegisterAdapter<ElementData, IElementDrawer, ElementDrawer>();
            container.RegisterAdapter<ElementDataBase, IElementDrawer, ElementDrawer>();
            container.RegisterAdapter<ImportedElementData, IElementDrawer, ElementDrawer>();
            container.RegisterAdapter<SubSystemData, IElementDrawer, SubSystemDrawer>();
            container.RegisterAdapter<SceneManagerData, IElementDrawer, SceneManagerDrawer>();
            container.RegisterAdapter<EnumData, IElementDrawer, DiagramEnumDrawer>();

            foreach (var diagramPlugin in GetDerivedTypes<DiagramPlugin>(false, false))
            {
                container.RegisterInstance(diagramPlugin.Name, Activator.CreateInstance(diagramPlugin) as DiagramPlugin,false);
            }

            foreach (var commandType in GetDerivedTypes<DiagramCommand>(false, false))
            {
                container.RegisterInstance(commandType.Name, Activator.CreateInstance(commandType) as DiagramCommand, false);
            }
            container.InjectAll();

            foreach (var diagramPlugin in Plugins)
            {
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

        public static IEnumerable<IDiagramCommand> GetCommandsFor<T>()
        {
            return Commands.Where(p => typeof(T).IsAssignableFrom(p.For));

        }
        public static IEnumerable<IDiagramCommand> GetContextCommandsFor<T>()
        {
            return Commands.Where(p => p is IContextMenuItemCommand && typeof(T).IsAssignableFrom(p.For));
        }
    }
}
