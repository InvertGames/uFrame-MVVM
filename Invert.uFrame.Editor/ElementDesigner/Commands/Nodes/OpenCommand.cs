using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class OpenCommand : EditorCommand<IDiagramNode>, IDynamicOptionsCommand, IDiagramNodeCommand
    {
        public override string Group
        {
            get { return "File"; }
        }
        public override void Perform(IDiagramNode node)
        {
            var generator = SelectedOption.Value as CodeGenerator;
            if (generator == null) return;
            var pathStrategy = EditorWindow.GetWindow<ElementsDesigner>().Diagram.Data.Settings.CodePathStrategy;
            var filePath = System.IO.Path.Combine(pathStrategy.AssetPath, generator.Filename);
            //var filename = repository.GetControllerCustomFilename(this.Name);
            var scriptAsset = AssetDatabase.LoadAssetAtPath(filePath, typeof(TextAsset));
            AssetDatabase.OpenAsset(scriptAsset);
        }

        public override string CanPerform(IDiagramNode node)
        {
            return null;
        }

        public IEnumerable<UFContextMenuItem> GetOptions(object item)
        {
            var diagramItem = item as IDiagramNode;
            if (diagramItem == null) yield break;
            var generators = uFrameEditor.GetAllCodeGenerators(diagramItem.Data.Settings.CodePathStrategy, diagramItem.Data)
                .Where(p =>!p.IsDesignerFile && p.ObjectData == item);

            foreach (var codeGenerator in generators)
            {
                yield return new UFContextMenuItem()
                {
                    Name = "Open/" + codeGenerator.Filename,
                    Value = codeGenerator
                };
            }
        }

        public UFContextMenuItem SelectedOption { get; set; }
        public MultiOptionType OptionsType { get; private set; }
    }
}