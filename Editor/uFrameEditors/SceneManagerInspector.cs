using System.IO;
using System.Text;
using Invert.Common;
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
            foreach (var instance in GameManager.ActiveSceneManager.Context.ViewModels)
            {
                if (UBEditor.DoTriggerButton(new UBTriggerContent(instance.Value.GetHashCode() +": " + instance.Key,
                    UBStyles.EventButtonLargeStyle, null, UBStyles.RemoveButtonStyle, null, false,
                    TextAnchor.MiddleCenter) { SubLabel = instance.Value.GetType().Name }))
                {
                    Debug.Log(instance.Value.GetHashCode().ToString() + ": " + instance.ToString());
                }

            }

            if (GUI.Button(UBEditor.GetRect(ElementDesignerStyles.ButtonStyle), "To Json", ElementDesignerStyles.ButtonStyle))
            {
                var fileStorage = new TextAssetStorage();
                var stringStorage = new StringSerializerStorage();
                ((SceneManager)target).Context.Save(fileStorage, new JsonStream() {UseReferences = true});
                Debug.Log(stringStorage);
            }
        }
        
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
        File.WriteAllText(Application.dataPath + "/test.txt", Encoding.UTF8.GetString(stream.Save()));
        AssetDatabase.Refresh();
        //var ast = new TextAsset() {};
        //ast.text = ;s
        //var ta = AssetDatabase.CreateAsset(, ScriptableObject.CreateInstance<TextAsset>());
    }

    public void Load(ISerializerStream stream)
    {
//        return AssetDatabase.LoadAssetAtPath(AssetPath, typeof(TextAsset)) as TextAsset;
    }
}