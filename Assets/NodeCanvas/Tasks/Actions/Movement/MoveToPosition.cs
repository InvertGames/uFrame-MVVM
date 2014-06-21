using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("Movement")]
	[AgentType(typeof(NavMeshAgent))]
	public class MoveToPosition : ActionTask{

		public BBVector TargetPosition;
		public BBFloat speed = new BBFloat{value = 3};
		public float keepDistance = 0.1f;

		private Vector3 lastRequest;

		//for faster acccess
		private NavMeshAgent navAgent{
			get {return (NavMeshAgent)agent;}
		}

		protected override string info{
			get {return "GoTo " + TargetPosition.ToString();}
		}

		protected override void OnExecute(){

			navAgent.speed = speed.value;
			if ( (navAgent.transform.position - TargetPosition.value).magnitude < navAgent.stoppingDistance + keepDistance){
				EndAction(true);
				return;
			}

			Go();
		}

		protected override void OnUpdate(){
			Go();
		}

		void Go(){

			if (lastRequest != TargetPosition.value){
				if ( !navAgent.SetDestination( TargetPosition.value) ){
					EndAction(false);
					return;
				}
			}

			lastRequest = TargetPosition.value;

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