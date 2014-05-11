using UnityEditor;

[CustomEditor(typeof(MouseEventBinding), true)]
public class ComponentMouseButtonEventBindingEditor : ComponentCommandBindingEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (!ViewModelInspector()) return;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_EventType"));

        serializedObject.ApplyModifiedProperties();
    }
}