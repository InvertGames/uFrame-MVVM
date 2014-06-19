namespace Invert.uFrame.Editor
{
    public interface IDiagramPlugin
    {
        decimal LoadPriority { get; }
        void Initialize(uFrameContainer container);
    }
}