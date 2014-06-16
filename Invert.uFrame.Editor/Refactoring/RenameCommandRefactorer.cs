namespace Invert.uFrame.Editor.Refactoring
{
    public class RenameCommandRefactorer : RenameRefactorer
    {
        public RenameIdentifierRefactorer Name { get; set; }
        public RenameIdentifierRefactorer NameAsExecuteMethod { get; set; }
        public RenameCommandRefactorer(ViewModelCommandData data)
        {
            Name = new RenameIdentifierRefactorer() { From = data.Name };
            NameAsExecuteMethod = new RenameIdentifierRefactorer() { From = data.NameAsExecuteMethod };
            
        }
        public override void Set(ISelectable data)
        {
            Set((ViewModelCommandData)data);
        }
        public void Set(ViewModelCommandData data)
        {
            Name.To = data.Name;
            NameAsExecuteMethod.To = data.NameAsExecuteMethod;
        }
        public override void Process(RefactorContext context)
        {
            Name.Process(context);
            NameAsExecuteMethod.Process(context);

        }
        public override void PostProcess(RefactorContext context)
        {
            Name.PostProcess(context);
            NameAsExecuteMethod.PostProcess(context);

        }

        public override void PreProcess(RefactorContext refactorContext)
        {
            
        }
    }
}