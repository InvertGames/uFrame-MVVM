using uFrame.MVVM.Templates;

namespace Invert.uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class ComputedPropertyPage : ComputedPropertyPageBase {
        public override Type ParentPage
        {
            get { return typeof (ElementPage); }
        }

        public override string Name
        {
            get { return "Computed Properties"; }
        }

        public override decimal Order
        {
            get { return 1; }
        }

        public override IEnumerable<ScaffoldGraph> Scaffolds()
        {
            ElementNode ele;
            yield return ScaffoldComputed(out ele);
        }

        private static ScaffoldGraph ScaffoldComputed(out ElementNode ele)
        {
            var scaffoldComputed = new ScaffoldGraph() {Name = "Computed Property Scaffold"};
            PropertiesChildItem healthProperty;
            var element = scaffoldComputed
                .BeginNode<ElementNode>("Player")
                .AddItem("Health", out healthProperty, "float")
                .EndNode();
            var computed = scaffoldComputed
                .BeginNode<ComputedPropertyNode>("IsDead")
                .EndNode();

            scaffoldComputed.AddConnection(healthProperty, computed);
            ele = element as ElementNode;
            return scaffoldComputed;
        }

        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
            ElementNode elementNode;
            ScaffoldComputed(out elementNode);
           
            _.ImageByUrl("http://i.imgur.com/qeDjrVR.png");
            _.Paragraph("A Computed Property is an extremely powerful feature added to uFrame's ViewModel layer.  These properties can calculate their value based on other properties, and are recalculated whenever one of those change.  So for example, if you have a boolean IsDead computed property on a PlayerViewModel, dependent on a Player.Health property, uFrame will generate these three things on that PlayerViewModel for you to override:");
            _.Break();
            _.Title2("Under the hood");
            _.TemplateExample<ViewModelTemplate, ElementNode>(elementNode, true, "ComputedDependents", "ResetComputed", "Compute");

            _.Title3("ResetIsDead");
            _.Paragraph("Usually fine without overriding, used to set up the computed observable");

            _.Title3("ComputeIsDead");
            _.Paragraph("A \"getter\" used to compute the property, based on dependents");

            _.Title3("GetIsDeadDepedents");
            _.Paragraph("This where you would override and provide additional dependents, if needed");

            _.Break();
            _.Note("It's important to correctly set all the dependents, because the computed only knows when it needs to be recalculated by observing these dependents for changes.  This is done in the diagram by dragging a link from a ViewModel's property to the computed, or in code by overriding the Get{ComputedName}Dependents() function.");
        }
    }
}
