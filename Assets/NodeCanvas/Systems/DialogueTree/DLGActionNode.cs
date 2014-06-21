#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
namespace NodeCanvas.DialogueTrees{

	[AddComponentMenu("")]
	[Name("Action")]
	[Description("This node will execute an ActionTask with the DialogueActor selected. The BlackBoard will be taken from the selected Actor.")]
	public class DLGActionNode : DLGNodeBase{

		[SerializeField]
		private ActionTask _action;

		public ActionTask action{
			get {return _action;}
			set
			{
				_action = value;
				if (_action != null)
					_action.SetOwnerSystem(this);
			}
		}

		public override string nodeName{
			get{return base.nodeName + " " + finalActorName;}
		}

		protected override NodeStates OnExecute(){

			if (!finalActor){
				DLGTree.StopGraph();
				return Error("Actor not found");
			}

			if (!action){
				OnActionEnd(true);
				return NodeStates.Success;
			}

			DLGTree.currentNode = this;

			nodeState = NodeStates.Running;
			action.ExecuteAction(finalActor, finalBlackboard, OnActionEnd);
			return nodeState;
		}

		private void OnActionEnd(System.ValueType success){

			if ( (bool)success ){
				Continue();
				return;
			}

			nodeState = NodeStates.Failure;
			DLGTree.StopGraph();
		}

		protected override void OnReset(){
			if (action)
				action.EndAction(false);
		}

		public override void OnGraphPaused(){
			if (action)
				action.PauseAction();
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			base.OnNodeGUI();
			
			if (action == null){
				GUILayout.Label("No Action");
				return;
			}

			GUILayout.Label(action.taskInfo);
		}

		protected override void OnNodeInspectorGUI(){
			
			base.OnNodeInspectorGUI();

			if (action == null){
				EditorUtils.ShowComponentSelectionButton(gameObject, typeof(ActionTask), delegate(Component a){action = (ActionTask)a;} );
				return;
			}
			
			EditorUtils.TaskTitlebar(action);
		}

		#endif
	}
}