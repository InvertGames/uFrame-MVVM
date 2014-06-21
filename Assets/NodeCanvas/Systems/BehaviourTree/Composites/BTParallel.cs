using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	[Name("Parallel")]
	[Category("Composites")]
	[Description("The Parallel will execute all it's children simultaneously. If set to 'First Failure', it will act as a Sequencer, while if set to 'First Success', it will act as a Selector")]
	[Icon("Parallel")]
	public class BTParallel : BTComposite{

		public enum TerminationConditions {FirstSuccess, FirstFailure}
		public TerminationConditions terminationCondition= TerminationConditions.FirstFailure;

		private List<Connection> finishedConnections = new List<Connection>();

		public override string nodeName{
			get {return "<color=#ff64cb>PARALLEL</color>";}
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			for ( int i= 0; i < outConnections.Count; i++){

				if (finishedConnections.Contains(outConnections[i]))
					continue;

				nodeState = outConnections[i].Execute(agent, blackboard);

				if (nodeState == NodeStates.Failure && terminationCondition == TerminationConditions.FirstFailure)
					return nodeState;

				if (nodeState == NodeStates.Success && terminationCondition == TerminationConditions.FirstSuccess)
					return nodeState;

				if (nodeState != NodeStates.Running)
					finishedConnections.Add(outConnections[i]);
			}

			if (finishedConnections.Count == outConnections.Count){
				if (terminationCondition == TerminationConditions.FirstFailure)
					return NodeStates.Success;
				if (terminationCondition == TerminationConditions.FirstSuccess)
					return NodeStates.Failure;
			}

			return NodeStates.Running;
		}

		protected override void OnReset(){

			finishedConnections.Clear();
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			GUILayout.Label(terminationCondition.ToString());
		}

		#endif
	}
}