namespace Invert.uFrame.Editor.ElementDesigner
{
    public interface IContextMenuItem
    {
        string Path { get; }
        bool IsChecked(object arg);
    }
}