//using System;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(UBInstanceBehaviour))]
//public class InstanceEditor : Editor
//{
//    public int instanceId;

//    public GameObject editorInstance;
//    public UBehaviours editorUBehavioursInstance;

//    public UBEventContainerEditor prefabEditor;

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        serializedObject.Update();
//        var prefab = serializedObject.FindProperty("_Prefab");
//        if (prefab.objectReferenceValue != null)
//        {
//            EnsureEditorInstance(prefab);

//            //editorInstance.hideFlags= HideFlags.HideAndDontSave;
//            if (prefabEditor == null)
//                prefabEditor = CreateEditor(editorUBehavioursInstance) as UBEventContainerEditor;

//            prefabEditor.OnInspectorGUI();
//            // Debug.Log(PrefabUtility.GetPropertyModifications(editorInstance).Length);
//            //Debug.Log(PrefabUtility.GetPrefabType(PrefabUtility.GetPrefabParent(editorInstance)));
//            // if (PrefabUtility.GetPrefabType(editorInstance) == PrefabType.DisconnectedPrefabInstance)
//            if (GUILayout.Button("Save"))
//            {
//                prefab.objectReferenceValue = PrefabUtility.ReplacePrefab(editorInstance, PrefabUtility.GetPrefabParent(editorUBehavioursInstance), ReplacePrefabOptions.Default);

//                DestroyImmediate(editorInstance.gameObject);
//                EnsureEditorInstance(prefab);
//                prefabEditor = null;
//            }

//            PrefabUtility.ReconnectToLastPrefab(prefab.objectReferenceValue as GameObject);
//            //if (GUILayout.Button("Save Changes"))
//            //{
//            //}
//            //
//            serializedObject.ApplyModifiedProperties();
//        }
//    }

//    public void OnDestroy()
//    {
//        // DestroyImmediate(editorInstance.gameObject);
//    }

//    private void EnsureEditorInstance(SerializedProperty prefab)
//    {
//        if (editorInstance == null)
//        {
//            if (instanceId != 0)
//            {
//                editorInstance = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
//            }
//            else
//            {
//                var obj = PrefabUtility.InstantiatePrefab(prefab.objectReferenceValue);
//                instanceId = obj.GetInstanceID();
//                editorInstance = obj as GameObject;
//                editorInstance.transform.parent = (target as Component).transform;
//            }
//            AssetDatabase.CreateAsset();
//            AssetDatabase.LoadAssetAtPath()
//            //editorInstance.hideFlags = HideFlags.HideAndDontSave;
//            editorUBehavioursInstance = editorInstance.GetComponent<UBehaviours>();
//        }
//    }
//}