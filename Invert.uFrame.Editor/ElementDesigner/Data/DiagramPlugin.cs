using Invert.uFrame;
namespace Invert.uFrame.Editor
{
    public abstract class DiagramPlugin : IDiagramPlugin
    {
        public virtual decimal LoadPriority { get { return 1; } }
        public abstract void Initialize(uFrameContainer container);
    }
}