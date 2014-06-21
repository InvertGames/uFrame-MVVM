using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[DoNotList]
	[Category("✫ Blackboard")]
	public class GameObjectIsNull : ConditionTask {

		public BBGameObject gameObjectVar = new BBGameObject{blackboardOnly = true};

		protected override string info{
			get {return gameObjectVar + " == null";}
		}

		protected override bool OnCheck(){
			return gameObjectVar.value == null;
		}
	}
}