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
        DrawTitleBar("Scene Manager");
        base.OnInspectorGUI();
        if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
        {

            if (GUIHelpers.DoToolbarEx("Instances"))
            {
                foreach (var instance in GameManager.ActiveSceneManager.Context.ViewModels)
                {
                    if (GUIHelpers.DoTriggerButton(new UFStyle(instance.Value.GetHashCode() + ": " + instance.Key,
                        UBStyles.EventButtonLargeStyle, null, UBStyles.RemoveButtonStyle, null, false,
                        TextAnchor.MiddleCenter) { SubLabel = instance.Value.GetType().Name }))
                    {
                        Debug.Log(instance.Value.GetHashCode().ToString() + ": " + instance.ToString());
                    }

                }
            }

            if (GUIHelpers.DoToolbarEx("Views"))
            {
                foreach (var viewBase in GameManager.ActiveSceneManager.RootViews)
                {
                    if (GUIHelpers.DoTriggerButton(new UFStyle(viewBase.name,
                        UBStyles.EventButtonLargeStyle, null, UBStyles.RemoveButtonStyle, null, false,
                        TextAnchor.MiddleCenter) { SubLabel = viewBase.Identifier }))
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


            if (GUI.Button(GUIHelpers.GetRect(ElementDesignerStyles.ButtonStyle), "Serialize To String", ElementDesignerStyles.ButtonStyle))
            {
                var sm = (target as SceneManager);

                sm.Save(new TextAssetStorage() { AssetPath = "Assets/TestData.txt" }, new JsonStream() { UseReferences = true });
                
            }
            if (GUI.Button(GUIHelpers.GetRect(ElementDesignerStyles.ButtonStyle), "Load From String", ElementDesignerStyles.ButtonStyle))
            {
                var sm = (target as SceneManager);
                sm.Load(new TextAssetStorage() { AssetPath = "Assets/TestData.txt"}, new JsonStream() {UseReferences = true});
                
            }
        }
        Toggle("GameRoot",true);

    }
}
public class TextAssetStorage : ISerializerStorage
{
    public string AssetPath { get; set; }

    public TextAsset Asset
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath(AssetPath, typeof (TextAsset)) as TextAsset;
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