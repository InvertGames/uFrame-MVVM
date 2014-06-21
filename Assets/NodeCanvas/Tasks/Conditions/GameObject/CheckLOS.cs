using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Name("Check Line Of Sight")]
	[Category("GameObject")]
	[AgentType(typeof(Transform))]
	public class CheckLOS : ConditionTask{

		[RequiredField]
		public BBGameObject LosTarget;
		public Vector3 Offset;

		protected override string info{
			get {return "LOS with " + LosTarget.ToString();}
		}

		protected override bool OnCheck(){

			Transform t = LosTarget.value.transform;

			RaycastHit hit = new RaycastHit();
			if (Physics.Linecast(agent.transform.position + Offset, t.position + Offset, out hit)){
				if (hit.collider != t.collider)
					return false;
			}

			return true;
		}
	}
}