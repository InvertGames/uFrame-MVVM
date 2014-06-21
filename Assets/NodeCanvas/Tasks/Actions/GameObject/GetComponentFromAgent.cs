using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Name("Get Component")]
	[Category("GameObject")]
	[AgentType(typeof(Transform))]
	public class GetComponentFromAgent : ActionTask{

		[RequiredField]
		public string TypeToGet = "Transform";

		public BBComponent saveAs = new BBComponent{blackboardOnly = true};

		protected override string info{
			get{return "Get '" + TypeToGet + "' as " + saveAs;}
		}

		protected override void OnExecute(){

			var foundCompo = agent.GetComponent(TypeToGet);
			saveAs.value = foundCompo;

			if (foundCompo != null){

				EndAction(true);

			} else {

				Debug.LogWarning("No Component named '" + TypeToGet + "' found on agent. Does a Type with such name exist?");
				EndAction(false);
			}
		}
	}
}