using UnityEngine;

namespace NodeCanvas.Actions{

	[Name("Set Mecanim Trigger")]
	[Category("Mecanim")]
	[AgentType(typeof(Animator))]
	public class MecanimSetTrigger : ActionTask{

		[RequiredField]
		public string mecanimParameter;

		[GetFromAgent]
		private Animator animator;

		protected override string info{
			get{return "Mec.SetTrigger '" + mecanimParameter + "'";}
		}

		protected override void OnExecute(){

			animator.SetTrigger(mecanimParameter);
			EndAction();
		}
	}
}