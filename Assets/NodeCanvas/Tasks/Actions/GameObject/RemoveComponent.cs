using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("GameObject")]
	[AgentType(typeof(Transform))]
	public class RemoveComponent : ActionTask {

		[RequiredField]
		public BBString componentName;

		protected override string info{
			get {return "Remove '" + componentName;}
		}

		protected override void OnExecute(){

			var comp = agent.GetComponent(componentName.value);
			if (comp != null){
				Destroy(comp);
				EndAction(true);
				return;
			}

			EndAction(false);
		}
	}
}