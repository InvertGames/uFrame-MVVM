using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("Movement")]
	[AgentType(typeof(NavMeshAgent))]
	public class MoveToGameObject : ActionTask{

		[RequiredField]
		public BBGameObject target;
		public BBFloat speed = new BBFloat{value = 3};
		public float keepDistance = 0.1f;

		private Vector3 lastRequest;

		//for faster access
		private NavMeshAgent navAgent{
			get {return (NavMeshAgent)agent; }
		}

		protected override string info{
			get {return "GoTo " + target.ToString();}
		}

		protected override void OnExecute(){

			navAgent.speed = speed.value;
			if ( (navAgent.transform.position - target.value.transform.position).magnitude < navAgent.stoppingDistance + keepDistance){
				EndAction(true);
				return;
			}

			Go();
		}

		protected override void OnUpdate(){
			Go();
		}

		void Go(){
			
			var pos = target.value.transform.position;

			if (lastRequest != pos){
				if ( !navAgent.SetDestination( pos) ){
					EndAction(false);
					return;
				}
			}

			lastRequest = pos;

			if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance + keepDistance)
				EndAction(true);
		}

		protected override void OnStop(){

			lastRequest = Vector3.zero;
			if (navAgent.gameObject.activeSelf)
				navAgent.ResetPath();
		}

		protected override void OnPause(){
			OnStop();
		}
	}
}