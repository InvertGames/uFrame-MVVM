using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using UnityEditor;
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

    //public class UBehavioursViewDrawer : ViewDrawer
    //{
    //    public UBehavioursViewDrawer()
    //    {
    //    }

    //    public UBehavioursViewDrawer(ViewData data, ElementsDiagram diagram)
    //        : base(data, diagram)
    //    {

    //    }

    //    private NodeItemHeader _behavioursHeader;

    //    public NodeItemHeader BehavioursHeader
    //    {
    //        get
    //        {
    //            if (_behavioursHeader != null)
    //                return _behavioursHeader;


    //            _behavioursHeader =
    //                new NodeItemHeader() { HeaderType = typeof(UBSharedBehaviour), Label = "Behaviours" };
    //            _behavioursHeader.AddCommand = Container.Resolve<AddNewBehaviourCommand>();
                
    //            return _behavioursHeader;
    //        }
    //        set { _behavioursHeader = value; }
    //    }
        
    //    protected override IEnumerable<DiagramSubItemGroup> GetItemGroups()
    //    {
    //        var baseItems = base.GetItemGroups().ToArray();
    //        foreach (var baseItem in baseItems)
    //        {
    //            yield return baseItem;
    //        }
    //        var vm = Data.ViewForElement;
    //        if (vm == null) yield break;
    //        if (Data != null)
    //        {

    //            Behaviours = UBAssetManager.Behaviours.OfType<UFrameBehaviours>()
    //                .Where(p => p != null && p.ViewModelTypeString == vm.ViewModelAssemblyQualifiedName).ToArray();

    //        }
    //        yield return new DiagramSubItemGroup()
    //        {
    //            Header = BehavioursHeader,
    //            Items = Behaviours.Select(p=>new BehaviourNodeItem() { Behaviour = p}).Cast<IDiagramNodeItem>().ToArray()
    //        };
    //        yield break;
    //    }

    //    public UFrameBehaviours[] Behaviours { get; set; }
    //}

    //public class AddNewBehaviourCommand : EditorCommand<ViewData>
    //{
    //    public override void Perform(ViewData node)
    //    {
    //        var paths = EditorWindow.GetWindow<ElementsDesigner>().Diagram.Data.Settings.CodePathStrategy;

    //        if (!Directory.Exists(paths.BehavioursPath))
    //        {
    //            Directory.CreateDirectory(paths.BehavioursPath);
    //        }
    //        var asset = UBAssetManager.CreateAsset<UFrameBehaviours>(paths.BehavioursPath, node.Name + "Behaviour");
    //        asset.ViewModelTypeString = node.ViewForElement.ViewModelAssemblyQualifiedName;
            
    //        //Save();
    //    }

    //    public override string CanPerform(ViewData node)
    //    {
    //        return null;
    //    }
    //}
}
