using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.BehaviourTrees{

	[Category("My Nodes")]
	[Icon("SomeIcon")]
	public class SimpleDelay : BTNodeBase {

		public BBFloat waitTime;
		public BBBool testBool;
		
		private float timer;

		//When the BT starts
		public override void OnGraphStarted(){

		}

		//When the BT stops
		public override void OnGraphStoped(){

		}

		//When the BT pauses
		public override void OnGraphPaused(){

		}

		//When the node is Ticked. Agent and Blackboard parameters are optional. You can use an overload
		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){
			
			timer += Time.deltaTime;
			if (timer > waitTime.value)
				return testBool.value == true? NodeStates.Success : NodeStates.Failure;

			return NodeStates.Running;
		}

		//When the node resets: Start of graph, interrupted, new tree traversal.
		protected override void OnReset(){

			timer = 0;
		}

		////////////////////////////////////////
		#if UNITY_EDITOR

		//This is shown IN the node if you want
		protected override void OnNodeGUI(){

			GUILayout.Label("Wait " + waitTime + " sec.");
		}

		#endif
		////////////////////////////////////////
	}
}