using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Category("GameObject")]
	[AgentType(typeof(Transform))]
	public class HasComponent : ConditionTask {

		[RequiredField]
		public BBString componentName;

		protected override string info{
			get {return "Has Component " + componentName;}
		}

		protected override bool OnCheck(){
			return agent.GetComponent(componentName.value) != null;
		}
	}
}