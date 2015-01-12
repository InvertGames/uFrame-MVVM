using System;
using Invert.Core.GraphDesigner;
//using uFrame.Graphs;
using System.Collections;
using Invert.uFrame.Editor;
using uFrame.Graphs;

[TemplateClass("SceneManagers", uFrameFormats.SCENE_MANAGER_SETTINGS_FORMAT, MemberGeneratorLocation.Both)]
public sealed class SceneManagerSettingsTemplate : IClassTemplate<SceneManagerNode>
{
    
    //public string[] _Scenes;
    public void TemplateSetup()
    {
        this.Ctx.AddAttribute(typeof (SerializableAttribute));
        if (Ctx.IsDesignerFile)
        Ctx.CurrentDecleration._public_(typeof (string[]), "_Scenes");
    }

    public TemplateContext<SceneManagerNode> Ctx { get; set; }
}