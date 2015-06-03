using System.CodeDom;
using System.Collections.Generic;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.Core.GraphDesigner.Unity.Refactoring;
using Invert.IOC;
using Invert.StateMachine;
using Invert.uFrame.MVVM;
using uFrame.MVVM.Bindings;
using uFrame.MVVM.Templates;
using UnityEngine;

namespace uFrame.MVVM.Templates
{
    public class uFrameTemplates : DiagramPlugin
    {
        // Make sure this plugin loads after the framework plugin loads
        public override decimal LoadPriority
        {
            get { return 5; }
        }

        private uFrameMVVM Framework { get; set; }

        public override string Title
        {
            get { return "uFrame MVVM Templates"; }
        }

        public override void Initialize(UFrameContainer container)
        {
            // Grab a reference to the main framework graphs plugin
            Framework = container.Resolve<uFrameMVVM>();
            //Framework.ElementsGraphRoot.AddCodeTemplate<BackupData>();
            // Register the code template
            Framework.Service.AddCodeTemplate<ServiceTemplate>();
            Framework.SimpleClass.AddCodeTemplate<SimpleClassTemplate>();
            Framework.Element.AddCodeTemplate<ControllerTemplate>();
            Framework.Element.AddCodeTemplate<ViewModelTemplate>();
            Framework.Element.AddCodeTemplate<ViewModelConstructorTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<CommandsChildItem, ViewModelCommandClassTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<CommandNode, CommandClassTemplate>();
            //Framework.SceneManager.AddCodeTemplate<SceneManagerTemplate>();
            //Framework.SceneManager.AddCodeTemplate<SceneManagerSettingsTemplate>();
            Framework.SceneType.AddCodeTemplate<SceneTemplate>();
            Framework.SceneType.AddCodeTemplate<SceneSettingsTemplate>();
            Framework.Subsystem.AddCodeTemplate<SystemLoaderTemplate>();
            Framework.SceneType.AddCodeTemplate<SceneLoaderTemplate>();
            Framework.View.AddCodeTemplate<ViewTemplate>();
            Framework.ViewComponent.AddCodeTemplate<ViewComponentTemplate>();
            Framework.State.AddCodeTemplate<StateTemplate>();
            Framework.StateMachine.AddCodeTemplate<StateMachineTemplate>();

            // Register our bindable methods
            container.AddBindingMethod(typeof (ViewBindings), "BindProperty",
                _ => _ is PropertiesChildItem || _ is ComputedPropertyNode)
                .SetNameFormat("{0} Changed")
                .ImplementWith(args =>
                {
                    var sourceItem = args.SourceItem as ITypedItem;

                    if (sourceItem.RelatedNode() is StateMachineNode)
                    {
                        args.Method.Parameters.Clear();
                        args.Method.Parameters.Add(new CodeParameterDeclarationExpression(typeof (State), "State"));
                    }
                })
                ;

            container.AddBindingMethod(typeof (ViewBindings), "BindStateProperty",
                _ => _ is PropertiesChildItem && _.RelatedNode() is StateMachineNode)
                .SetNameFormat("{0} State Changed")
                .SetDescription(
                    "Binding to a state property creates methods for each state, and in the designer code will property call the each state's method when it changes.")
                .ImplementWith(args =>
                {
                    args.Method.Parameters[0].Type = typeof (State).ToCodeReference();
                    var sourceItem = args.SourceItem as ITypedItem;
                    var stateMachine = sourceItem.RelatedNode() as StateMachineNode;
                    if (args.IsDesignerFile)
                    {
                        foreach (var state in stateMachine.States)
                        {
                            var method = new CodeMemberMethod()
                            {
                                Name = "On" + state.Name,
                                Attributes = MemberAttributes.Public
                            };
                            var conditionStatement =
                                new CodeConditionStatement(
                                    new CodeSnippetExpression(string.Format("arg1 is {0}", state.Name)));
                            conditionStatement.TrueStatements.Add(
                                new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), method.Name));

                            args.Method.Statements.Add(conditionStatement);
                            args.Decleration.Members.Add(method);

                        }

                    }

                    args.Method.Parameters[0].Type = "Invert.StateMachine.State".ToCodeReference();
                });

            container.AddBindingMethod(typeof (ViewBindings), "BindCollection", _ => _ is CollectionsChildItem)
                .SetNameFormat("{0} Collection Changed")
                .SetDescription(
                    "Collection bindings bind to a collection giving you two methods, {CollectionName}Added, and {CollectionName}Removed, override these methods to execute something when the collection is modified.")
                ;

            container.AddBindingMethod(typeof (ViewBindings), "BindToViewCollection", _ => _ is CollectionsChildItem)
                .SetNameFormat("{0} View Collection Changed")
                .SetDescription(
                    "The view collection changed binding automatically creates views for each element's viewmodel when created.")
                .ImplementWith(args =>
                {

                    if (args.Method.Name.EndsWith("CreateView"))
                    {
                        args.Method.Parameters[0].Name = "viewModel";
                        args.Method._("return InstantiateView(viewModel)");
                    }
                    else
                    {
                        args.Method.Parameters[0].Name = "view";
                    }
                })
                ;

            container.AddBindingMethod(typeof (ViewBindings), "BindCommandExecuted", _ => _ is CommandsChildItem)
                .SetNameFormat("{0} Executed")
                .SetDescription(
                    "The executed binding is for listening to when a command is invoked on a view.  It will provide you with a method in the format {CommandName}Executed({CommandClass} data)")

                .ImplementWith(args =>
                {

                    args.Method.Parameters[0].Name = "command";
                    var commandItem = args.SourceItem as CommandsChildItem;
                    args.Method.Parameters[0].Type = commandItem.ClassName.ToCodeReference();
                })
                ;

            container.AddBindingMethod(typeof (UGUIExtensions), "BindInputFieldToProperty", // Registration
                _ => _ is PropertiesChildItem && _.RelatedTypeName == typeof (string).Name) // Validation
                .SetDescription(
                    "Binds a string property to an uGUI input field.  A field will be created on the view for specifying the uGUI field.")
                .SetNameFormat("{0} To Input Field") // Configuration
                ;

            container.AddBindingMethod(typeof (UGUIExtensions), "BindButtonToCommand", _ => _ is CommandsChildItem)
                .SetDescription(
                    "The ButtonToCommand binding will create a reference to a uGUI button on the view and automatically wire the click event to invoke the command.")
                .SetNameFormat("{0} To Button");
            container.AddBindingMethod(typeof (UGUIExtensions), "BindToggleToProperty",
                _ => _ is PropertiesChildItem && _.RelatedTypeName == typeof (bool).Name)
                .SetDescription("Bind toggle to property will bind a boolean property directly to a uGUI toggle box.")
                .SetNameFormat("{0} To Toggle");
            container.AddBindingMethod(typeof (UGUIExtensions), "BindTextToProperty",
                _ => _ is PropertiesChildItem && _.RelatedTypeName == typeof (string).Name)
                .SetDescription("Binds a string property to a uGUI text label.")
                .SetNameFormat("{0} To Text");

            container.AddBindingMethod(typeof (UGUIExtensions), "BindSliderToProperty",
                _ => _ is PropertiesChildItem && _.RelatedTypeName == typeof (float).Name)
                .SetDescription("Binds a slider to a float value.")
                .SetNameFormat("{0} To Slider");

        }
    }
}