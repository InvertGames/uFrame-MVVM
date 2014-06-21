using UnityEngine;
using System.Linq;
using NodeCanvas;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[AgentType(typeof(Transform))]
	[Category("✫ Blackboard")]
	[Description("Will sort the gameobjects in the target list by their distance to the agent (closer first) and save that list to the blackboard")]
	public class SortListByDistance : ActionTask {

		[RequiredField]
		public BBGameObjectList targetList;
		public BBGameObjectList saveAs = new BBGameObjectList{blackboardOnly = true};
		public bool reverse;

		protected override string info{
			get {return "Sort " + targetList + " by distance as " + saveAs;}
		}

		protected override void OnExecute(){

			saveAs.value = targetList.value.OrderBy(go => Vector3.Distance(go.transform.position, agent.transform.position)).ToList();
			if (reverse)
				saveAs.value.Reverse();

			EndAction();
		}
	}
}