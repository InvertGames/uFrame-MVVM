namespace Invert.uFrame.Editor
{
    public interface IDiagramPlugin
    {
        string Title { get; }
        bool Enabled { get; set; }
        decimal LoadPriority { get; }
        void Initialize(uFrameContainer container);
    }
}