using System;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class DeleteCommand : EditorCommand<IDiagramNode>, IDiagramNodeCommand,IKeyBindable
    {
        public override string Group
        {
            get { return "File"; }
        }

        public override decimal Order
        {
            get { return 3; }
        }

        public override void Perform(IDiagramNode node)
        {

            if (node == null) return;
            var pathStrategy = EditorWindow.GetWindow<ElementsDesigner>().Diagram.Data.Settings.CodePathStrategy;
   
            var generators = uFrameEditor.GetAllCodeGenerators(pathStrategy, node.Data)
                .Where(p => !p.IsDesignerFile && p.ObjectData == node).ToArray();

            var customFiles = generators.Select(p=>p.Filename).ToArray();
            var customFileFullPaths = generators.Select(p=>System.IO.Path.Combine(pathStrategy.AssetPath, p.Filename)).Where(File.Exists).ToArray();

            if (node is IDiagramFilter)
            {
                var filter = node as IDiagramFilter;
                if (filter.Locations.Keys.Count > 1)
                {
                    EditorUtility.DisplayDialog("Delete sub items first.",
                        "There are items defined inside this item please hide or delete them before removing this item.", "OK");
                    return;
                }
            }
            if (EditorUtility.DisplayDialog("Confirm", "Are you sure you want to delete this?", "Yes", "No"))
            {
                node.RemoveFromDiagram();
                if (customFileFullPaths.Length > 0)
                {
                    if (EditorUtility.DisplayDialog("Confirm",
                        "You have files associated with this. Delete them to?" + Environment.NewLine +
                        string.Join(Environment.NewLine, customFiles), "Yes Delete Them", "Don't Delete them"))
                    {
                        foreach (var customFileFullPath in customFileFullPaths)
                        {
                            File.Delete(customFileFullPath);
                        }
                        var saveCommand = uFrameEditor.Container.Resolve<IToolbarCommand>("SaveCommand");
                        //Execute the save command
                        EditorWindow.GetWindow<ElementsDesigner>().ExecuteCommand(saveCommand);
                    }
                }
            }
        }

        public override string CanPerform(IDiagramNode node)
        {
            return null;
        }
    }
}