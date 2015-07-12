using System;
using System.Linq;
using Invert.Common;
using Invert.Common.UI;
using uFrame.Kernel;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(uFrameKernel), true)]
public class UFrameMvvmKernelInspector : ManagerInspector<uFrameKernel>
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
            return s.path.EndsWith("KernelScene.unity");
        }))
        {
            Warning("Please add this scene to the build settings!");
        }

        if (Application.isPlaying)
        {

            if (!uFrameKernel.IsKernelLoaded)
            {
                Warning("Kernel is not loaded!");                
            }
            if (uFrameKernel.Instance == null) return;

            if (GUIHelpers.DoToolbarEx("Services"))
            {
                foreach (var instance in uFrameKernel.Instance.Services)
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

                foreach (var instance in uFrameKernel.Instance.SystemLoaders)
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
                foreach (var instance in uFrameKernel.Container.Instances)
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
                foreach (var instance in uFrameKernel.Container.Mappings)
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
[CustomEditor(typeof(Scene), true)]
public class SceneInspector :Editor
{

    public Scene Target
    {
        get { return target as Scene; }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!string.IsNullOrEmpty(Target.DefaultKernelScene))
        {
            EditorGUILayout.HelpBox(
                string.Format("Leave the 'Kernel Scene' property blank to use the default '{0}'",
                    Target.DefaultKernelScene), MessageType.Info);
        }
      
        
    }


}