using Invert.uFrame;
using Invert.uFrame.Editor;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Data;
using UnityEngine;

public class UFrameEditorPlugin : DiagramPlugin
{
    public override void Initialize(uFrameContainer container)
    {
#if DEBUG
        Debug.Log("Registering " + "UFrameEditorPlugin");
#endif
        container.Register<DiagramItemGenerator,ElementDataGenerator>("ElementData");
        container.Register<DiagramItemGenerator,EnumDataGenerator>("EnumData");
        container.Register<DiagramItemGenerator,ViewDataGenerator>("ViewData");
        container.Register<DiagramItemGenerator,SceneManagerDataGenerator>("SceneManagerData");

        container.RegisterInstance<IToolbarCommand>(new ImportCommand(),"Import");
    }
}