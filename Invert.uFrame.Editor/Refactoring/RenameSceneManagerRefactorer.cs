namespace Invert.uFrame.Editor.Refactoring
{
    public class RenameSceneManagerRefactorer : RenameRefactorer
    {
        public RenameIdentifierRefactorer SceneManager { get; set; }
        public RenameIdentifierRefactorer SceneManagerBase { get; set; }
        public RenameIdentifierRefactorer Settings { get; set; }
        public RenameIdentifierRefactorer SettingsField { get; set; }

        public RenameSceneManagerRefactorer(SceneManagerData data)
        {
         
            SceneManager = new RenameIdentifierRefactorer() {From = data.NameAsSceneManager};
            SceneManagerBase = new RenameIdentifierRefactorer() { From = data.NameAsSceneManagerBase };
            Settings = new RenameIdentifierRefactorer() { From = data.NameAsSettings };
            SettingsField = new RenameIdentifierRefactorer() { From = data.NameAsSettingsField };
            
           
        }
        public override void Set(ISelectable data)
        {
            Set((SceneManagerData)data);
        }
        public void Set(SceneManagerData data)
        {
            SceneManager.To = data.NameAsSceneManager;
            SceneManagerBase.To = data.NameAsSceneManagerBase;
            Settings.To = data.NameAsSettings;
            SettingsField.To = data.NameAsSettingsField;
         
        }
        public override void Process(RefactorContext context)
        {
            SceneManager.Process(context);
            SceneManagerBase.Process(context);
            Settings.Process(context);
            SettingsField.Process(context);
          
        }

        public override void PostProcess(RefactorContext context)
        {
            SceneManager.PostProcess(context);
            SceneManagerBase.PostProcess(context);
            Settings.PostProcess(context);
            SettingsField.PostProcess(context);
        }
    }
}