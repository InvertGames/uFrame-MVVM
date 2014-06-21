using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Blackboard")]
	[Description("Create a new Vector out of 3 floats and save it to the blackboard")]
	public class ComposeVector : ActionTask {

		public BBFloat x;
		public BBFloat y;
		public BBFloat z;
		public BBVector saveAs =  new BBVector{blackboardOnly = true};

		protected override string info{
			get {return "New Vector as " + saveAs;}
		}

		protected override void OnExecute(){

			saveAs.value = new Vector3(x.value, y.value, z.value);
			EndAction();
		}
	}
}