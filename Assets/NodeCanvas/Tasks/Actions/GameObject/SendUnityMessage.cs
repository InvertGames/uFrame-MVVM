using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("GameObject")]
	[AgentType(typeof(Transform))]
	public class SendUnityMessage : ActionTask{

		[RequiredField]
		public BBString methodName;

		protected override string info{
			get {return "Message " + methodName;}
		}

		protected override void OnExecute(){

			agent.SendMessage(methodName.value);
			EndAction();
		}
	}
}