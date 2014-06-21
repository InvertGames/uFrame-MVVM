using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("Movement")]
	[AgentType(typeof(NavMeshAgent))]
	public class MoveToFromList : ActionTask{

		[RequiredField]
		public BBGameObjectList targetList = new BBGameObjectList{useBlackboard = true};
		public BBFloat speed = new BBFloat{value = 3};
		public float keepDistance = 0.1f;

		private int randomIndex;
		private Vector3 lastRequest;

		//for faster acccess
		private NavMeshAgent navAgent{
			get {return (NavMeshAgent)agent;}
		}

		protected override string info{
			get {return "Go Random " + targetList;}
		}

		protected override void OnExecute(){

			int newValue = Random.Range(0, targetList.value.Count);
			while(newValue == randomIndex)
				newValue = Random.Range(0, targetList.value.Count);

			randomIndex = newValue;
			var targetGo = targetList.value[randomIndex];
			if (targetGo == null){
				Debug.Log("List's game object is null on MoveToFromList Action");
				EndAction(false);
				return;
			}

			var targetPos = targetGo.transform.position;

			navAgent.speed = speed.value;
			if ( (navAgent.transform.position - targetPos).magnitude < navAgent.stoppingDistance + keepDistance){
				EndAction(true);
				return;
			}

			Go();
		}

		protected override void OnUpdate(){
			Go();
		}

		void Go(){

			var targetPos = targetList.value[randomIndex].transform.position;
			
			if (lastRequest != targetPos){
				if ( !navAgent.SetDestination( targetPos) ){
					EndAction(false);
					return;
				}
			}

			lastRequest = targetPos;

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