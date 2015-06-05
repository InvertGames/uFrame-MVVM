using System;
using System.Linq;
using Invert.Common;
using Invert.Common.UI;
using uFrame.Kernel;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneManagementService), true)]
public class SceneManagementServiceInspector : ManagerInspector<SceneManagementService>
{
    private bool _RenderSettingsOpen;
    private SceneManagementService _service;


    public void Warning(string message)
    {

        EditorGUILayout.HelpBox(message, MessageType.Warning);
    }

    public SceneManagementService Service
    {
        get { return _service ?? (_service = uFrameKernel.Container.Resolve<SceneManagementService>()); }
    }

    public void OnDisable()
    {
        _service = null;
    }
    public override void OnInspectorGUI()
    {
        GUIHelpers.IsInsepctor = true;
        //
        DrawTitleBar("Scene Management Service");
        base.OnInspectorGUI();
        serializedObject.Update();

        if (Application.isPlaying)
        {
            if (GUIHelpers.DoToolbarEx("Loaded Scenes"))
            {
                if (Service != null)
                {
                    foreach (var instance in Service.LoadedScenes)
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