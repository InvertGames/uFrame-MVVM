using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Name("Set IK")]
	[Category("Mecanim")]
	[AgentType(typeof(Animator))]
	public class MecanimSetIK : ActionTask{

		public AvatarIKGoal IKGoal;
		[RequiredField]
		public BBGameObject goal;
		public BBFloat weight;

		[GetFromAgent]
		private Animator animator;

		protected override string info{
			get{return "Set '" + IKGoal + "' " + goal;}
		}

		protected override void OnExecute(){

			animator.SetIKPositionWeight(IKGoal, weight.value);
			animator.SetIKPosition(IKGoal, goal.value.transform.position);
			EndAction();
		}
	}
}