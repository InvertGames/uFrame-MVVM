using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Blackboard")]
	[Description("Set a blackboard component variable")]
	public class SetComponent : ActionTask {

		public BBComponent valueA = new BBComponent{blackboardOnly = true};
		public BBComponent valueB;

		protected override string info{
			get {return "Set " + valueA + " = " + valueB;}
		}

		protected override void OnExecute(){

			valueA.value = valueB.value;
			EndAction();
		}
	}
}