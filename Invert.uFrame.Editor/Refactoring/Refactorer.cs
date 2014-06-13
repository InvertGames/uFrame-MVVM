namespace Invert.uFrame.Editor.Refactoring
{
    public abstract class Refactorer
    {
        public virtual int Priority {get { return 0; }}
        public bool Applied { get; set; }
        public abstract void Process(RefactorContext context);

        public abstract void PostProcess(RefactorContext context);
    }
}