using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("GameObject")]
	[AgentType(typeof(Transform))]
	public class InstantiateGameObject : ActionTask {

		public BBVector clonePosition;
		public BBGameObject saveCloneAs = new BBGameObject{blackboardOnly = true};

		protected override string info{
			get {return "Instantiate " + agentInfo + " at " + clonePosition + " as " + saveCloneAs;}
		}

		protected override void OnExecute(){

			saveCloneAs.value = (GameObject)Instantiate(agent.gameObject, clonePosition.value, Quaternion.identity);
			EndAction();
		}
	}
}