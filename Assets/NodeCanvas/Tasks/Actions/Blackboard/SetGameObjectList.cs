using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Blackboard")]
	[Description("Set a blackboard GameObject list variable")]
	public class SetGameObjectList : ActionTask {

		public BBGameObjectList valueA = new BBGameObjectList{blackboardOnly = true};
		public BBGameObjectList valueB;

		protected override string info{
			get {return "Set " + valueA + " = " + valueB;}
		}

		protected override void OnExecute(){

			valueA.value = valueB.value;
			EndAction();
		}
	}
}