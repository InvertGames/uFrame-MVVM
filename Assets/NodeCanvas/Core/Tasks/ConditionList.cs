#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas{

	[ExecuteInEditMode]
	[Category("✫ Utility")]
	public class ConditionList : ConditionTask{

		public List<ConditionTask> conditions = new List<ConditionTask>();
		public bool allSuccessRequired = true;

		override protected string info{
			get
			{
				string finalText = conditions.Count != 0? "" : "No Conditions";
				if (conditions.Count > 1)
					finalText += "<b>(" + (allSuccessRequired? "ALL True" : "ANY True") + ")</b>\n";

				for (int i= 0; i < conditions.Count; i++){
					finalText += conditions[i].taskInfo + (i == conditions.Count -1? "" : "\n" );
				}
				return finalText;
			}
		}

		override protected bool OnCheck(){

			int succeedChecks = 0;

			foreach (ConditionTask con in conditions){

				if (con.CheckCondition(agent, blackboard)){

					if (!allSuccessRequired)
						return true;

					succeedChecks ++;
				}
			}

			return succeedChecks == conditions.Count;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		ConditionTask currentViewCondition;

		protected override void OnValidate(){

			base.OnValidate();
			for (int i = 0; i < conditions.Count; i++){
				if (conditions[i] == null)
					conditions.RemoveAt(i);
			}
		}

		private void OnDestroy(){

			foreach(ConditionTask condition in conditions){
				var c = condition;
				EditorApplication.delayCall += ()=>
				{
					if (c) DestroyImmediate(c, true);
				};
			}
		}

		public override Task CopyTo(GameObject go){

			if (this == null)
				return null;

			ConditionList copiedList = (ConditionList)go.AddComponent<ConditionList>();
			Undo.RegisterCreatedObjectUndo(copiedList, "Copy List");
			Undo.RecordObject(copiedList, "Copy List");
			UnityEditor.EditorUtility.CopySerialized(this, copiedList);
			copiedList.conditions.Clear();

			foreach (ConditionTask condition in conditions){
				var copiedCondition = condition.CopyTo(go);
				copiedList.AddCondition(copiedCondition as ConditionTask);
			}

			return copiedList;
		}

		override protected void OnTaskInspectorGUI(){

			ShowListGUI();
			ShowNestedConditionsGUI();

			if (GUI.changed && this != null)
	            EditorUtility.SetDirty(this);
		}

		public void ShowListGUI(){

			if (this == null)
				return;

			EditorUtils.ShowComponentSelectionButton(gameObject, typeof(ConditionTask), delegate(Component c){ AddCondition((ConditionTask)c) ;});

			//Check for possibly removed components
			foreach (ConditionTask condition in conditions.ToArray()){
				if (condition == null)
					conditions.Remove(condition);
			}

			if (conditions.Count == 0){
				EditorGUILayout.HelpBox("Please add some Conditions", MessageType.Info);
				return;
			}

			foreach (ConditionTask con in conditions.ToArray()){

				GUI.backgroundColor = new Color(1, 1, 1, 0.25f);
				GUILayout.BeginHorizontal("box");
					
				GUI.color = con == currentViewCondition? Color.yellow : Color.white;
				GUILayout.Label(con.taskInfo);
				GUI.color = Color.white;

				var e = Event.current;
				var lastRect = GUILayoutUtility.GetLastRect();
				EditorGUIUtility.AddCursorRect(lastRect, MouseCursor.Link);
				if (e.button == 0 && e.type == EventType.MouseUp && lastRect.Contains(e.mousePosition)){
					currentViewCondition = currentViewCondition == con? null : con;
					e.Use();
				}
				
				if (GUILayout.Button("X", GUILayout.MaxWidth(20))){
					Undo.RecordObject(this, "List Remove Task");
					conditions.Remove(con);
					Undo.DestroyObjectImmediate(con);
				}
				GUILayout.EndHorizontal();
			}

			EditorUtils.Separator();

			if (conditions.Count > 1){
				GUI.backgroundColor = new Color(0.5f,0.5f,0.5f);
				if (GUILayout.Button(allSuccessRequired? "ALL True Required":"ANY True Suffice"))
					allSuccessRequired = !allSuccessRequired;
				GUI.backgroundColor = Color.white;
			}
		}


		public void ShowNestedConditionsGUI(){

			if (conditions.Count == 1)
				currentViewCondition = conditions[0];

			if (currentViewCondition){
				EditorUtils.BoldSeparator();
				EditorUtils.TaskTitlebar(currentViewCondition);
			}
		}

		public void AddCondition(ConditionTask condition){
			Undo.RecordObject(this, "List Add Task");
			Undo.RecordObject(condition, "List Add Task");
			currentViewCondition = condition;
			conditions.Add(condition);
			condition.SetOwnerSystem(ownerSystem);
		}

		#endif
	}
}