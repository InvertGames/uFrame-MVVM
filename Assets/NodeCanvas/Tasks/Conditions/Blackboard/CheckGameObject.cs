using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Category("✫ Blackboard")]
	public class CheckGameObject : ConditionTask {

		public BBGameObject valueA = new BBGameObject{blackboardOnly = true};
		public BBGameObject valueB;

		protected override string info{
			get {return valueA + " == " + valueB;}
		}

		protected override bool OnCheck(){

			return valueA.value == valueB.value;
		}
	}
}