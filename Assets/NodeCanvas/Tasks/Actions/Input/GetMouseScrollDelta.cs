using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("Input")]
	public class GetMouseScrollDelta : ActionTask {

		public BBFloat saveAs = new BBFloat{blackboardOnly = true};
		public bool forever = false;

		protected override string info{
			get {return "Get Scroll Delta as " + saveAs;}
		}

		protected override void OnExecute(){
			Do();
		}

		protected override void OnUpdate(){
			Do();
		}

		void Do(){

			saveAs.value = Input.GetAxis("Mouse ScrollWheel");
			if (!forever)
				EndAction();
		}
	}
}