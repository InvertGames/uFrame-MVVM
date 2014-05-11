using UnityEditor;

[CustomEditor(typeof(ViewModelCollectionBinding),true)]
public class ComponentCollectionBindingEditor : ComponentBindingEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (!ViewModelInspector()) return;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_Parent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_ViewName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_Immediate"));

        serializedObject.ApplyModifiedProperties();
    }
}