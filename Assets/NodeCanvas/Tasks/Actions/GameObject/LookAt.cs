using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("GameObject")]
	[AgentType(typeof(Transform))]
	public class LookAt : ActionTask{

		[RequiredField]
		public BBGameObject lookTarget;
		public bool forever = false;

		protected override string info{
			get {return "LookAt " + lookTarget;}
		}

		protected override void OnExecute(){
			DoLook();
		}

		protected override void OnUpdate(){
			DoLook();
		}

		void DoLook(){

			Vector3 lookPos = lookTarget.value.transform.position;
			lookPos.y = agent.transform.position.y;
			agent.transform.LookAt(lookPos);

			if (!forever)
				EndAction(true);
		}
	}
}