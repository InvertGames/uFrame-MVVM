using UnityEngine;
using System.Collections;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	[Name("Condition")]
	[Description("The Condition Node can be assigned a ConditionTask. It returns Success or Failure based on that Condition Task assigned.")]
	[Icon("Condition")]
	public class BTCondition : BTNodeBase{

		[SerializeField]
		private ConditionTask _condition;

		[SerializeField]
		private BTCondition _referencedNode;

		public ConditionTask condition{
			get
			{
				if (referencedNode != null)
					return referencedNode.condition;
				return _condition;
			}
			set
			{
				_condition = value;
				if (_condition != null)
					_condition.SetOwnerSystem(graph);
			}
		}

		public BTCondition referencedNode{
			get {return _referencedNode;}
			private set {_referencedNode = value;}
		}

		public override string nodeName{
			get{return "CONDITION";}
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (condition)
				return condition.CheckCondition(agent, blackboard)? NodeStates.Success: NodeStates.Failure;

			return NodeStates.Failure;
		}

		/////////////////////////////////////////
		/////////GUI AND EDITOR STUFF////////////
		/////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){
			
	        Rect markRect = new Rect(nodeRect.width - 15, 5, 15, 15);
	        if (referencedNode != null)
	        	GUI.Label(markRect, "<b>R</b>");

			if (condition == null) GUILayout.Label("No Condition");
			else GUILayout.Label(condition.taskInfo);
		}

		protected override void OnNodeInspectorGUI(){

			if (referencedNode != null){
				if (GUILayout.Button("Select Reference"))
					Graph.currentSelection = referencedNode;

				if (GUILayout.Button("Break Reference"))
					BreakReference();

				if (condition != null){
					GUILayout.Label("<b>" + condition.taskName + "</b>");
					condition.ShowInspectorGUI();
				}
				return;
			}

			if (!condition){
				EditorUtils.ShowComponentSelectionButton(gameObject, typeof(ConditionTask), delegate(Component c){condition = (ConditionTask)c;});
				return;
			}

			EditorUtils.TaskTitlebar(condition);
		}

		protected override void OnContextMenu(UnityEditor.GenericMenu menu){
			menu.AddItem (new GUIContent ("Duplicate (Reference)"), false, DuplicateReference);
		}
		
		private void DuplicateReference(){
			var newNode = graph.AddNewNode(typeof(BTCondition)) as BTCondition;
			newNode.nodeRect.center = this.nodeRect.center + new Vector2(50, 50);
			newNode.referencedNode = referencedNode != null? referencedNode : this;
		}

		public void BreakReference(){

			if (referencedNode == null)
				return;

			if (referencedNode.condition != null)
				condition = (ConditionTask)referencedNode.condition.CopyTo(this.gameObject);

			referencedNode = null;
		}

		#endif
	}
}