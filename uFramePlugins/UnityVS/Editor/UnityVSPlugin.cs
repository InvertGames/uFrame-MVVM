using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Invert.uFrame;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using UnityEditor;

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
            uFrameEditor.HookCommand<IToolbarCommand>("SaveCommand", new HookCommand(() =>
            {
                try
                {
                    

                    EditorApplication.ExecuteMenuItem("Visual Studio Tools/Generate Project Files");
                    //EditorApplication.ExecuteMenuItem("UnityVS/Generate Project Files");
                }
                catch
                {
                    
                }
            }));
            
        }
    }
}
