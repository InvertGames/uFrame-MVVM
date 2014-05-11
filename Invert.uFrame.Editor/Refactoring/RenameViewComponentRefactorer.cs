namespace Invert.uFrame.Editor.Refactoring
{
    public class RenameViewComponentRefactorer : RenameRefactorer
    {
        public RenameIdentifierRefactorer Name { get; set; }
        public RenameViewComponentRefactorer(ViewComponentData data)
        {
            Name = new RenameIdentifierRefactorer() { From = data.Name };
        }
        public override void Set(ISelectable data)
        {
            Set((ViewComponentData)data);
        }
        public void Set(ViewComponentData data)
        {
            Name.To = data.Name;
        }
        public override void Process(RefactorContext context)
        {
            Name.Process(context);
           
        }
        public override void PostProcess(RefactorContext context)
        {
            Name.PostProcess(context);
           
        }
    }
}