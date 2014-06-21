using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("Input")]
	public class GetMousePosition : ActionTask {

		public BBVector saveAs = new BBVector{blackboardOnly = true};
		public bool forever;


		protected override void OnExecute(){
			Do();
		}

		protected override void OnUpdate(){
			Do();
		}

		void Do(){

			saveAs.value = Input.mousePosition;
			if (!forever)
				EndAction();
		}
	}
}