using System;
[Obsolete]
public interface ISwitchLevelSettings
{
    string[] Levels { get; set; }

    Action<LevelLoadProgress> ProgressUpdated { get; set; }

    Type StartManagerType { get; }

    void InvokeControllerSetup(SceneManager sceneManager);
}