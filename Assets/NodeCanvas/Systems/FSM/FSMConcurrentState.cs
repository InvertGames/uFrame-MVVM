#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.StateMachines{

	[AddComponentMenu("")]
	[Name("Concurrent")]
	[Description("This node will execute as soon as the graph is started and in parallel to any other state. This is not a state per se and thus it has no transitions as well as it can't be Entered by transitions.")]
	public class FSMConcurrentState : FSMState{

		[SerializeField]
		private ActionList _actionList;

		private bool hasBeenExecuted;

		public ActionList actionList{
			get {return _actionList;}
			set
			{
				_actionList = value;
				if (_actionList != null)
					_actionList.SetOwnerSystem(graph);
			}
		}

		public override int maxInConnections{
			get {return 0;}
		}

		public override int maxOutConnections{
			get {return 0;}
		}

		public override bool allowAsPrime{
			get {return false;}
		}

		protected override void Init(){
			hasBeenExecuted = false;
		}

		protected override void Enter(){

			if (!actionList){
				graph.StopGraph();
				return;
			}

			if (hasBeenExecuted){
				Finish();
				return;
			}

			actionList.ExecuteAction(graphAgent, graphBlackboard, OnActionListFinished);
		}

		private void OnActionListFinished(System.ValueType didSucceed){
			Finish();
			hasBeenExecuted = true;
		}

		protected override void Exit(){
			actionList.EndAction(false);
		}

		protected override void Pause(){
			actionList.PauseAction();
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		[SerializeField]
		private bool tempIsUpdated;

		protected override void OnCreate(){
			actionList = gameObject.AddComponent<ActionList>();
			actionList.runInParallel = true;
			tempIsUpdated = true;
		}

		///TEMPORAARY UPDATE
		protected override void OnValidate(){
			base.OnValidate();
			if (!tempIsUpdated){
				tempIsUpdated = true;
				actionList.runInParallel = true;
			}
		}
		/////
		
		protected override void OnNodeGUI(){

			base.OnNodeGUI();

			if (actionList)
				GUILayout.Label(actionList.taskInfo);
		}

		protected override void OnNodeInspectorGUI(){

			if (!actionList)
				return;

			EditorUtils.CoolLabel("Actions");
			actionList.ShowListGUI();
			actionList.ShowNestedActionsGUI();
		}

		#endif
	}
}