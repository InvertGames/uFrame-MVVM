using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	[Name("Interruptor")]
	[Category("Decorators")]
	[Description("Interruptor will execute it's child node if the condition assigned is false. If the condition is or becomes true, Interruptor will stop & reset the child node if running and return false. Otherwise it will return whatever the child returns.")]
	[Icon("Interruptor")]
	public class BTInterruptor : BTDecorator{

		[SerializeField]
		private ConditionTask _condition;
		public ConditionTask condition{
			get {return _condition;}
			set
			{
				_condition = value;
				if (_condition != null)
					_condition.SetOwnerSystem(graph);
			}
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (!decoratedConnection)
				return NodeStates.Resting;

			if (!condition || condition.CheckCondition(agent, blackboard) == false)
				return decoratedConnection.Execute(agent, blackboard);

			if (decoratedConnection.connectionState == NodeStates.Running)
				decoratedConnection.ResetConnection();
			
			return NodeStates.Failure;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			if (condition == null){

				GUILayout.Label("No Condition");
				return;
			}

			GUILayout.Label(condition.taskInfo);
		}

		protected override void OnNodeInspectorGUI(){

			if (condition == null){
				EditorUtils.ShowComponentSelectionButton(gameObject, typeof(ConditionTask), delegate(Component c){condition = (ConditionTask)c;});
				return;
			}
			
			EditorUtils.TaskTitlebar(condition);
		}

		#endif
	}
}