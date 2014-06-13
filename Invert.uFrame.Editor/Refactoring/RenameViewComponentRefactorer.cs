namespace Invert.uFrame.Editor.Refactoring
{
    public class RenameViewComponentRefactorer : RenameRefactorer
    {
        public RenameFileRefactorer SceneManagerFileRenamer { get; set; }
        public RenameIdentifierRefactorer Name { get; set; }
        public RenameViewComponentRefactorer(ViewComponentData data)
        {
            Name = new RenameIdentifierRefactorer() { From = data.Name };
            SceneManagerFileRenamer = new RenameFileRefactorer();
            SceneManagerFileRenamer.RootPath = data.Data.Settings.CodePathStrategy.AssetPath;
            SceneManagerFileRenamer.From =
                data.Data.Settings.CodePathStrategy.GetEditableViewComponentFilename(data.Name);
        }
        public override void Set(ISelectable data)
        {
            Set((ViewComponentData)data);
        }
        public void Set(ViewComponentData data)
        {
            Name.To = data.Name;
            SceneManagerFileRenamer.To =
             data.Data.Settings.CodePathStrategy.GetEditableViewComponentFilename(data.Name);
        }
        public override void Process(RefactorContext context)
        {
            SceneManagerFileRenamer.Process(context);
            Name.Process(context);
           
        }
        public override void PostProcess(RefactorContext context)
        {
            Name.PostProcess(context);
           
        }
    }
}