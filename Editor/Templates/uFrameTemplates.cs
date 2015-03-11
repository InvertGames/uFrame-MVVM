using System.CodeDom;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.StateMachine;
using Invert.uFrame.MVVM;

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
        // Register the code templates
        Framework.Element.AddCodeTemplate<ViewModelTemplate>();
        Framework.Element.AddCodeTemplate<ControllerTemplate>();
        Framework.SceneManager.AddCodeTemplate<SceneManagerTemplate>();
        Framework.SceneManager.AddCodeTemplate<SceneManagerSettingsTemplate>();
        Framework.View.AddCodeTemplate<ViewTemplate>();
        Framework.ViewComponent.AddCodeTemplate<ViewComponentTemplate>();
        Framework.State.AddCodeTemplate<StateTemplate>();
        Framework.StateMachine.AddCodeTemplate<StateMachineTemplate>();

        // Register our bindable methods
        container.AddBindingMethod(typeof(ViewBindings), "BindProperty", _ => _ is PropertiesChildItem)
            .SetNameFormat("{0} Changed")
            .ImplementWith(args =>
            {
                if (args.SourceItem.OutputTo<StateMachineNode>() != null)
                {
                    args.Method.Parameters.Clear();
                    args.Method.Parameters.Add(new CodeParameterDeclarationExpression(typeof (State), "State"));
                }
            })
            ;
        container.AddBindingMethod(typeof(ViewBindings), "BindTwoWay", _ => _ is PropertiesChildItem)
            .SetNameFormat("{0} Two Way")
            .ImplementWith(args =>
            {
                if (args.SourceItem.OutputTo<StateMachineNode>() != null)
                {
                    args.Method.Parameters.Clear();
                    args.Method.Parameters.Add(new CodeParameterDeclarationExpression(typeof (State), "State"));
                }
            });
        container.AddBindingMethod(typeof(ViewBindings), "BindStateProperty", _ => _ is PropertiesChildItem && _.OutputTo<StateMachineNode>() != null)

            .SetNameFormat("{0} State Changed")
            .ImplementWith(args =>
            {
                var stateMachine = args.SourceItem.OutputTo<StateMachineNode>();

                foreach (var state in stateMachine.States)
                {
                    var method = new CodeMemberMethod()
                    {
                        Name = "On" + state.Name,
                        Attributes = MemberAttributes.Public
                    };


                    //if ()
                    //{
                    //    method.Attributes = MemberAttributes.Public | MemberAttributes.Override;
                    //    method.Statements.Add(new CodeSnippetExpression(string.Format("base.{0}()", method.Name)));

                    //}
                    //else
                    //{
                        var conditionStatement =
                        new CodeConditionStatement(
                            new CodeSnippetExpression(string.Format("value is {0}", state.Name)));
                        conditionStatement.TrueStatements.Add(
                            new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), method.Name));

                        args.Method.Statements.Add(conditionStatement);
                    //}

                    args.Decleration.Members.Add(method);

                }
            });

        container.AddBindingMethod(typeof(ViewBindings), "BindCollection", _ => _ is CollectionsChildItem)
            .SetNameFormat("{0} Collection Changed");

        container.AddBindingMethod(typeof(ViewBindings), "BindToViewCollection", _ => _ is CollectionsChildItem)
            .DisplayFormat = "{0} View Collection Changed";

        container.AddBindingMethod(typeof(ViewBindings), "BindCommandExecuted", _ => _ is CommandsChildItem)
            .SetNameFormat("{0} Executed")
            .ImplementWith(args =>
            {
                
            })
            ;

        container.AddBindingMethod(typeof(UGUIExtensions), "BindInputFieldToProperty", _ => _ is PropertiesChildItem && _.RelatedTypeName == typeof(string).Name)
            .SetNameFormat("{0} To Input Field"); 
        container.AddBindingMethod(typeof(UGUIExtensions), "BindButtonToCommand", _ => _ is CommandsChildItem)
            .SetNameFormat("{0} To Button");
        container.AddBindingMethod(typeof(UGUIExtensions), "BindToggleToProperty", _ => _ is PropertiesChildItem && _.RelatedTypeName == typeof(bool).Name)
            .SetNameFormat("{0} To Toggle");
        //container.RegisterGraphItem<SubsystemNode, SubsystemNodeViewModel, SubsystemNodeDrawer>();

    }
}