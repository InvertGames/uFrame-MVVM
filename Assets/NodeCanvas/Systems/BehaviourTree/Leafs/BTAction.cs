using UnityEngine;
using System.Collections;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	[Name("Action")]
	[Description("The Action Node can be assigned an ActionTask wich will be executed as soon as the Action Node is executed. The node will return Running until the Action assigned ends in success of failure, at which point the node will return Success or Failure accordingly.")]
	[Icon("Action")]
	///Executes an Action Task assigned and returns Success or Failure based on that Action Task
	public class BTAction : BTNodeBase{

		[SerializeField]
		private ActionTask _action;

		[SerializeField]
		private BTAction _referencedNode;

		public Task task{
			get {return action;}
			set {action = (ActionTask)value;}
		}

		public ActionTask action{
			get
			{
				if (referencedNode != null)
					return referencedNode.action;
				return _action;
			}
			set
			{
				_action = value;
				if (_action != null)
					_action.SetOwnerSystem(graph);
			}
		}

		public BTAction referencedNode{
			get { return _referencedNode; }
			private set {_referencedNode = value;}
		}

		public override string nodeName{
			get {return "ACTION";}
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (action == null)
				return NodeStates.Success;

			//need to check action.isPaused here. Special case
			if (nodeState == NodeStates.Resting || action.isPaused){
				nodeState = NodeStates.Running;
				action.ExecuteAction(agent, blackboard, OnActionEnd);
			}

			return nodeState;
		}

		//Callback from the "ActionTask".
		private void OnActionEnd(System.ValueType didSucceed){

			nodeState = (bool)didSucceed? NodeStates.Success : NodeStates.Failure;
		}

		protected override void OnReset(){
			if (action)
				action.EndAction(false);
		}

		public override void OnGraphPaused(){
			if (action)
				action.PauseAction();
		}

		/////////////////////////////////////////
		/////////GUI AND EDITOR STUFF////////////
		/////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){

	        Rect markRect = new Rect(nodeRect.width - 15, 5, 15, 15);
	        if (referencedNode != null)
	        	GUI.Label(markRect, "<b>R</b>");

			if (action == null) GUILayout.Label("No Action");
			else GUILayout.Label(action.taskInfo);
		}

		protected override void OnNodeInspectorGUI(){
			
			if (referencedNode != null){

				if (GUILayout.Button("Select Reference"))
					Graph.currentSelection = referencedNode;

				if (GUILayout.Button("Break Reference"))
					BreakReference();

				if (action != null){
					GUILayout.Label("<b>" + action.taskName + "</b>");
					action.ShowInspectorGUI();
				}
				return;
			}

			if (action == null){
				EditorUtils.ShowComponentSelectionButton(gameObject, typeof(ActionTask), delegate(Component a){action = (ActionTask)a;});
				return;
			}
			
			EditorUtils.TaskTitlebar(action);
		}

		protected override void OnContextMenu(UnityEditor.GenericMenu menu){
			menu.AddItem (new GUIContent ("Duplicate (Reference)"), false, DuplicateReference);
		}
		
		private void DuplicateReference(){
			var newNode = graph.AddNewNode(typeof(BTAction)) as BTAction;
			newNode.nodeRect.center = this.nodeRect.center + new Vector2(50, 50);
			newNode.referencedNode = referencedNode != null? referencedNode : this;
		}

		public void BreakReference(){

			UnityEditor.Undo.RecordObject(this, "Break Reference");
			if (referencedNode == null)
				return;

			if (referencedNode.action != null)
				action = (ActionTask)referencedNode.action.CopyTo(this.gameObject);

			referencedNode = null;
		}

		#endif
	}
}