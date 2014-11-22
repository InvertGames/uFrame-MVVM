using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invert.Core;
using Invert.Core.GraphDesigner;
using Invert.uFrame;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using UnityEditor;
using UnityEngine;

namespace Assets.uFrameComplete.uFrame.Editor.DiagramPlugins.UnityVS
{
    public class UnityVSPlugin : DiagramPlugin
    {
        public override bool EnabledByDefault
        {
            get { return false; }
        }

        public override void Initialize(uFrameContainer container)
        {
            InvertGraphEditor.HookCommand<IToolbarCommand>("SaveCommand", new HookCommand(() =>
            {
                    EditorApplication.ExecuteMenuItem("Visual Studio Tools/Generate Project Files");
            }));
            
        }
    }
}
