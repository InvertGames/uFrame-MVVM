using System.CodeDom;
using Invert.Core;
using Invert.Core.GraphDesigner;
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
        container.AddBindingMethod(typeof(ViewBindings), "BindProperty", _ => _ is PropertiesChildItem).DisplayFormat = "{0}Changed";
        container.AddBindingMethod(typeof(ViewBindings), "BindCollection", _ => _ is CollectionsChildItem).DisplayFormat = "{0}Changed";

        container.AddBindingMethod(typeof(ViewBindings), "BindToViewCollection", _ => _ is CollectionsChildItem)
            .DisplayFormat = " Changed With View";

        container.AddBindingMethod(typeof(ViewBindings), "BindCommandExecuted", _ => _ is CommandsChildItem)
            .SetNameFormat("{0}Executed")
            .ImplementWith(args =>
            {
                args.Method.Comments.Add(new CodeCommentStatement("BLABLABLABLA"));
            })
            ;


        //container.RegisterGraphItem<SubsystemNode, SubsystemNodeViewModel, SubsystemNodeDrawer>();

    }
}