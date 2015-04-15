namespace Invert.uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class SimpleClassPage : SimpleClassPageBase {
        public override string Name
        {
            get { return "Simple Classes"; }
        }

        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);

            _.Paragraph(
                "These are basically simple classes that automatically implement " +
                "INotifyPropertyChanged, which can help when quickly encapsulating " +
                "portable data and bridging uFrame with other Unity assets, libraries," +
                " and frameworks.  Class nodes can be used as Element node properties " +
                "and command parameters, which adds a whole new level of flexibility.");

        }
    }
}
