using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(UBInstanceBehaviour), true)]
//class TBehavioursInstanceEditor : TBehavioursEditorBase
//{
 
//    public UBInstanceBehaviour InstanceTarget
//    {
//        get { return target as UBInstanceBehaviour; }
//    }

//    public override bool CanAllowOverrides
//    {
//        get { return false; }
//    }

//    public override void OnInspectorGUI()
//    {
       
//        if (EditorApplication.isPlaying || EditorApplication.isPaused)
//        {
//            var guiStyle = new GUIStyle(UBStyles.DebugBackgroundStyle)
//            {
//                normal = {textColor = Color.white},
//                fixedHeight = 25,
//                padding = new RectOffset(5, 0, 5, 0)
//            };
//            var rect = GetRect(guiStyle);
//            GUI.Box(rect, "Temporary Editing: Changes will be lost!", guiStyle);
//        }
//        base.OnInspectorGUI();
//    }

//    protected override void DoAdditionalGUI()
//    {
//        base.DoAdditionalGUI();
//        DoIncludes();
//    }

//    protected override void DoTriggerLists()
//    {
//        base.DoTriggerLists();
//        if (InstanceTarget.Includes.Count < 1) return;
//        //DoSubHeader("Included");
//        //foreach (var include in InstanceTarget.Includes)
//        //{
//        //    if (include == null) continue;
//        //    foreach (var trigger in include.Behaviour.Triggers)
//        //    {
//        //        DoTriggerButton(new UBTriggerContent(include.Behaviour.name + ": " + trigger.DisplayName, UBStyles.IncludeTriggerBackgroundStyle, UBStyles.InstanceTriggerBackgroundStyle, null, null, false)
//        //        {
//        //            //SubLabel = include.name
//        //        });
//        //    }
//        //}
//    }

//    private void DoIncludeGUI(SerializedProperty includes, UBSharedBehaviour behaviour, int index, string s)
//    {
//        var closureSafeIndex = index;
//        if (DoTriggerButton(new UBTriggerContent(s, UBStyles.CommandBarClosedStyle, null, UBStyles.RemoveButtonStyle, delegate()
//        {
//            InstanceTarget.Includes.RemoveAll(p => p == null || p.Behaviour == behaviour);
//            EditorUtility.SetDirty(target);
//        })))
//        {
//            if (!BehaviourStack.Contains(target))
//            {
//                BehaviourStack.Push(target);
//            }
//            Selection.activeObject = behaviour;
//        }
//    }

//    private void DoIncludes()
//    {
//        if (!IsShowingTriggers) return;
//        var includes = serializedObject.FindProperty("_includes");
//        if (DoToolbar("Includes", IncludesOpen, () => OnAddReference(includes))) IncludesOpen = !IncludesOpen;
//        if (!IncludesOpen) return;
//        for (var i = 0; i < includes.arraySize; i++)
//        {
//            var include = includes.GetArrayElementAtIndex(i);
//            var behaviourProperty = include.FindPropertyRelative("_behaviour");
//            if (behaviourProperty.objectReferenceValue == null) continue;
//            DoIncludeGUI(includes, behaviourProperty.objectReferenceValue as UBSharedBehaviour, i, behaviourProperty.objectReferenceValue.name);
//        }
//    }

//    private void OnAddReference(SerializedProperty references)
//    {
//        var menu = new GenericMenu();
//        foreach (var uBehaviour in UBAssetManager.Behaviours)
//        {
//            UBSharedBehaviour behaviour = uBehaviour;
//            menu.AddItem(new GUIContent(uBehaviour.name), false, () =>
//            {
//                serializedObject.Update();
//                references.arraySize++;
//                var item = references.GetArrayElementAtIndex(references.arraySize - 1);
//                item.FindPropertyRelative("_behaviour").objectReferenceValue = behaviour;
//                item.FindPropertyRelative("_declares").arraySize = 0;
//                serializedObject.ApplyModifiedProperties();
//            });
//        }
//        //menu.AddItem(new GUIContent("Create New For " +Target.Name), false, () =>
//        //{
//        //    var behaviour = UBAssetManager.NewUBehaviour();
//        //    AssetDatabase.Refresh();
//        //    serializedObject.Update();
//        //    references.arraySize++;
//        //    var item = references.GetArrayElementAtIndex(references.arraySize - 1);
//        //    item.FindPropertyRelative("_behaviour").objectReferenceValue = behaviour;
//        //    item.FindPropertyRelative("_declares").arraySize = 0;
//        //    serializedObject.ApplyModifiedProperties();
//        //});
//        menu.ShowAsContext();
//    }
//}

[CustomEditor(typeof(UBComponent), true)]
public class TBehavioursComponentEditor : TBehavioursEditorBase
{

    public UBComponent InstanceTarget
    {
        get { return target as UBComponent; }
    }

    public override IUBehaviours Target
    {
        get { return InstanceTarget.Behaviour; }
    }

    public override bool CanAllowOverrides
    {
        get { return false; }
    }

    public override void OnInspectorGUI()
    {
        IsGlobals = false;
        serializedObject.ApplyModifiedProperties();
        if (InstanceTarget.BehaviourInclude != null)
        {
            InstanceTarget.BehaviourInclude.Sync();
        }
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        var sp = serializedObject.FindProperty("_BehaviourInclude");
        var behaviourProperty = sp.FindPropertyRelative("_behaviour");
        //if (InstanceTarget.BehaviourInclude == null)
        EditorGUILayout.PropertyField(behaviourProperty);
        if (EditorGUI.EndChangeCheck())
        {
            if (InstanceTarget.BehaviourInclude != null) 
                InstanceTarget.BehaviourInclude.Sync();
        }


        if (Target != null && InstanceTarget.BehaviourInclude != null) 
        {
            
            DoIncludeVariables(sp,true);
            
        }

   
    }

    protected override void DoAdditionalGUI()
    {
        base.DoAdditionalGUI();
        
    }

  
}