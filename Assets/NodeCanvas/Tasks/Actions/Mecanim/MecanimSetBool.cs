using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Name("Set Mecanim Bool")]
	[Category("Mecanim")]
	[AgentType(typeof(Animator))]
	public class MecanimSetBool : ActionTask{

		[RequiredField]
		public string mecanimParameter;
		public BBBool setTo;

		[GetFromAgent]
		private Animator animator;

		protected override string info{
			get{return "Mec.SetBool '" + mecanimParameter + "' to " + setTo;}
		}

		protected override void OnExecute(){

			animator.SetBool(mecanimParameter, (bool)setTo.value);
			EndAction(true);
		}
	}
}