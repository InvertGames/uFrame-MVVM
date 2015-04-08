using System;
using System.Linq;
using Invert.Common;
using Invert.Common.UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(uFrameMVVMKernel), true)]
public class UFrameMvvmKernelInspector : ManagerInspector<SceneManager>
{
    private bool _RenderSettingsOpen;


    public void Warning(string message)
    {

        EditorGUILayout.HelpBox(message, MessageType.Warning);
    }
    public override void OnInspectorGUI()
    {
        GUIHelpers.IsInsepctor = true;
        //base.OnInspectorGUI();
        DrawTitleBar("UFrame MVVM Kernel");
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
            if (GUIHelpers.DoToolbarEx("Services"))
            {
                foreach (var instance in uFrameMVVMKernel.Instance.Services)
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

            if (GUIHelpers.DoToolbarEx("Systems"))
            {
                foreach (var instance in uFrameMVVMKernel.Instance.SystemLoaders)
                {
                    if (GUIHelpers.DoTriggerButton(new UFStyle()
                    {
                        BackgroundStyle = ElementDesignerStyles.EventButtonStyleSmall,
                        Label = string.Format("{0}", instance.GetType().Name.Replace("Loader",""))
                    }))
                    {
                        Selection.activeGameObject = (instance as MonoBehaviour).gameObject;
                    }
                }
            }

            if (GUIHelpers.DoToolbarEx("Scene Loaders"))
            {
                if (GUIHelpers.DoTriggerButton(new UFStyle()
                {
                    BackgroundStyle = ElementDesignerStyles.EventButtonStyleSmall,
                    Label = string.Format("{0}", "DefaultSceneLoader")
                }))
                {

                }

                //foreach (var instance in uFrameMVVMKernel.Instance.SceneLoaders)
                //{
                //    if (GUIHelpers.DoTriggerButton(new UFStyle()
                //    {
                //        BackgroundStyle = ElementDesignerStyles.EventButtonStyleSmall,
                //        Label = string.Format("{0}", instance.GetType().Name)
                //    }))
                //    {
                //        Selection.activeGameObject = (instance as MonoBehaviour).gameObject;
                //    }
                //}
            }


            if (GUIHelpers.DoToolbarEx("Dependency Container - Instances", defOn: false))
            {
                foreach (var instance in uFrameMVVMKernel.Container.Instances)
                {
                    if (GUIHelpers.DoTriggerButton(new UFStyle()
                    {
                        Label =
                            string.Format("'{0}': {1}->{2}", instance.Name, instance.Base.Name,
                                instance.Instance.GetType().Name),
                        BackgroundStyle = ElementDesignerStyles.EventButtonStyleSmall
                    }))
                    {
                        Debug.Log(instance.Instance);
                    }



                }
            }

            if (GUIHelpers.DoToolbarEx("Dependency Container - Mappings",defOn:false))
            {
                foreach (var instance in uFrameMVVMKernel.Container.Mappings)
                {
                    if (GUIHelpers.DoTriggerButton(new UFStyle()
                    {
                        BackgroundStyle = ElementDesignerStyles.EventButtonStyleSmall,
                        Label = string.Format("{0}: {1}->{2}", instance.Name, instance.From.Name, instance.To.Name)
                    }))
                    {
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