using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnityGraphData),true)]
public class GraphDataInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Convert to Text Asset"))
        {
            var t = target as IGraphData;
            var property = serializedObject.FindProperty("_jsonData");
            var jsonData = InvertGraph.Serialize(t).ToString();
            var path = AssetDatabase.GetAssetPath(target);
            File.WriteAllText(path.Replace(".asset",".txt"),jsonData);
            AssetDatabase.Refresh();
        }
    }
}

