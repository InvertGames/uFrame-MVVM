using UnityEditor;

[CustomEditor(typeof(UBGlobals))]
public class TGlobalsEditor : TBehavioursEditorBase
{
    public bool IsGlobalsOpen(string key)
    {
        return EditorPrefs.GetBool("UBIsGlobalsOpen" + name, true);

    }
    public void SetGlobalsOpen(string key, bool value)
    {
        EditorPrefs.SetBool("UBIsGlobalsOpen" + name, value);
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();
        if (DoToolbar(IsGlobals ? target.name : "Variables", IsGlobalsOpen(target.name), OnAddVariable)) SetGlobalsOpen(target.name,!IsGlobalsOpen(target.name));
        if (IsGlobalsOpen(target.name))
        DoVariablesEditor(serializedObject.FindProperty("_declares"),true,false);

        serializedObject.ApplyModifiedProperties();
    }

    //protected override void OnShowTriggerWindow()
    //{
    //    //base.OnShowTriggerWindow();
    //    var triggersProperty = serializedObject.FindProperty("_triggers");
    //    UBInputDialog.Init("Add Global Trigger", (name) =>
    //    {
    //        AddTrigger(triggersProperty, name, typeof(UBCustomTrigger).AssemblyQualifiedName);
    //    });
    //}
}