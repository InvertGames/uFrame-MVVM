using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Blackboard")]
	[Description("Create up to 3 floats from a Vector and save them to blackboard")]
	public class DecomposeVector : ActionTask {

		public BBVector targetVector;
		public BBFloat x = new BBFloat{blackboardOnly = true};
		public BBFloat y = new BBFloat{blackboardOnly = true};
		public BBFloat z = new BBFloat{blackboardOnly = true};

		protected override string info{
			get {return "Decompose Vector " + targetVector;}
		}

		protected override void OnExecute(){

			x.value = targetVector.value.x;
			y.value = targetVector.value.y;
			z.value = targetVector.value.z;
			EndAction();
		}
	}
}