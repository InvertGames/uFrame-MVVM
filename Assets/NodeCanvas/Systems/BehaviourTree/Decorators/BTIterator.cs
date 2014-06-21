#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Variables;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	[Name("Iterator")]
	[Category("Decorators")]
	[Description("Iterator will iterate a GameObject List taken from the Blackboard passed and on each execution the current iterated GameObject will be saved on that same blackboard with the name provided. It will keep iterating until the requirements set are met. If Reset Index is checked, then the Iterator will reset the current iterated index to zero whenever it resets, else the index will remain unless it's the last index of the list.")]
	[Icon("List")]
	///Iterate a GameObject list
	public class BTIterator : BTDecorator{

		public BBGameObjectList list = new BBGameObjectList{blackboardOnly = true};
		public BBGameObject current = new BBGameObject{blackboardOnly = true};
		public BBInt maxIteration;

		public enum TerminationConditions {FirstSuccess, FirstFailure, None}
		public TerminationConditions terminationCondition = TerminationConditions.None;
		public bool resetIndex = true;

		private int currentIndex;
		
		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (!decoratedConnection)
				return NodeStates.Resting;

			if (list.value == null || list.value.Count == 0)
				return NodeStates.Failure;

			if (list.value[currentIndex] == null){
				list.value.RemoveAt(currentIndex);
				return NodeStates.Running;
			}

			current.value = list.value[currentIndex];
			nodeState = decoratedConnection.Execute(agent, blackboard);

			if (nodeState == NodeStates.Success && terminationCondition == TerminationConditions.FirstSuccess)
				return NodeStates.Success;

			if (nodeState == NodeStates.Failure && terminationCondition == TerminationConditions.FirstFailure)
				return NodeStates.Failure;

			if (nodeState == NodeStates.Success || nodeState == NodeStates.Failure){

				if (currentIndex == list.value.Count - 1 || currentIndex == maxIteration.value - 1)
					return nodeState;

				decoratedConnection.ResetConnection();
				currentIndex ++;
				return NodeStates.Running;
			}

			return nodeState;
		}


		protected override void OnReset(){

			if (resetIndex || currentIndex == list.value.Count - 1 || currentIndex == maxIteration.value - 1)
				currentIndex = 0;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){

			GUILayout.Label("For Each \t'<b>$" + current.dataName + "</b>'");
			GUILayout.Label("In \t\t\t'<b>$" + list.dataName + "</b>'");
			if (terminationCondition != TerminationConditions.None)
				GUILayout.Label("Exit on " + terminationCondition.ToString());

			if (Application.isPlaying)
				GUILayout.Label("Index: " + currentIndex.ToString() + " / " + (list.value != null && list.value.Count != 0? (list.value.Count -1).ToString() : "?") );
		}

		#endif
	}
}