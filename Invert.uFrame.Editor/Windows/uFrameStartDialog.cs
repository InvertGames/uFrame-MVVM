using Invert.Common;
using UnityEditor;
using UnityEngine;

public class uFrameStartDialog : EditorWindow
{
    private TextAsset _ChangeLog;

    private Vector2 _ChangeLogScrollPosition;

    private Rect _MainAreaRect = new Rect(4, 48, 512, 345);

    private bool _ViewingReadme;

    [MenuItem("Tools/[u]Frame/Change Log")]
    internal static void ShowWindow()
    {
        var window = GetWindow<uFrameStartDialog>();
        window.title = "uFrame";
        window.minSize = window.maxSize = new Vector2(520, 400);
        window._ChangeLog = Resources.Load("uFrameReadme", typeof(TextAsset)) as TextAsset;
        window.ShowUtility();
    }

    private void OnEnable()
    {
        minSize = new Vector2(520, 400);
        maxSize = new Vector2(520, 400);
        position = new Rect(position.x, position.y, 520, 400);
    }

    public static void DrawTitleBar(string subTitle)
    {
        //GUI.Label();
        ElementDesignerStyles.DoTilebar(subTitle);
    }

    public void OnGUI()
    {
        try
        {
            if (_ChangeLog == null)
            {
                EditorGUILayout.HelpBox("This installation appears to be broken. Cannot find the 'Change Log.txt' resource", MessageType.Error);
                return;
            }

            DrawTitleBar("uFrame Change Log");

            GUILayout.BeginArea(_MainAreaRect, GUI.skin.box);

            _ChangeLogScrollPosition = GUILayout.BeginScrollView(_ChangeLogScrollPosition, false, false, GUILayout.Width(502), GUILayout.Height(292));

            GUILayout.Label(_ChangeLog.text, EditorStyles.wordWrappedLabel);

            GUILayout.EndScrollView();

            GUILayout.BeginVertical();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Done", GUILayout.Height(22)))
                this.Close();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.EndArea();
        }
        catch { }
    }
}