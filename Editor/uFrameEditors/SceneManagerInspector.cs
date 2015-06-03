using System;
using System.IO;
using System.Text;
using Invert.Common;
using Invert.Common.UI;
using uFrame.Serialization;
using UnityEditor;
using UnityEngine;
[Obsolete]
[CustomEditor(typeof(SceneManager), true)]
public class SceneManagerInspector : uFrameInspector
{
    protected override void OnHeaderGUI()
    {
        base.OnHeaderGUI();

    }

    public override void OnInspectorGUI()
    {
        GUIHelpers.IsInsepctor = true;
        DrawTitleBar("Scene Manager");

        base.OnInspectorGUI();
        if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
        {

            //if (GUI.Button(GUIHelpers.GetRect(ElementDesignerStyles.ButtonStyle), "Serialize To String", ElementDesignerStyles.ButtonStyle))
            //{
            //    var sm = (target as SceneManager);

            //    sm.SaveState(new TextAssetStorage() { AssetPath = "Assets/TestData.txt" }, new JsonStream() { UseReferences = true });

            //}
            //if (GUI.Button(GUIHelpers.GetRect(ElementDesignerStyles.ButtonStyle), "Load From String", ElementDesignerStyles.ButtonStyle))
            //{
            //    var sm = (target as SceneManager);
            //    sm.LoadState(new TextAssetStorage() { AssetPath = "Assets/TestData.txt" }, new JsonStream() { UseReferences = true });

            //}
            if (GUIHelpers.DoToolbarEx("Persistable Views"))
            {
                foreach (var viewBase in GameManager.ActiveSceneManager.PersistantViews)
                {
                    if (viewBase == null) continue;
                    if (GUIHelpers.DoTriggerButton(new UFStyle(viewBase.Identifier + ": " + viewBase.name,
                        ElementDesignerStyles.EventButtonStyleSmall, null, null, null, false,
                        TextAnchor.MiddleCenter) { }))
                    {
                        //var fileStorage = new TextAssetStorage();
                        var stringStorage = new StringSerializerStorage();
                        var stream = new JsonStream();
                        viewBase.Write(stream);
                        stringStorage.Save(stream);
                        Debug.Log(stringStorage);
                    }
                }
               


            }
            if (GUIHelpers.DoToolbarEx("Persistable View-Models"))
            {
                foreach (var viewBase in GameManager.ActiveSceneManager.PersistantViewModels)
                {
                    if (viewBase == null) continue;
                    if (GUIHelpers.DoTriggerButton(new UFStyle(viewBase.Identifier + ": " + viewBase.GetType().Name,
                        ElementDesignerStyles.EventButtonStyleSmall, null, null, null, false,
                        TextAnchor.MiddleCenter) { }))
                    {
                        //var fileStorage = new TextAssetStorage();
                        var stringStorage = new StringSerializerStorage();
                        var stream = new JsonStream();
                        viewBase.Write(stream);
                        stringStorage.Save(stream);
                        Debug.Log(stringStorage);
                    }
                }
            }
        }

        GUIHelpers.IsInsepctor = false;
    }
}
