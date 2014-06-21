using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Blackboard")]
	[Description("Set a blackboard float variable at random between min and max value")]
	public class SetFloatRandom : ActionTask {

		public BBFloat minValue;
		public BBFloat maxValue;

		public BBFloat setValue = new BBFloat{blackboardOnly = true};

		protected override string info{
			get {return "Set " + setValue + " Random(" + minValue + ", " + maxValue + ")";}
		}

		protected override void OnExecute(){

			setValue.value = Random.Range(minValue.value, maxValue.value);
			EndAction();
		}
	}
}