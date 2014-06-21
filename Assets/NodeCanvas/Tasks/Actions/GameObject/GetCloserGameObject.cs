using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Name("Get Closer GameObject From List")]
	[Category("GameObject")]
	[AgentType(typeof(Transform))]
	public class GetCloserGameObject : ActionTask {

		[RequiredField]
		public BBGameObjectList list;
		
		[RequiredField]
		public BBGameObject saveAs = new BBGameObject(){blackboardOnly = true};

		protected override string info{
			get {return "Get Closer from '" + list + "' as " + saveAs;}
		}

		protected override void OnExecute(){

			if (list.value.Count == 0){
				EndAction(false);
				return;
			}

			float closerDistance = Mathf.Infinity;
			GameObject closerGO = null;
			foreach(GameObject go in list.value){
				var dist = Vector3.Distance(agent.transform.position, go.transform.position);
				if (dist < closerDistance){
					closerDistance = dist;
					closerGO = go;
				}
			}

			saveAs.value = closerGO;
			EndAction(true);
		}
	}
}