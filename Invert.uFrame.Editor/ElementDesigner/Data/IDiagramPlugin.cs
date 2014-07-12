namespace Invert.uFrame.Editor
{
    public interface IDiagramPlugin
    {
        string Title { get; }
        bool Enabled { get; set; }
        decimal LoadPriority { get; }
        string PackageName { get; }
        void Initialize(uFrameContainer container);
    }
}