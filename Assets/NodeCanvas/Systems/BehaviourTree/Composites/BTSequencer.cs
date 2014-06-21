#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	[Name("Sequencer")]
	[Category("Composites")]
	[Description("The Sequencer executes it’s child nodes in order from highest to lowest priority. It will return Failure as soon as any child returns Failure. It will return Success as soon as all children finish in Success.\nA ‘Dynamic’ Sequencer, will keep executing it’s children in order even if one is currently Running. So if a higher priority child returns Failure, the Sequencer will Interrupt the currently Running child node and return Failure as well.")]
	[Icon("Sequencer")]
	public class BTSequencer : BTComposite{

		public bool isDynamic;
		public bool isRandom;

		private int lastRunningNodeIndex= 0;

		public override string nodeName{
			get{return "<color=#bf7fff>SEQUENCER</color>";}
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (outConnections.Count-1 < lastRunningNodeIndex && lastRunningNodeIndex != 0)
				lastRunningNodeIndex--;

			for ( int i= isDynamic? 0 : lastRunningNodeIndex; i < outConnections.Count; i++){

				nodeState = outConnections[i].Execute(agent, blackboard);

				if (nodeState == NodeStates.Running){

					if (isDynamic && i < lastRunningNodeIndex)
						outConnections[lastRunningNodeIndex].ResetConnection();
					
					lastRunningNodeIndex = i;
					return nodeState;
				}

				if (nodeState == NodeStates.Failure){

					if (isDynamic && i < lastRunningNodeIndex)
						outConnections[lastRunningNodeIndex].ResetConnection();

					return nodeState;
				}
			}

			return nodeState;
		}

		protected override void OnReset(){

			lastRunningNodeIndex = 0;
			if (isRandom)
				outConnections = Shuffle(outConnections);
		}

		public override void OnGraphStarted(){
			OnReset();
		}

		//Fisher-Yates shuffle algorithm
		private List<Connection> Shuffle(List<Connection> list){
			for ( int i= list.Count -1; i > 0; i--){
				int j = (int)Mathf.Floor(Random.value * (i + 1));
				Connection temp = list[i];
				list[i] = list[j];
				list[j] = temp;
			}

			return list;
		}

		/////////////////////////////////////////
		/////////GUI AND EDITOR STUFF////////////
		/////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){

			if (isDynamic)
				GUILayout.Label("Dynamic");
			if (isRandom)
				GUILayout.Label("Random");

			if (!isDynamic && !isRandom)
				GUILayout.Label("", GUILayout.Height(1));
		}

		#endif
	}
}