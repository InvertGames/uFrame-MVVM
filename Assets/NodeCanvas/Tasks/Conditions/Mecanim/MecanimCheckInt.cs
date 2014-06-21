using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Name("Check Mecanim Int")]
	[Category("Mecanim")]
	[AgentType(typeof(Animator))]
	public class MecanimCheckInt : ConditionTask {

		public enum ComparisonTypes{
			EqualTo,
			GreaterThan,
			LessThan
		}

		[RequiredField]
		public string mecanimParameter;
		public ComparisonTypes comparison = ComparisonTypes.EqualTo;
		public BBInt value;

		[GetFromAgent]
		private Animator animator;

		protected override string info{
			get
			{
				string comparisonString = "==";
				if (comparison == ComparisonTypes.GreaterThan)
					comparisonString = ">";
				if (comparison == ComparisonTypes.LessThan)
					comparisonString = "<";
				return "Mec.Int '" + mecanimParameter + "' " + comparisonString + " " + value;
			}
		}

		protected override bool OnCheck(){

			if (comparison == ComparisonTypes.GreaterThan)
				return animator.GetInteger(mecanimParameter) > value.value;

			if (comparison == ComparisonTypes.LessThan)
				return animator.GetInteger(mecanimParameter) < value.value;

			return animator.GetInteger(mecanimParameter) == value.value;
		}
	}
}