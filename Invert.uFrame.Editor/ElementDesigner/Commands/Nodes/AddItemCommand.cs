using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public abstract class AddItemCommand<TType> : AddNewCommand, IDiagramContextCommand
    {
        public override string CanPerform(ElementsDiagram node)
        {
            if (node == null) return "Diagram must be loaded first.";

            if (!node.Data.CurrentFilter.IsAllowed(null, typeof(TType)))
                return "Item is not allowed in this part of the diagram.";

            return null;
        }

        public override string Path
        {
            get { return  "Add New/" + Title; }
        }
    }

    public interface IChildCommand
    {
        Type ChildCommandFor { get; }
    }

    public class RenameCommand : EditorCommand<IDiagramNode>, IDiagramNodeCommand
    {
        public override void Perform(IDiagramNode node)
        {
            node.BeginEditing();
        }

        public override string CanPerform(IDiagramNode node)
        {
            if (node == null) return "Invalid argument";
            return null;
        }
    }

    public interface IDiagramContextCommand
    {
        
    }
    public interface IDiagramNodeCommand
    {
        
    }

    public interface IDiagramNodeItemCommand
    {
        
    }
    
    public class DeleteCommand : EditorCommand<IDiagramNode>, IDiagramNodeCommand
    {
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
    public class HideCommand : EditorCommand<IDiagramNode>, IDiagramNodeCommand
    {
        public override void Perform(IDiagramNode node)
        {
            node.Data.CurrentFilter.Locations.Remove(node.Identifier);
        }

        public override string CanPerform(IDiagramNode node)
        {
            if (node == null) return "Diagram Item must not be null.";
            return null;
        }
    }

    public class OpenCommand : EditorCommand<IDiagramNode>, IDynamicOptionsCommand, IDiagramNodeCommand
    {
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

    public class RemoveLinkCommand : EditorCommand<IDiagramNode>, IDynamicOptionsCommand, IDiagramNodeCommand
    {
        public override void Perform(IDiagramNode node)
        {
            var link = SelectedOption.Value as IDiagramLink;

            if (link != null) 
                link.Source.RemoveLink(node);
        }

        public override string CanPerform(IDiagramNode node)
        {
            return null;
        }

        public IEnumerable<UFContextMenuItem> GetOptions(object item)
        {
            var diagramItem = item as IDiagramNode;
            if (diagramItem == null) yield break;
            var links = diagramItem.Data.Links.Where(p => p.Target == item);
            foreach (var link in links)
            {
                yield return new UFContextMenuItem()
                {
                    Checked = true,
                    Name = "Remove Link From/" + link.Source.Label,
                    Value = link
                };
            }

        }

        public UFContextMenuItem SelectedOption { get; set; }
        public MultiOptionType OptionsType { get; private set; }
    }

    public class SelectViewBaseElement : EditorCommand<ViewData>, IDynamicOptionsCommand, IDiagramNodeCommand
    {
        public override string Group
        {
            get { return "View"; }
        }
        public override void Perform(ViewData node)
        {
            if (node == null) return;
            var view = SelectedOption.Value as ViewData;
            if (view == null)
            {
                node.BaseViewIdentifier = null;
            }
            else
            {
                node.BaseViewIdentifier = view.Identifier;    
            }
            
        }

        public override string CanPerform(ViewData node)
        {
            if (node == null) return "This operation can only be performed on a view.";
            return null;
        }

        public IEnumerable<UFContextMenuItem> GetOptions(object item)
        {
            var view = item as ViewData;
            if (view == null) yield break;
            var element = view.ViewForElement;
            if (element == null) yield break;

            var baseViews = element.AllBaseTypes.SelectMany(
                p => view.Data.AllDiagramItems.OfType<ViewData>().Where(x => x.ViewForElement == p));
            yield return new UFContextMenuItem()
            {
                Name = "Base View/" + element.NameAsViewBase,
                Value = null
            };
            foreach (var baseView in baseViews)
            {
                yield return new UFContextMenuItem()
                {
                    Name = "Base View/" + baseView.NameAsView,
                    Value = baseView
                };
            }

        }

        public UFContextMenuItem SelectedOption { get; set; }
        public MultiOptionType OptionsType { get; private set; }
    }

    public class ScaleCommand : EditorCommand<float>
    {
        public float Scale { get; set; }

        public override void Perform(float node)
        {

            UFStyles.Scale = Scale;

            
        }

        public override string CanPerform(float node)
        {
            if (Scale < 0.5f)
            {
                return "Can't scale that small";
            }
            return null;
        }
    }
}
