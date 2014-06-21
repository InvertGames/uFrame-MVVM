#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.DialogueTrees{

	[AddComponentMenu("")]
	[Name("Condition")]
	[Description("This node will execute the first child node if the Condition is true, or the second one if the Condition is false. The Actor selected is used for the Condition check")]
	public class DLGConditionNode : DLGNodeBase{

		[SerializeField]
		private ConditionTask _condition;

		public ConditionTask condition{
			get{return _condition;}
			set
			{
				_condition = value;
				if (_condition != null)
					_condition.SetOwnerSystem(this);
			}
		}

		public override string nodeName{
			get{return base.nodeName + " " + finalActorName;}
		}

		public override int maxOutConnections{
			get {return 2;}
		}

		public override System.Type outConnectionType{
			get{return typeof(Connection);}
		}

		protected override NodeStates OnExecute(){

			if (outConnections.Count == 0){
				DLGTree.StopGraph();
				return Error("There are no connections.");
			}

			if (!finalActor){
				DLGTree.StopGraph();
				return Error("Actor not found");
			}

			if (!condition){
				Debug.LogWarning("No Condition on Dialoge Condition Node ID " + ID);
				outConnections[0].Execute(finalActor, finalBlackboard);
				return NodeStates.Success;
			}

			if (condition.CheckCondition(finalActor, finalBlackboard)){
				outConnections[0].Execute(finalActor, finalBlackboard);
				return NodeStates.Success;
			}

			if (outConnections.Count == 2){
				outConnections[1].Execute(finalActor, finalBlackboard);
				return NodeStates.Failure;
			}

			graph.StopGraph();
			return NodeStates.Success;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			base.OnNodeGUI();

			if (condition == null){
				GUILayout.Label("No Condition");
				return;
			}

			if (outConnections.Count == 0){
				GUILayout.Label("Connect Outcomes");
				return;
			}

			GUILayout.Label(condition.taskInfo);
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();
			if (condition == null){
				EditorUtils.ShowComponentSelectionButton(gameObject, typeof(ConditionTask), delegate(Component c){condition = (ConditionTask)c;});
				return;
			}
			
			EditorUtils.TaskTitlebar(condition);
		}

		#endif
	}
}