using UnityEngine;

namespace NodeCanvas.Actions{

	[Name("Set Visibility")]
	[Category("GameObject")]
	[AgentType(typeof(Transform))]
	public class SetObjectVisibility : ActionTask{

		public enum SetMode {Invisible, Visible, Toggle}
		public SetMode SetTo= SetMode.Toggle;

		protected override string info{
			get {return "Set Visibility To '" + SetTo + "'";}
		}

		protected override void OnExecute(){

			bool value;
			
			if (SetTo == SetMode.Toggle){
			
				value = !agent.gameObject.activeSelf;
			
			} else {

				value = (int)SetTo == 1;
			}

			agent.gameObject.SetActive(value);
			EndAction();
		}
	}
}