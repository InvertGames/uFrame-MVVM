namespace Invert.uFrame.Editor.Refactoring
{
    public class RenameViewRefactorer : RenameRefactorer
    {
        public RenameIdentifierRefactorer View { get; set; }
        public RenameIdentifierRefactorer ViewBase { get; set; }
        public RenameViewRefactorer(ViewData data)
        {
            View = new RenameIdentifierRefactorer() { From = data.NameAsView };
            ViewBase = new RenameIdentifierRefactorer() { From = data.NameAsViewBase };
        }
        public override void Set(ISelectable data)
        {
            Set((ViewData)data);
        }
        public void Set(ViewData data)
        {
            View.To = data.NameAsView;
            ViewBase.To = data.NameAsViewBase;
        }
        public override void Process(RefactorContext context)
        {
            View.Process(context);
            ViewBase.Process(context);
        }
        public override void PostProcess(RefactorContext context)
        {
            View.PostProcess(context);
            ViewBase.PostProcess(context);
        }
    }
}