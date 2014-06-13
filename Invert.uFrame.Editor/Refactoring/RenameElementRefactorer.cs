namespace Invert.uFrame.Editor.Refactoring
{
    public class RenameElementRefactorer : RenameRefactorer
    {
        public RenameFileRefactorer ControllerRenamer { get; set; }
        public RenameFileRefactorer ViewModelRenamer { get; set; }
        public RenameIdentifierRefactorer ViewBase { get; set; }
        public RenameIdentifierRefactorer ControllerBase { get; set; }
        public RenameIdentifierRefactorer Controller { get; set; }
        public RenameIdentifierRefactorer ViewModel { get; set; }
        public RenameIdentifierRefactorer Initialize { get; set; }
        public RenameIdentifierRefactorer Variable { get; set; }

        public RenameElementRefactorer(ElementData data)
        {
            ControllerRenamer = new RenameFileRefactorer();
            ViewModelRenamer = new RenameFileRefactorer();
            ViewBase = new RenameIdentifierRefactorer();
            ControllerBase = new RenameIdentifierRefactorer();
            Controller = new RenameIdentifierRefactorer();
            ViewModel = new RenameIdentifierRefactorer();
            Initialize = new RenameIdentifierRefactorer();
            Variable = new RenameIdentifierRefactorer();
            ViewModelRenamer.RootPath = data.Data.Settings.CodePathStrategy.AssetPath;
            ControllerRenamer.RootPath = data.Data.Settings.CodePathStrategy.AssetPath;
            ViewModelRenamer.From =
                data.Data.Settings.CodePathStrategy.GetEditableViewModelFilename(data.NameAsViewModel);
            ControllerRenamer.From =
                data.Data.Settings.CodePathStrategy.GetEditableControllerFilename(data.NameAsController);
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

            ViewModelRenamer.To =
              data.Data.Settings.CodePathStrategy.GetEditableViewModelFilename(data.NameAsViewModel);
            ControllerRenamer.To =
                data.Data.Settings.CodePathStrategy.GetEditableControllerFilename(data.NameAsController);

            ViewBase.To = data.NameAsViewBase;
            ControllerBase.To = data.NameAsControllerBase;
            Controller.To = data.NameAsController;
            ViewModel.To = data.NameAsViewModel;
            Initialize.To = "Initialize" + data.Name;
            Variable.To = data.NameAsVariable;
        }
        public override void Process(RefactorContext context)
        {
            ViewModelRenamer.Process(context);
            ControllerRenamer.Process(context);
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