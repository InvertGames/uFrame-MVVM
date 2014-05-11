namespace Invert.uFrame.Editor.Refactoring
{
    public class RenameElementRefactorer : RenameRefactorer
    {
        public RenameIdentifierRefactorer ViewBase { get; set; }
        public RenameIdentifierRefactorer ControllerBase { get; set; }
        public RenameIdentifierRefactorer Controller { get; set; }
        public RenameIdentifierRefactorer ViewModel { get; set; }
        public RenameIdentifierRefactorer Initialize { get; set; }
        public RenameIdentifierRefactorer Variable { get; set; }

        public RenameElementRefactorer(ElementData data)
        {
   
            ViewBase = new RenameIdentifierRefactorer();
            ControllerBase = new RenameIdentifierRefactorer();
            Controller = new RenameIdentifierRefactorer();
            ViewModel = new RenameIdentifierRefactorer();
            Initialize = new RenameIdentifierRefactorer();
            Variable = new RenameIdentifierRefactorer();
            ViewBase.From = data.NameAsViewBase;
            ControllerBase.From = data.NameAsControllerBase;
            Controller.From = data.NameAsController;
            ViewModel.From = data.NameAsViewModel;
            Initialize.From = "Initialize" + data.Name;
            Variable.From = data.NameAsVariable;
        }
        public override void Set(ISelectable data)
        {
            Set((ElementData)data);
        }
        public void Set(ElementData data)
        {
            ViewBase.To = data.NameAsViewBase;
            ControllerBase.To = data.NameAsControllerBase;
            Controller.To = data.NameAsController;
            ViewModel.To = data.NameAsViewModel;
            Initialize.To = "Initialize" + data.Name;
            Variable.To = data.NameAsVariable;
        }
        public override void Process(RefactorContext context)
        {
            ViewBase.Process(context);
            ControllerBase.Process(context);
            Controller.Process(context);
            ViewModel.Process(context);
            Initialize.Process(context);
            Variable.Process(context);
        }

        public override void PostProcess(RefactorContext context)
        {
            ViewBase.PostProcess(context);
            ControllerBase.PostProcess(context);
            Controller.PostProcess(context);
            ViewModel.PostProcess(context);
            Initialize.PostProcess(context);
            Variable.PostProcess(context);
        }
    }
}