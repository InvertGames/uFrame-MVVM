using System.Collections.Generic;

using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Data;
using UnityEditor;
using UnityEngine;

namespace Assets.uFrameComplete.uFrame.Editor.DiagramPlugins
{
    public class UBehavioursPlugin : DiagramPlugin
    {
  

        public override void Initialize()
        {
            Debug.Log("UBehaviours Plugin Initialized.");
        }

        public override IElementDrawer GetDrawer(ElementsDiagram diagram, IDiagramItem data)
        {
            if (data is ViewData)
            {
                return new UBehavioursViewDrawer(data as ViewData,diagram);
            }
            return null;
        }

    
    }
}