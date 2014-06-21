#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	[Name("Remapper")]
	[Category("Decorators")]
	[Description("Remapper will remap it's child node's Success and Failure return state, to another state. Used to either invert the childs return state or to allways return a specific state.")]
	[Icon("Remap")]
	public class BTRemapper : BTDecorator{

		public enum RemapStates
		{
			Failure  = 0,
			Success  = 1,
			Inactive = 3
		}

		public RemapStates successRemap = RemapStates.Success;
		public RemapStates failureRemap = RemapStates.Failure;

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (!decoratedConnection)
				return NodeStates.Resting;
			
			nodeState = decoratedConnection.Execute(agent, blackboard);
			
			if (nodeState == NodeStates.Success){

				if (successRemap == RemapStates.Inactive)
					decoratedConnection.ResetConnection();

				return (NodeStates)successRemap;

			} else if (nodeState == NodeStates.Failure){

				if (successRemap == RemapStates.Inactive)
					decoratedConnection.ResetConnection();

				return (NodeStates)failureRemap;
			}

			return nodeState;
		}

		/////////////////////////////////////////
		/////////GUI AND EDITOR STUFF////////////
		/////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){

			if ((int)successRemap != (int)NodeStates.Success)
				GUILayout.Label("Success = " + successRemap);

			if ((int)failureRemap != (int)NodeStates.Failure)
				GUILayout.Label("Failure = " + failureRemap);

			GUILayout.Label("", GUILayout.Height(1));
		}

		#endif
	}
}