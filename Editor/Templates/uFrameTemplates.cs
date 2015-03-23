using System.CodeDom;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.StateMachine;
using Invert.uFrame.MVVM;
using UnityEngine;

public class uFrameTemplates : DiagramPlugin
{
    // Make sure this plugin loads after the framework plugin loads
    public override decimal LoadPriority
    {
        get { return 5; }
    }

    private uFrameMVVM Framework { get; set; }

    public override void Initialize(uFrameContainer container)
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
        Framework.SceneManager.AddCodeTemplate<SceneManagerTemplate>();
        Framework.SceneManager.AddCodeTemplate<SceneManagerSettingsTemplate>();
        Framework.View.AddCodeTemplate<ViewTemplate>();
        Framework.ViewComponent.AddCodeTemplate<ViewComponentTemplate>();
        Framework.State.AddCodeTemplate<StateTemplate>();
        Framework.StateMachine.AddCodeTemplate<StateMachineTemplate>();

        // Register our bindable methods
        container.AddBindingMethod(typeof(ViewBindings), "BindProperty", _ => _ is PropertiesChildItem || _ is ComputedPropertyNode)
            .SetNameFormat("{0} Changed")
            .ImplementWith(args =>
            {
                var sourceItem = args.SourceItem as ITypedItem;

                if (sourceItem.RelatedNode() is StateMachineNode)
                {
                    args.Method.Parameters.Clear();
                    args.Method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(State), "State"));
                }
            })
            ;

        container.AddBindingMethod(typeof(ViewBindings), "BindStateProperty", _ => _ is PropertiesChildItem && _.RelatedNode() is StateMachineNode)
            .SetNameFormat("{0} State Changed")
            .ImplementWith(args =>
            {
                args.Method.Parameters[0].Type = typeof(State).ToCodeReference();
                var sourceItem = args.SourceItem as ITypedItem;
                var stateMachine = sourceItem.RelatedNode() as StateMachineNode;

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
            });

        container.AddBindingMethod(typeof(ViewBindings), "BindCollection", _ => _ is CollectionsChildItem)
            .SetNameFormat("{0} Collection Changed");

        container.AddBindingMethod(typeof(ViewBindings), "BindToViewCollection", _ => _ is CollectionsChildItem)
            .SetNameFormat("{0} View Collection Changed")
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

        container.AddBindingMethod(typeof(ViewBindings), "BindCommandExecuted", _ => _ is CommandsChildItem)
            .SetNameFormat("{0} Executed")
            .ImplementWith(args =>
            {
                args.Method.Parameters[0].Name = "command";
                var commandItem = args.SourceItem as CommandsChildItem;
                args.Method.Parameters[0].Type = commandItem.ClassName.ToCodeReference();
            })
            ;

        container.AddBindingMethod(typeof(UGUIExtensions), "BindInputFieldToProperty", // Registration
            _ => _ is PropertiesChildItem && _.RelatedTypeName == typeof(string).Name) // Validation
            .SetNameFormat("{0} To Input Field")// Configuration
            ;

        container.AddBindingMethod(typeof(UGUIExtensions), "BindButtonToCommand", _ => _ is CommandsChildItem)
            .SetNameFormat("{0} To Button");
        container.AddBindingMethod(typeof(UGUIExtensions), "BindToggleToProperty", _ => _ is PropertiesChildItem && _.RelatedTypeName == typeof(bool).Name)
            .SetNameFormat("{0} To Toggle");
        container.AddBindingMethod(typeof(UGUIExtensions), "BindTextToProperty", _ => _ is PropertiesChildItem && _.RelatedTypeName == typeof(string).Name)
            .SetNameFormat("{0} To Text");

    }

}