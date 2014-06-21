using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Category("✫ Blackboard")]
	public class CheckFloat : ConditionTask{

		public enum CheckTypes
		{
			EqualTo,
			GreaterThan,
			LessThan
		}
		public BBFloat valueA = new BBFloat{blackboardOnly = true};
		public CheckTypes checkType = CheckTypes.EqualTo;
		public BBFloat valueB;

		[SliderField(0,0.1f)]
		public float differenceThreshold = 0.05f;

		protected override string info{
			get
			{
				string symbol = " == ";
				if (checkType == CheckTypes.GreaterThan)
					symbol = " > ";
				if (checkType == CheckTypes.LessThan)
					symbol = " < ";
				return valueA + symbol + valueB;
			}
		}

		protected override bool OnCheck(){

			if (checkType == CheckTypes.EqualTo){
				if (Mathf.Abs(valueA.value - valueB.value) <= differenceThreshold)
					return true;
				return false;
			}

			if (checkType == CheckTypes.GreaterThan){
				if (valueA.value > valueB.value)
					return true;
				return false;
			}

			if (checkType == CheckTypes.LessThan){
				if (valueA.value < valueB.value)
					return true;
				return false;
			}

			return true;
		}
	}
}