using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Name("Check If In Front")]
	[Category("GameObject")]
	[AgentType(typeof(Transform))]
	public class CheckInFront : ConditionTask{

		[RequiredField]
		public BBGameObject CheckTarget;
		public float AngleToCheck = 70f;

		protected override string info{
			get {return CheckTarget + " in front";}
		}

		protected override bool OnCheck(){
			return Vector3.Angle(CheckTarget.value.transform.position - agent.transform.position, agent.transform.forward) < AngleToCheck;
		}
	}
}