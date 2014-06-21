using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("GameObject")]
	public class GetGameObjectWithName : ActionTask {

		[RequiredField]
		public BBString gameObjectName;
		public BBGameObject saveAs = new BBGameObject(){blackboardOnly = true};

		protected override string info{
			get {return "Get Object " + gameObjectName + " as " + saveAs;}
		}

		protected override void OnExecute(){

			saveAs.value = GameObject.Find(gameObjectName.value);
			EndAction();
		}
	}
}