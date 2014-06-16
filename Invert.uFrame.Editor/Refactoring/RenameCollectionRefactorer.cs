namespace Invert.uFrame.Editor.Refactoring
{
    public class RenameCollectionRefactorer : RenameRefactorer
    {
        public RenameIdentifierRefactorer Name { get; set; }

        public RenameIdentifierRefactorer NameAsAddHandler { get; set; }

        public RenameIdentifierRefactorer NameAsBindingOption { get; set; }

        public RenameIdentifierRefactorer NameAsContainerBindingOption { get; set; }

        public RenameIdentifierRefactorer NameAsCreateHandler { get; set; }

        public RenameIdentifierRefactorer NameAsListBindingOption { get; set; }

        public RenameIdentifierRefactorer NameAsSceneFirstBindingOption { get; set; }

        public RenameIdentifierRefactorer NameAsUseArrayBindingOption { get; set; }

        public RenameCollectionRefactorer(ViewModelCollectionData data)
        {
            Name = new RenameIdentifierRefactorer() { From = data.Name };
            NameAsAddHandler = new RenameIdentifierRefactorer() { From = data.NameAsAddHandler };
            NameAsBindingOption = new RenameIdentifierRefactorer() { From = data.NameAsBindingOption };
            NameAsContainerBindingOption = new RenameIdentifierRefactorer() { From = data.NameAsContainerBindingOption };
            NameAsCreateHandler = new RenameIdentifierRefactorer() { From = data.NameAsCreateHandler };
            NameAsListBindingOption = new RenameIdentifierRefactorer() { From = data.NameAsListBindingOption };
            NameAsSceneFirstBindingOption = new RenameIdentifierRefactorer() { From = data.NameAsSceneFirstBindingOption };
            NameAsUseArrayBindingOption = new RenameIdentifierRefactorer() { From = data.NameAsUseArrayBindingOption };
        }

        public override void PostProcess(RefactorContext context)
        {
            Name.PostProcess(context);
            NameAsAddHandler.PostProcess(context);
            NameAsBindingOption.PostProcess(context);
            NameAsContainerBindingOption.PostProcess(context);
            NameAsCreateHandler.PostProcess(context);
            NameAsListBindingOption.PostProcess(context);
            NameAsSceneFirstBindingOption.PostProcess(context);
            NameAsUseArrayBindingOption.PostProcess(context);
            
          
        }

        public override void PreProcess(RefactorContext refactorContext)
        {
            
        }

        public override void Process(RefactorContext context)
        {
            Name.Process(context);
            NameAsAddHandler.Process(context);
            NameAsBindingOption.Process(context);
            NameAsContainerBindingOption.Process(context);
            NameAsCreateHandler.Process(context);
            NameAsListBindingOption.Process(context);
            NameAsSceneFirstBindingOption.Process(context);
            NameAsUseArrayBindingOption.Process(context);
            
          
        }

        public override void Set(ISelectable data)
        {
            Set((ViewModelCollectionData)data);
        }

        public void Set(ViewModelCollectionData data)
        {
            Name.To = data.Name;
            NameAsAddHandler.To = data.NameAsAddHandler;
            NameAsBindingOption.To = data.NameAsBindingOption;
            NameAsContainerBindingOption.To = data.NameAsContainerBindingOption;
            NameAsCreateHandler.To = data.NameAsCreateHandler;
            NameAsListBindingOption.To = data.NameAsListBindingOption;
            NameAsSceneFirstBindingOption.To = data.NameAsSceneFirstBindingOption;
            NameAsUseArrayBindingOption.To = data.NameAsUseArrayBindingOption;
          
        }
    }
}