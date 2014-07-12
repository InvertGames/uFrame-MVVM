using Invert.uFrame;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using UnityEditor;
using UnityEngine;

namespace Invert.uFrame.Editor
{
    public abstract class DiagramPlugin : IDiagramPlugin
    {
        public virtual string PackageName
        {
            get { return string.Empty; }
        }

        public string Title
        {
            get { return this.GetType().Name; }
        }

        public virtual bool EnabledByDefault
        {
            get { return true; }
        }
        public virtual bool Enabled
        {
            get { return EditorPrefs.GetBool("UFRAME_PLUGIN_" + this.GetType().Name, EnabledByDefault); }
            set { EditorPrefs.SetBool("UFRAME_PLUGIN_" + this.GetType().Name, value); }
        }

        public virtual decimal LoadPriority { get { return 1; } }
        public abstract void Initialize(uFrameContainer container);
    }

    public class DefaultKeyBindings : DiagramPlugin
    {
        public override void Initialize(uFrameContainer container)
        {
            container.RegisterInstance<IKeyBinding>(new KeyBinding<IDiagramNodeItemCommand>("Delete",KeyCode.Delete),"DeleteItemCommand");
            container.RegisterInstance<IKeyBinding>(new KeyBinding<IDiagramNodeCommand>("Delete",KeyCode.Delete),"DeleteCommand");
            container.RegisterInstance<IKeyBinding>(new KeyBinding<IDiagramNodeItemCommand>("MoveItemUp", KeyCode.UpArrow, true), "MoveItemUp");
            container.RegisterInstance<IKeyBinding>(new KeyBinding<IDiagramNodeItemCommand>("MoveItemDown", KeyCode.DownArrow, true), "MoveItemDown");
        }
    }
}