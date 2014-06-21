using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Blackboard")]
	[Description("Set a blackboard GameObject variable")]
	public class SetGameObject : ActionTask {

		public BBGameObject valueA = new BBGameObject{blackboardOnly = true};
		public BBGameObject valueB;

		protected override string info{
			get {return "Set " + valueA + " = " + valueB;}
		}

		protected override void OnExecute(){

			valueA.value = valueB.value;
			EndAction();
		}
	}
}