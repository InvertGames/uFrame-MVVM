using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Category("GameObject")]
	[AgentType(typeof(Transform))]
	public class CheckDistance : ConditionTask{

		[RequiredField]
		public BBGameObject CheckTarget;
		public BBFloat distance;

		protected override string info{
			get {return "Distance < " + distance.ToString() + " to " + CheckTarget;}
		}

		protected override bool OnCheck(){

			if (Vector3.Distance(agent.transform.position, (CheckTarget.value as GameObject).transform.position) < distance.value)
				return true;

			return false;
		}
	}
}