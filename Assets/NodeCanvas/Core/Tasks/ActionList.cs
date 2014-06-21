#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas{

	[Category("✫ Utility")]
	[ExecuteInEditMode]
	///ActionList is an ActionTask itself that though holds multilple Action Tasks which can executes either in parallel or in sequence.
	public class ActionList : ActionTask{

		public List<ActionTask> actions = new List<ActionTask>();
		public bool runInParallel;
		
		private int currentActionIndex;

		public override float estimatedLength{
			get
			{
				float total = 0;
				foreach (ActionTask action in actions)
					total += action.estimatedLength;
				return total;			
			}
		}

		protected override string info{
			get
			{
				if (actions.Count == 0)
					return "No Actions";

				string finalText= string.Empty;
				for (int i= 0; i < actions.Count; i++)
					finalText += (actions[i].isRunning? "► " : actions[i].isPaused? "<b>||</b> " : "") + actions[i].taskInfo + (i == actions.Count -1? "" : "\n" );

				return finalText;			
			}
		}

		protected override void OnExecute(){

			if (actions.Count == 0){
				EndAction(false);
				return;
			}

			currentActionIndex = 0;
			
			if (runInParallel){
		
				for (int i= 0; i < actions.Count; i++)
					actions[i].ExecuteAction(agent, blackboard, OnNestedActionEnd);

			} else {

				actions[0].ExecuteAction(agent, blackboard, OnNestedActionEnd);
			}
		}

		//This is the callback from a nested action
		private void OnNestedActionEnd(System.ValueType didSucceed){

			if (!(bool)didSucceed){
				EndAction(false);
				return;
			}

			currentActionIndex ++;

			if (runInParallel){

				if (currentActionIndex == actions.Count){
					EndAction(true);
					return;
				}

			} else {

				if (currentActionIndex < actions.Count)
					actions[currentActionIndex].ExecuteAction(agent, blackboard, OnNestedActionEnd);
				else
					EndAction(true);
			}
		}

		protected override void OnStop(){

			foreach (ActionTask action in actions)
				action.EndAction(false);
		}

		protected override void OnPause(){
			
			foreach (ActionTask action in actions)
				action.PauseAction();			
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		private ActionTask currentViewAction;

		protected override void OnValidate(){

			base.OnValidate();
			for (int i = 0; i < actions.Count; i++){
				if (actions[i] == null)
					actions.RemoveAt(i);
			}
		}

		private void OnDestroy(){

			foreach(ActionTask action in actions){
				var a = action;
				EditorApplication.delayCall += ()=>
				{
					if (a) DestroyImmediate(a, true);
				};
			}
		}

		public override Task CopyTo(GameObject go){

			if (this == null)
				return null;

			ActionList copiedList = (ActionList)go.AddComponent<ActionList>();
			Undo.RegisterCreatedObjectUndo(copiedList, "Copy List");
			Undo.RecordObject(copiedList, "Copy List");
			UnityEditor.EditorUtility.CopySerialized(this, copiedList);
			copiedList.actions.Clear();

			foreach (ActionTask action in actions){
				var copiedAction = action.CopyTo(go);
				copiedList.AddAction(copiedAction as ActionTask);
			}

			return copiedList;
		}

		protected override void OnTaskInspectorGUI(){

			ShowListGUI();
			ShowNestedActionsGUI();

			if (GUI.changed && this != null)
	            EditorUtility.SetDirty(this);
		}

		//The action list gui
		public void ShowListGUI(){

			if (this == null)
				return;

			EditorUtils.ShowComponentSelectionButton(gameObject, typeof(ActionTask), delegate(Component a){ AddAction((ActionTask)a); });

			//Check first for possibly removed components
			foreach (ActionTask action in actions.ToArray()){
				if (action == null)
					actions.Remove(action);
			}

			if (actions.Count == 0){
				EditorGUILayout.HelpBox("No Actions", MessageType.None);
				return;
			}

			//Then present them
			EditorUtils.ReorderableList(actions, delegate(object o){

				var action = (ActionTask)o;
				GUI.backgroundColor = new Color(1, 1, 1, 0.25f);
				EditorGUILayout.BeginHorizontal("box");

				if (GUILayout.Button(EditorUtils.viewIcon, GUILayout.Width(25), GUILayout.Height(18)))
					currentViewAction = action == currentViewAction? null : action;

				GUI.color = action == currentViewAction? Color.yellow : Color.white;
				GUILayout.Label( (action.isRunning? "► " : action.isPaused? "<b>||</b> " : "") + action.taskInfo);
				GUI.color = Color.white;
				if (GUILayout.Button("X", GUILayout.Width(20))){
					Undo.RecordObject(this, "List Remove Task");
					actions.Remove(action);
					Undo.DestroyObjectImmediate(action);
				}
				EditorGUILayout.EndHorizontal();
			});

			if (actions.Count > 1)
				runInParallel = EditorGUILayout.ToggleLeft("Run In Parallel", runInParallel);
		}



		public void ShowNestedActionsGUI(){

			if (actions.Count == 1)
				currentViewAction = actions[0];

			if (currentViewAction != null){
				EditorUtils.BoldSeparator();
				EditorUtils.TaskTitlebar(currentViewAction);
			}
		}

		private void AddAction(ActionTask action){
			
			Undo.RecordObject(this, "List Add Task");
			Undo.RecordObject(action, "List Add Task");
			currentViewAction = action;
			actions.Add(action);
			action.SetOwnerSystem(ownerSystem);
		}

		#endif
	}
}