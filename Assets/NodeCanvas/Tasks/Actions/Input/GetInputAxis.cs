using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("Input")]
	public class GetInputAxis : ActionTask {

		public string xAxisName = "Horizontal";
		public string yAxisName = "Vertical";
		public BBVector saveAs = new BBVector{blackboardOnly = true};

		public bool forever;

		protected override void OnExecute(){
			Do();
		}

		protected override void OnUpdate(){
			Do();
		}

		void Do(){
			
			saveAs.value = new Vector3(Input.GetAxis(xAxisName), Input.GetAxis(yAxisName), 0);
			if (!forever)
				EndAction();
		}
	}
}