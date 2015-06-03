using uFrame.MVVM.Templates;

namespace Invert.uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class ViewScenePropertiesPage : ViewScenePropertiesPageBase {
        
        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
            _.Paragraph("Usually a property binding is a one-way stream of information, where a View is simply receiving information about a property's changing values.  In a game environment however, there are times where it makes sense to allow Views to actually determine these values as well, for things like a GameObject's position, rotation, etc.  This is where two-way bindings are needed.");
            _.Paragraph("Scene Properties are two-way bindings, and allows for a View to calculate and set a property on its ViewModel.  This is done in an observable way, and when adding a scene property to a view, 3 specific methods are made available.  For example, adding a Position scene property on a PlayerView will result in these underlying base methods:");

            var graph = new ScaffoldGraph();
            PropertiesChildItem positionProperty;
            var player = graph.BeginNode<ElementNode>("Player")
                .AddItem("Position", out positionProperty, "Vector3")
                .EndNode();

            var view = (ViewNode) graph
                .BeginNode<ViewNode>("PlayerView")
                
                .EndNode()
                ;
            graph.AddConnection(player, view.ElementInputSlot);
            graph.AddConnection(positionProperty,view.ScenePropertiesInputSlot);

            _.Break();
            _.Title2("Under the hood. Generated Scene Properties");
            _.TemplateExample<ViewTemplate, ViewNode>(view, true,new [] {"ResetProperty", "CalculateProperty", "GetPropertyObservable", "Bind"});
            _.Break();
            _.Title3("ResetPosition");
            _.Paragraph("ResetPosition() is mostly used to initialize the binding, is called in the View's base Bind() method, and typically doesn't need to be overridden and altered.");
            _.Break();
            _.Title3("CalculatePosition");
            _.Paragraph("CalculatePosition() is the main method you would override on your generated PlayerView, where you would return a Vector3 to give the player's position.");
            _.ShowGist("2e5c9b6d07b76bcaedb1","CalculatePosition.cs");
            _.Break();
            _.Title3("GetPositionObservable");
            _.Paragraph("GetPositionObservable() should only be overridden in cases where you have a more convenient or performant method of observing the scene property change, because as you see, this calculation is happening every Update by default.  In this case, we know that ViewBase is already monitoring a TransformChangedObservable (and specifically a PositionChangedObservable as well), so on our PlayerView we would override the GetPositionObservable like this:");
            _.ShowGist("3d0e2f8bd65a044e82d6","GetPositionObservable.cs");
            _.Break();
        }
    }
}
