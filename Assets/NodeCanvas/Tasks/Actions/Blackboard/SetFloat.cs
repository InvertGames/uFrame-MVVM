using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Blackboard")]
	[Description("Set a blackboard float variable")]
	public class SetFloat : ActionTask{

		public enum SetMode
		{
			SET,
			ADD,
			SUBTRACT,
			MULTIPLY
		}
		public BBFloat valueA = new BBFloat{blackboardOnly = true};
		public SetMode Operation = SetMode.SET;
		public BBFloat valueB;

		protected override string info{
			get
			{
				if (Operation == SetMode.SET)
					return "Set " + valueA + " = " + valueB;

				if (Operation == SetMode.ADD)
					return "Set " + valueA + " += " + valueB;
				
				if (Operation == SetMode.SUBTRACT)
					return "Set " + valueA + " -= " + valueB;

				if (Operation == SetMode.MULTIPLY)
					return "Set " + valueA + " *= " + valueB;

				return string.Empty;			
			}
		}

		protected override void OnExecute(){

			if (Operation == SetMode.SET){
				valueA.value = valueB.value;
			} else
			if (Operation == SetMode.ADD){
				valueA.value += valueB.value;
			} else
			if (Operation == SetMode.SUBTRACT){
				valueA.value -= valueB.value;
			} else
			if (Operation == SetMode.MULTIPLY){
				valueA.value *= valueB.value;
			}

			EndAction(true);
		}
	}
}