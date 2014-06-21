using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	[Name("Limiter")]
	[Category("Decorators")]
	[Description("Limiter, limits the access of it's child node either a specific number of times, or every specific amount of time. By default the node is 'Treated as Inactive' to it's parent when access is Limited. Unchecking this option will instead return Failure when access is limited.")]
	[Icon("Lock")]
	public class BTLimiter : BTDecorator {

		public enum LimitMode{
			LimitNumberOfTimes,
			CoolDown
		}

		public LimitMode limitMode = LimitMode.CoolDown;

		public BBInt maxCount = new BBInt{value = 1};
		private int executedCount;

		public BBFloat coolDownTime = new BBFloat{value = 5};
		private float currentTime;

		public bool inactiveWhenLimited = true;


		public override void OnGraphStarted(){
			executedCount = 0;
			currentTime = 0;
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (decoratedConnection == null)
				return NodeStates.Resting;

			if (limitMode == LimitMode.CoolDown){

				if (currentTime > 0)
					return inactiveWhenLimited? NodeStates.Resting : NodeStates.Failure;

				nodeState = decoratedConnection.Execute(agent, blackboard);
				if (nodeState == NodeStates.Success || nodeState == NodeStates.Failure)
					StartCoroutine(Cooldown());
			}
			else
			if (limitMode == LimitMode.LimitNumberOfTimes){

				if (executedCount >= maxCount.value)
					return inactiveWhenLimited? NodeStates.Resting : NodeStates.Failure;

				nodeState = decoratedConnection.Execute(agent, blackboard);
				if (nodeState == NodeStates.Success || nodeState == NodeStates.Failure)
					executedCount += 1;
			}

			return nodeState;
		}

		IEnumerator Cooldown(){

			currentTime = coolDownTime.value;
			while (currentTime > 0){
				currentTime -= Time.deltaTime;
				yield return null;
			}
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			if (limitMode == LimitMode.CoolDown){
				GUILayout.Label("", GUILayout.Height(25));
				Rect pRect = new Rect(5, GUILayoutUtility.GetLastRect().y, nodeRect.width - 10, 20);
				UnityEditor.EditorGUI.ProgressBar(pRect, currentTime/coolDownTime.value, currentTime > 0? "Cooling..." : "Accessible");
			}
			else
			if (limitMode == LimitMode.LimitNumberOfTimes){
				GUILayout.Label(executedCount + " / " + maxCount.value + " Accessed Times");
			}
		}

		protected override void OnNodeInspectorGUI(){

			limitMode = (LimitMode)UnityEditor.EditorGUILayout.EnumPopup("Mode", limitMode);

			if (limitMode == LimitMode.CoolDown){
				coolDownTime = (BBFloat)EditorUtils.BBVariableField("CoolDown Time", coolDownTime);
			}
			else
			if (limitMode == LimitMode.LimitNumberOfTimes){
				maxCount = (BBInt)EditorUtils.BBVariableField("Max Count", maxCount);
			}

			inactiveWhenLimited = UnityEditor.EditorGUILayout.Toggle("Inactive When Limited", inactiveWhenLimited);
		}
		
		#endif
	}
}