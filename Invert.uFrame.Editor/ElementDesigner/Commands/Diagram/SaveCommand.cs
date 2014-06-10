
using System.IO;
using System.Linq;
using Invert.uFrame.Editor.Refactoring;
using UnityEditor;
using UnityEngine;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public class SaveCommand : ElementsDiagramToolbarCommand
    {
        public override void Perform(ElementsDiagram node)
        {
            // Go ahead and process any code refactors
            ProcessRefactorings(node);

            //var codeGenerators = uFrameEditor.GetAllCodeGenerators(item.Data).ToArray();

            var fileGenerators = uFrameEditor.GetAllFileGenerators(node.Data).ToArray();
            
            foreach (var codeFileGenerator in fileGenerators)
            {
                // Grab the information for the file
                var fileInfo = new FileInfo(System.IO.Path.Combine(node.CodePathStrategy.AssetPath, codeFileGenerator.Filename));
                // Make sure we are allowed to generate the file
                if (!codeFileGenerator.CanGenerate(fileInfo)) continue;
                // Get the path to the directory
                var directory = System.IO.Path.GetDirectoryName(fileInfo.FullName);
                // Create it if it doesn't exist
                if (directory != null && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                // Write the file
                File.WriteAllText(fileInfo.FullName, codeFileGenerator.ToString());
                //Debug.Log("Created file: " + fileInfo.FullName);

            }
            AssetDatabase.Refresh();

            node.Save();
            node.DeselectAll();
            node.Refresh();
        }

        /// <summary>
        /// Execute all the refactorings queued in the diagram
        /// </summary>
        public void ProcessRefactorings(ElementsDiagram diagram)
        {
            var refactorer = new RefactorContext(diagram.Data.Refactorings);
            Debug.Log(diagram.CodePathStrategy.AssetPath);
            var files = uFrameEditor.GetAllFileGenerators(diagram.Data).Where(p=>!p.Filename.EndsWith(".designer.cs")).Select(p => System.IO.Path.Combine(diagram.CodePathStrategy.AssetPath, p.Filename)).ToArray();


            if (refactorer.Refactors.Count > 0)
            {
                refactorer.Refactor(files);
            }
            UnityEngine.Debug.Log(string.Format("Applied {0} refactors.", refactorer.Refactors.Count));
            diagram.Data.RefactorApplied();
        }
    }

    public class PrintPlugins : ElementsDiagramToolbarCommand
    {
        public override string Name
        {
            get { return "Print Plugins"; }
        }

        public override void Perform(ElementsDiagram node)
        {
            foreach (var diagramPlugin in uFrameEditor.GetAllCodeGenerators(node.Data))
            {
                UnityEngine.Debug.Log((diagramPlugin.IsDesignerFile ? "Designer File" : "Editable File" ) + 
                    diagramPlugin.GetType().Name + diagramPlugin.Filename );
            }
        }
    }
}