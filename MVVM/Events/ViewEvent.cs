using uFrame.Kernel;

namespace uFrame.MVVM
{
    public class ViewEvent
    {
        public bool IsInstantiated { get; set; }
        public IScene Scene { get; set; }
        public ViewBase View { get; set; }
    }
}