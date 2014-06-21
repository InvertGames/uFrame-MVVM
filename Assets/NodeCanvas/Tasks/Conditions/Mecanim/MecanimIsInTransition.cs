using UnityEngine;

namespace NodeCanvas.Conditions{

	[Category("Mecanim")]
	[Name("Is In Transition")]
	[AgentType(typeof(Animator))]
	public class MecanimIsInTransition : ConditionTask {

		public int layerIndex;

		[GetFromAgent]
		Animator animator;

		protected override string info{
			get {return "Mec.Is In Transition";}
		}

		protected override bool OnCheck(){

			return animator.IsInTransition(layerIndex);
		}
	}
}