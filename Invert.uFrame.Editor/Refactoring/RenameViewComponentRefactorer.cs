namespace Invert.uFrame.Editor.Refactoring
{
    public class RenameViewComponentRefactorer : RenameRefactorer
    {
        public RenameFileRefactorer ViewComponentFileRenamer { get; set; }
        public RenameIdentifierRefactorer Name { get; set; }
        public RenameViewComponentRefactorer(ViewComponentData data)
        {
            Name = new RenameIdentifierRefactorer() { From = data.Name };
            ViewComponentFileRenamer = new RenameFileRefactorer();
            ViewComponentFileRenamer.RootPath = data.Data.Settings.CodePathStrategy.AssetPath;
            ViewComponentFileRenamer.From =
                data.Data.Settings.CodePathStrategy.GetEditableViewComponentFilename(data.Name);
        }
        public override void Set(ISelectable data)
        {
            Set((ViewComponentData)data);
        }
        public void Set(ViewComponentData data)
        {
            Name.To = data.Name;
            ViewComponentFileRenamer.To =
             data.Data.Settings.CodePathStrategy.GetEditableViewComponentFilename(data.Name);
        }
        public override void PreProcess(RefactorContext context)
        {
            ViewComponentFileRenamer.Process(context);
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