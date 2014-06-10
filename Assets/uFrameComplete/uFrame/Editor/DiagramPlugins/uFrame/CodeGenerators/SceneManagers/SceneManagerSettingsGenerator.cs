using Invert.uFrame.Editor;

public class SceneManagerSettingsGenerator : SceneManagerClassGenerator
{
    public override void Initialize(CodeFileGenerator fileGenerator)
    {
        base.Initialize(fileGenerator);
        AddSceneManagerSettings(Data,null);
    }
}