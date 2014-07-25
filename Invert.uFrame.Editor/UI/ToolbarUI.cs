using System.Collections.Generic;
using System.Linq;
using Invert.Common;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using UnityEditor;
using UnityEngine;

namespace Invert.uFrame.Editor
{
    public class ToolbarUI : ICommandUI
    {
        public ToolbarUI()
        {
            LeftCommands = new List<IEditorCommand>();
            RightCommands = new List<IEditorCommand>();
            BottomLeftCommands = new List<IEditorCommand>();
            BottomRightCommands = new List<IEditorCommand>();
            AllCommands = new List<IEditorCommand>();
        }

        public List<IEditorCommand> AllCommands { get; set; }

        public List<IEditorCommand> LeftCommands { get; set; }
        public List<IEditorCommand> RightCommands { get; set; }
        public List<IEditorCommand> BottomLeftCommands { get; set; }
        public List<IEditorCommand> BottomRightCommands { get; set; }
       

        public void AddCommand(IEditorCommand command)
        {
            AllCommands.Add(command);
            var cmd = command as IToolbarCommand;
            if (cmd == null || cmd.Position == ToolbarPosition.Right)
            {
                RightCommands.Add(command);
            }
            else if (cmd.Position == ToolbarPosition.BottomLeft)
            {
                BottomLeftCommands.Add(command);
            }else if (cmd.Position == ToolbarPosition.BottomRight)
            {
                BottomRightCommands.Add(command);
            }
            else
            {
                LeftCommands.Add(command);
            }
        }

 

        public void Go()
        {
            foreach (var editorCommand in LeftCommands.OrderBy(p=>p.Order))
            {
                DoCommand(editorCommand);
            }
            GUILayout.FlexibleSpace();
          
           
            foreach (var editorCommand in RightCommands.OrderBy(p => p.Order))
            {
                DoCommand(editorCommand);
            }

           
        }

        public void GoBottom()
        {
            var scale = GUILayout.HorizontalSlider(ElementDesignerStyles.Scale, 0.55f, 1f, GUILayout.Width(200f));
            if (scale != ElementDesignerStyles.Scale)
            {
                ElementDesignerStyles.Scale = scale;
                Handler.ExecuteCommand(new ScaleCommand() { Scale = scale });

            }
            foreach (var editorCommand in BottomLeftCommands.OrderBy(p => p.Order))
            {
                DoCommand(editorCommand);
            }
            GUILayout.FlexibleSpace();
            foreach (var editorCommand in BottomRightCommands.OrderBy(p => p.Order))
            {
                DoCommand(editorCommand);
            }
            
        }
        public ICommandHandler Handler { get; set; }

        public void DoCommand(IEditorCommand command)
        {
            var obj = Handler.ContextObjects.FirstOrDefault(p => command.For.IsAssignableFrom(p.GetType()));
            GUI.enabled = command.CanPerform(obj) == null;
            if (command is IDynamicOptionsCommand)
            {
                var cmd = command as IDynamicOptionsCommand;
                

                foreach (var ufContextMenuItem in cmd.GetOptions(obj))
                {
                    if (GUILayout.Button(new GUIContent(ufContextMenuItem.Name), EditorStyles.toolbarButton))
                    {
                        cmd.SelectedOption = ufContextMenuItem;
                        Handler.ExecuteCommand(command);
                    }
                }
            }
            else if (GUILayout.Button(new GUIContent(command.Title), EditorStyles.toolbarButton))
            {

                if (command is IParentCommand)
                {
                    var contextUI = uFrameEditor.CreateCommandUI<ContextMenuUI>(Handler, command.GetType());
                    contextUI.Flatten = true;
                    contextUI.Go();
                }
                else
                {
                    Handler.ExecuteCommand(command);
                }
            }
            GUI.enabled = true;
        }
    }
}