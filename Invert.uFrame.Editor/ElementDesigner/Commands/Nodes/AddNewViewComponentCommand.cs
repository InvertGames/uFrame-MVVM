using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Invert.uFrame.Editor.ElementDesigner.Commands
{
    public class AddNewViewComponentCommand : AddItemCommand<ViewComponentData>
    {
        public override string Title
        {
            get { return "Add New View Component"; }
        }
        public override void Perform(ElementsDiagram node)
        {
            var data = new ViewComponentData()
            {
                Data = node.Data,
                Name = node.Data.GetUniqueName("NewViewComponent"),
                Location = new Vector2(15, 15)
            };
            node.Data.AddNode(data);
            data.Location = node.LastMouseDownPosition;
        }
    }

    public class ShowItemCommand : EditorCommand<IElementDesignerData>,
        IToolbarCommand, IDiagramContextCommand, IDynamicOptionsCommand
    {
        public override void Perform(IElementDesignerData node)
        {
            var diagramItem = SelectedOption.Value as IDiagramNode;
            node.CurrentFilter.Locations[diagramItem] = new Vector2(0f, 0f);
        }

        public override string CanPerform(IElementDesignerData node)
        {
            if (node == null) return "Designer Data must not be null";
            return null;
        }

        public ToolbarPosition Position
        {
            get
            {
                return ToolbarPosition.Right;
            }
        }

        public IEnumerable<UFContextMenuItem> GetOptions(object item)
        {
            var designerData = item as IElementDesignerData;
            foreach (var importable in designerData.GetImportableItems())
            {
                yield return new UFContextMenuItem()
                {
                    Name = "Show Item/" + importable.Name,
                    Value = importable
                };
            }

        }

        public UFContextMenuItem SelectedOption { get; set; }

        public MultiOptionType OptionsType
        {
            get
            {
                return MultiOptionType.DropDown;
            }
        }
    }
    public class ShowExternalItemCommand : EditorCommand<IElementDesignerData>,
        IToolbarCommand, IDiagramContextCommand, IDynamicOptionsCommand
    {
        public override void Perform(IElementDesignerData node)
        {
            var diagramItem = (KeyValuePair<IElementDesignerData, SubSystemData>)SelectedOption.Value;
            var externalNode = new ExternalSubsystem()
            {
                ExternalDiagramIdentifier = diagramItem.Key.Identifier,
                ExternalNodeIdentifier = diagramItem.Value.Identifier,


            };
            node.AddNode(externalNode);

            node.CurrentFilter.Locations[externalNode] = new Vector2(15f, 15f);
        }

        public override string CanPerform(IElementDesignerData node)
        {
            if (node == null) return "Designer Data must not be null";
            return null;
        }

        public ToolbarPosition Position
        {
            get
            {
                return ToolbarPosition.Right;
            }
        }

        public IEnumerable<UFContextMenuItem> GetOptions(object item)
        {
            //var designerData = item as IElementDesignerData;
            foreach (var diagram in UFrameAssetManager.Diagrams)
            {
                foreach (var importable in diagram.SubSystems.OfType<SubSystemData>())
                {
                    yield return new UFContextMenuItem()
                     {
                         Name = "Show External Item/" + importable.Name,
                         Value = new KeyValuePair<IElementDesignerData,SubSystemData>(diagram,importable)
                     };
                }

            }

        }

        public UFContextMenuItem SelectedOption { get; set; }

        public MultiOptionType OptionsType
        {
            get
            {
                return MultiOptionType.DropDown;
            }
        }
    }
}