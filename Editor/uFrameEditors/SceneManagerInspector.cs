using System.IO;
using System.Text;
using Invert.Common;
using Invert.Common.UI;
using UnityEditor;
using UnityEngine;

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
public class TextAssetStorage : ISerializerStorage
{
    public string AssetPath { get; set; }

    public TextAsset Asset
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath(AssetPath, typeof(TextAsset)) as TextAsset;
        }
    }
    public void Save(ISerializerStream stream)
    {
        File.WriteAllText(AssetPath, Encoding.UTF8.GetString(stream.Save()));
        AssetDatabase.Refresh();
    }

    public void Load(ISerializerStream stream)
    {
        stream.Load(Asset.bytes);
    }
}