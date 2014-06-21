using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Utility")]
	[Description("Simply wait for an amount of seconds")]
	public class Wait : ActionTask {

		public BBFloat waitTime = new BBFloat{value = 1};

		protected override string info{
			get {return "Wait " + waitTime + " sec.";}
		}

		protected override void OnUpdate(){
			if (elapsedTime >= waitTime.value)
				EndAction();
		}
	}
}