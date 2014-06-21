#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	[Name("Repeater")]
	[Category("Decorators")]
	[Description("Repeater will repeat it's child either x times, or until it returns the specified state")]
	[Icon("Repeat")]
	public class BTRepeater : BTDecorator{

		public enum RepeatTypes {

			RepeatTimes,
			RepeatUntil,
			RepeatForever
		}

		public enum RepeatUntil {

			Failure = 0,
			Success = 1
		}

		public RepeatTypes repeatType= RepeatTypes.RepeatTimes;
		public RepeatUntil repeatUntil= RepeatUntil.Success;
		public int repeatTimes = 1;

		private int currentIteration = 1;

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (!decoratedConnection)
				return NodeStates.Resting;

			nodeState = decoratedConnection.Execute(agent, blackboard);

			if (nodeState == NodeStates.Success || nodeState == NodeStates.Failure){

				if (repeatType == RepeatTypes.RepeatTimes){

					repeatTimes = Mathf.Max(repeatTimes, 1);
					if (currentIteration >= repeatTimes)
						return nodeState;

					currentIteration ++;

				} else if (repeatType == RepeatTypes.RepeatUntil){

					if ((int)nodeState == (int)repeatUntil)
						return nodeState;
				}

				decoratedConnection.ResetConnection();
				return NodeStates.Running;
			}

			return nodeState;
		}

		protected override void OnReset(){

			currentIteration = 1;
		}


		/////////////////////////////////////////
		/////////GUI AND EDITOR STUFF////////////
		/////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){

			if (repeatType == RepeatTypes.RepeatTimes){

				GUILayout.Label(repeatTimes + " Times");
				if (Application.isPlaying)
					GUILayout.Label("Itteration: " + currentIteration.ToString());

			} else if (repeatType == RepeatTypes.RepeatUntil){

				GUILayout.Label("Until " + repeatUntil);
			
			} else {

				GUILayout.Label("Repeat Forever");
			}
		}

		protected override void OnNodeInspectorGUI(){

			repeatType = (RepeatTypes)EditorGUILayout.EnumPopup("Repeat Type", repeatType);

			if (repeatType == RepeatTypes.RepeatTimes){

				repeatTimes = EditorGUILayout.IntField("Times", repeatTimes);

			} else if (repeatType == RepeatTypes.RepeatUntil){

				repeatUntil = (RepeatUntil)EditorGUILayout.EnumPopup("Repeat Until", repeatUntil);
			}
		}

		#endif
	}
}