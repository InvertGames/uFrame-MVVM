using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Invert.uFrame.Editor.ElementDesigner.Data
{
    [Serializable]
    public class PluginData : DiagramNode
    {
        [SerializeField]
        private string _pluginTypeName;

        public IDiagramPlugin Plugin { get; set; }

        public override string Label
        {
            get { return Name; }
        }

        public override void CreateLink(IDiagramNode container, IDrawable target)
        {
            
        }

        public override bool CanCreateLink(IDrawable target)
        {
            return false;
        }

        public override IEnumerable<IDiagramLink> GetLinks(IDiagramNode[] nodes)
        {
            yield break;
        }

        public override void RemoveLink(IDiagramNode target)
        {
            
        }

        public override IEnumerable<IDiagramNodeItem> Items
        {
            get { yield break; }
        }

        public Type PluginType
        {
            get { return Type.GetType(_pluginTypeName); }
            set { _pluginTypeName = value.AssemblyQualifiedName; }
        }

        public string PluginTypeName
        {
            get { return _pluginTypeName; }
            set { _pluginTypeName = value; }
        }

        public override void RemoveFromDiagram()
        {
            base.RemoveFromDiagram();
        }
    }

    public class PluginDrawer : DiagramNodeDrawer<PluginData>
    {
        public PluginDrawer()
        {
        }

        public PluginDrawer(PluginData data, ElementsDiagram diagram) : base(data, diagram)
        {
            
        }

        protected override IEnumerable<DiagramSubItemGroup> GetItemGroups()
        {
            yield break;
        }
    }

    public interface IDiagramPlugin
    {
        decimal LoadPriority { get; }
        bool Enabled { get; set; }
        string Title { get; }
        void Initialize(uFrameContainer container);
    }

    public abstract class DiagramPlugin :IDiagramPlugin
    {
        public virtual bool Enabled
        {
            get { return EditorPrefs.GetBool("UFRAME_PLUGIN_" + this.GetType().FullName, true); }
            set {  EditorPrefs.SetBool("UFRAME_PLUGIN_" + this.GetType().FullName, value); }
        }

        public virtual string Title
        {
            get { return this.GetType().Name; }
        }

        public virtual decimal LoadPriority { get { return 1; } }
        public abstract void Initialize(uFrameContainer container);
    }
}
