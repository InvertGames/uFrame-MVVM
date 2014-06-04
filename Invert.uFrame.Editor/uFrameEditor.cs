using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Data;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

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

        private static IEnumerable<CodeGenerator> _generators;

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

            // Drawers
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
                container.RegisterInstance(Activator.CreateInstance(diagramPlugin) as IDiagramPlugin, diagramPlugin.Name,false);
            }

            foreach (var commandType in GetDerivedTypes<DiagramCommand>(false, false))
            {
                
                var command = Activator.CreateInstance(commandType) as DiagramCommand;
                if (command != null)
                {
                    container.RegisterInstance<IDiagramCommand>(command, command.Name, false);    
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

        public static IEnumerable<IDiagramCommand> GetCommandsFor<T>()
        {
            return Commands.Where(p => typeof(T).IsAssignableFrom(p.For));

        }
        public static IEnumerable<IDiagramCommand> GetContextCommandsFor<T>()
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
    }
    
    public class CodeFileGenerator
    {
        public CodeNamespace Namespace { get; set; }
        public CodeCompileUnit Unit { get; set; }

        public bool RemoveComments { get; set; }
        public string NamespaceName { get; set; }
        public CodeFileGenerator(string ns = null)
        {
            NamespaceName = ns;
        }

        public void Generate()
        {
            AddNamespaces();
        }
        public CodeGenerator[] Generators
        {
            get; set; 
        }

        public string Filename { get; set; }

        public virtual void AddNamespaces()
        {
            Namespace.Imports.Add(new CodeNamespaceImport("System"));
            Namespace.Imports.Add(new CodeNamespaceImport("System.Collections"));
            Namespace.Imports.Add(new CodeNamespaceImport("System.Linq"));
            Unit.Namespaces.Add(Namespace);
        }

        public override string ToString()
        {
            Namespace = new CodeNamespace(NamespaceName);
            Unit = new CodeCompileUnit();
            Unit.Namespaces.Add(Namespace);
            foreach (var codeGenerator in Generators)
            {
                codeGenerator.Initialize(this);
            }
            var provider = new CSharpCodeProvider();

            var sb = new StringBuilder();
            var tw1 = new IndentedTextWriter(new StringWriter(sb), "    ");
            provider.GenerateCodeFromCompileUnit(Unit, tw1, new CodeGeneratorOptions());
            tw1.Close();
            if (!RemoveComments)
            {
                var removedLines = sb.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Skip(9).ToArray();
                return string.Join(Environment.NewLine, removedLines);
            }
            return sb.ToString();
        }
    }

    public abstract class CodeGenerator
    {
        private CodeNamespace _ns;
        private CodeCompileUnit _unit;

        public virtual Type RelatedType
        {
            get; set;
        }

        public virtual string Filename
        {
            get;
            set;
        }

        public virtual void Initialize(CodeFileGenerator fileGenerator)
        {
            _unit = fileGenerator.Unit;
            _ns = fileGenerator.Namespace;
        }

        public CodeNamespace Namespace
        {
            get { return _ns; }
        }

        public CodeCompileUnit Unit
        {
            get { return _unit; }
        }
    }

    public abstract class DiagramItemGenerator
    {
        public abstract Type DiagramItemType
        {
            get;
        }

        [Inject]
        public IUFrameContainer Container { get; set; }

        [Inject]
        public IElementsDataRepository Repository { get; set; }

        public object ObjectData { get; set; }

        public abstract IEnumerable<CodeGenerator> GetGenerators(ElementDesignerData diagramData, IDiagramItem item);
    }

    public abstract class DiagramItemGenerator<TData> : DiagramItemGenerator where TData : DiagramItem
    {
        public override Type DiagramItemType
        {
            get { return typeof (TData); }
        }

        public sealed override IEnumerable<CodeGenerator> GetGenerators(ElementDesignerData diagramData, IDiagramItem item)
        {
            return CreateGenerators(diagramData, item as TData);
        }
        public abstract IEnumerable<CodeGenerator> CreateGenerators(ElementDesignerData diagramData, TData item);

    }

}
