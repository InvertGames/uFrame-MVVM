using System.CodeDom;
using Invert.Core;

namespace Invert.uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class ViewBindingsPage : ViewBindingsPageBase {
        private IEnumerable<uFrameBindingType> _uFrameBindingTypes;

        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
            if (_uFrameBindingTypes == null)
            _uFrameBindingTypes = InvertApplication.Container.ResolveAll<uFrameBindingType>();
            foreach (var item in _uFrameBindingTypes)
            {
                _.Title2(item.DisplayFormat,"{Name}");
                _.Paragraph(item.Description);
                
                //var tempDecleration = new CodeTypeDeclaration();
                //item.CreateBindingSignature(new CreateBindingSignatureParams(tempDecleration,_=>new CodeTypeReference("PROPERTY_TYPE"), ))
             //   item.CreateBindingSignature()
            }
        }
    }
}
