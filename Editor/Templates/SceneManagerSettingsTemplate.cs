using System;
using Invert.Core.GraphDesigner;
//using uFrame.Graphs;
using System.Collections;
using Invert.uFrame.MVVM;
using uFrame.Graphs;

[TemplateClass(MemberGeneratorLocation.Both, uFrameFormats.SCENE_MANAGER_SETTINGS_FORMAT)]
public sealed class SceneManagerSettingsTemplate : IClassTemplate<SceneManagerNode>
{
    
    //public string[] _Scenes;
    public string OutputPath
    {
        get { return Path2.Combine(Ctx.Data.Graph.Name, "SceneManagers"); }
    }

    public bool CanGenerate
    {
        get { return true; }
    }

    public void TemplateSetup()
    {
        this.Ctx.AddAttribute(typeof (SerializableAttribute));
        if (Ctx.IsDesignerFile)
        Ctx.CurrentDecleration._public_(typeof (string[]), "_Scenes");
    }

    public TemplateContext<SceneManagerNode> Ctx { get; set; }
}