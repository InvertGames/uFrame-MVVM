using System;
using System.Linq;
using Invert.Common;
using Invert.Common.UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ViewService), true)]
public class ViewServiceInspector : ManagerInspector<ViewService>
{
    private bool _RenderSettingsOpen;
    private ViewService _viewService;


    public void Warning(string message)
    {

        EditorGUILayout.HelpBox(message, MessageType.Warning);
    }

    public ViewService ViewService
    {
        get { return _viewService ?? (_viewService = uFrameMVVMKernel.Container.Resolve<ViewService>()); }
    }

    public void OnDisable()
    {
        _viewService = null;
    }
    public override void OnInspectorGUI()
    {
        GUIHelpers.IsInsepctor = true;
        //base.OnInspectorGUI();
        DrawTitleBar("View Service");
        serializedObject.Update();

        if (!UnityEditor.EditorBuildSettings.scenes.Any(s =>
        {
            return s.path.EndsWith("uFrameMVVMKernelScene.unity");
        }))
        {
            Warning("Please add this scene to the build settings!");
        }

        if (Application.isPlaying)
        {
            if (GUIHelpers.DoToolbarEx("Views"))
            {
                foreach (var instance in ViewService.Views)
                {
                    if (GUIHelpers.DoTriggerButton(new UFStyle()
                    {
                        BackgroundStyle = ElementDesignerStyles.EventButtonStyleSmall,
                        Label = string.Format("{0}", instance.GetType().Name)
                    }))
                    {
                        Selection.activeGameObject = (instance as MonoBehaviour).gameObject;
                    }
                }
            }
        }
        else
        {
        }

        if (serializedObject.ApplyModifiedProperties())
        {
            //var t = Target as GameManager;
            //t.ApplyRenderSettings();
        }
        GUIHelpers.IsInsepctor = false;
    }

    protected override bool ExistsInScene(Type itemType)
    {
        return FindObjectOfType(itemType) != null;
    }

    protected override string GetTypeNameFromName(string name)
    {
        return name + "Game";
    }

}