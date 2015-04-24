namespace Invert.uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class ElementCommandsPage : ElementCommandsPageBase {
        public override bool ShowInNavigation
        {
            get { return base.ShowInNavigation; }
        }

        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
            _.Paragraph("Commands define a property of type ISignal<TCommandClass> on an element.  These act as a delegate to a pre-initialized handler." +
                        "The handlers are setup in the controllers initialize method to point to a method, in this method you are passed the view-model instance." +
                        "");

            var ele = new ScaffoldGraph()
                .BeginNode<ElementNode>("Player")
                .AddItem<CommandsChildItem>("Hit")
            .EndNode();

            _.Title2("Generated Model Commands");
            _.TemplateExample<ViewModelTemplate, ElementNode>(ele as ElementNode, true, "CommandItems", "Bind");
            _.Break();
            _.Title2("Generated Controller Command Handlers");
            _.TemplateExample<ControllerTemplate, ElementNode>(ele as ElementNode, true, "CommandItems", "CommandMethod");
            _.Break();


            _.Break();
            _.Title3("Also See");
            _.LinkToPage<Controllers>();

        }
    }
}
