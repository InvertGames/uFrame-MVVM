using Invert.Common;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UBSharedBehaviour))]
public class TBehavioursEditor : TBehavioursEditorBase
{
    private int _renameTemplateIndex;

    public bool InstanceTemplatesOpen
    {
        get { return EditorPrefs.GetBool("UBInstanceTemplatesOpen", true); }
        set { EditorPrefs.SetBool("UBInstanceTemplatesOpen", value); }
    }

    public override bool CanAllowOverrides
    {
        get { return true; }
    }

    public override void OnInspectorGUI()
    {
        if (EditorApplication.isPlaying || EditorApplication.isPaused)
        {
            var guiStyle = new GUIStyle(UBStyles.CurrentActionBackgroundStyle);
            guiStyle.normal.textColor = Color.black;
            guiStyle.fixedHeight = 25;
            guiStyle.padding = new RectOffset(5, 0, 5, 0);
            var rect = GetRect(guiStyle);
            GUI.Box(rect, "Live Editing: Changes will be saved!", guiStyle);
        }
        base.OnInspectorGUI();
    }

    protected override void DoAdditionalTriggerLists()
    {
        base.DoAdditionalTriggerLists();
        //var triggerTemplates = serializedObject.FindProperty("_triggerTemplates");
        //DoInstanceTemplates(triggerTemplates);
    }

    private void DoInstanceTemplates(SerializedProperty instanceTemplatesArray)
    {
        if (DoToolbar("Events", InstanceTemplatesOpen, () => UBInputDialog.Init("Instance Template Name",
            (templateName) =>
            {
                instanceTemplatesArray.serializedObject.Update();
                var item = instanceTemplatesArray.GetArrayElementAtIndex(instanceTemplatesArray.arraySize++);
                item.stringValue = templateName;
                instanceTemplatesArray.serializedObject.ApplyModifiedProperties();
            },
            null,
            v=>UBUtils.ValidateInstanceTemplate(instanceTemplatesArray,v)
            )))
        {
            InstanceTemplatesOpen = !InstanceTemplatesOpen;
        }
        if (!InstanceTemplatesOpen) return;
        for (var i = 0; i < instanceTemplatesArray.arraySize; i++)
        {
            var template = instanceTemplatesArray.GetArrayElementAtIndex(i);
            int i1 = i;

            var actionContent = new UBActionContent()
            {
                BackgroundStyle = UBStyles.IncludeTriggerBackgroundStyle,
                DisplayText = template.stringValue,
                OptionsStyle = UBStyles.FoldoutOpenButtonStyle,
                OnShowOptions = (c) =>
                {
                    var contextMenu = new GenericMenu();
                    contextMenu.AddItem(new GUIContent("Rename"), false, () =>
                    {
                        UBInputDialog.Init("Rename Event", (newValue) =>
                        {
                            template.serializedObject.Update();
                            template.stringValue = newValue;
                            template.serializedObject.ApplyModifiedProperties();
                        }, template.stringValue);
                    });
                    contextMenu.ShowAsContext();
                },
                OnRemove = () =>
                {
                    instanceTemplatesArray.DeleteArrayElementAtIndex(i1);
                }
            };

            DoActionButton(actionContent);
        }
    }
}