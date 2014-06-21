#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	[Name("Selector")]
	[Category("Composites")]
	[Description("The Selector Node executes it’s child nodes either in priority order or randomly, until one of it’s children returns Success at which point the Selector will also return Success. If none does, the Selector will return Failure.\nIf a Selector is ‘Dynamic’, it will keep evaluating in order even if a child node is Running. So if a higher priority node returns Success, the Selector will interupt the currenly Running child node and return Success as well.")]
	[Icon("Selector")]
	public class BTSelector : BTComposite{

		public bool isDynamic;
		public bool isRandom;

		private int lastRunningNodeIndex= 0;

		public override string nodeName{
			get {return "<color=#b3ff7f>SELECTOR</color>";}
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

				if (nodeState == NodeStates.Success){
					
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