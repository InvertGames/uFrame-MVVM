using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.uFrameComplete.uFrame.Editor.DiagramPlugins
{
    //public class PlayMakerPlugin : DiagramPlugin
    //{
    //    public void Bla()
    //    {

    //    }

    //    public override PluginDrawer GetDrawer(ElementsDiagram diagram, PluginData data)
    //    {
    //        return new PlayMakerDiagramDrawer(data, diagram);
    //    }

    //    public override IEnumerable<IDiagramSubItem> GetItems(PluginData data)
    //    {
    //        yield break;
    //    }

    //    public override void OnAddContextItems(ElementsDiagram diagram, GenericMenu menu)
    //    {
    //        if (diagram.Data.CurrentFilter is ElementData)
    //        {
    //            menu.AddItem(new GUIContent("New Playmaker Template"), false, () =>
    //            {
    //                diagram.AddNewPluginItem(this, "NewPlaymakerTemplate");
    //                EditorWindow.GetWindow<ToolWindow>().Show(true);
    //            });
    //        }
    //    }
    //}

    //public class PlayMakerDiagramDrawer : PluginDrawer
    //{
    //    public PlayMakerDiagramDrawer(PluginData data, ElementsDiagram diagram)
    //        : base(data, diagram)
    //    {
    //    }
    //}

    public class UBehavioursViewDrawer : ViewDrawer
    {
        public UBehavioursViewDrawer(ViewData data, ElementsDiagram diagram)
            : base(data, diagram)
        {

        }

        private DiagramItemHeader _behavioursHeader;

        public DiagramItemHeader BehavioursHeader
        {
            get
            {
                if (_behavioursHeader != null)
                    return _behavioursHeader;


                _behavioursHeader =
                    new DiagramItemHeader() { HeaderType = typeof(UBSharedBehaviour), Label = "Behaviours" };
                _behavioursHeader.OnAddItem += BehavioursHeaderOnOnAddItem;
                return _behavioursHeader;
            }
            set { _behavioursHeader = value; }
        }

        private void BehavioursHeaderOnOnAddItem()
        {
            var asset = UBAssetManager.CreateAsset<UFrameBehaviours>(Diagram.Repository.AssetPath, Data.Name + "Behaviour");
            asset.ViewModelTypeString = Data.ViewForElement.ViewModelAssemblyQualifiedName;
            Diagram.Refresh();
        }

        protected override IEnumerable<DiagramSubItemGroup> GetItemGroups()
        {

            var vm = Data.ViewForElement;
            if (vm == null) yield break;
            if (Data != null)
            {

                Behaviours = UBAssetManager.Behaviours.OfType<UFrameBehaviours>()
                    .Where(p => p != null && p.ViewModelTypeString == vm.ViewModelAssemblyQualifiedName).ToArray();

            }
            yield return new DiagramSubItemGroup()
            {
                Header = BehavioursHeader,
                Items = Behaviours.Select(p=>new BehaviourSubItem() { Behaviour = p}).Cast<IDiagramSubItem>().ToArray()
            };
            yield break;
        }

        public UFrameBehaviours[] Behaviours { get; set; }
    }
}
