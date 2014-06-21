using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Blackboard")]
	[Description("Set a blackboard string variable")]
	public class SetString : ActionTask {

		public BBString stringA = new BBString{blackboardOnly = true};
		public BBString stringB;

		protected override string info{
			get {return "Set " + stringA + " = " + stringB;}
		}

		protected override void OnExecute(){
			stringA.value = stringB.value;
			EndAction();
		}
	}
}