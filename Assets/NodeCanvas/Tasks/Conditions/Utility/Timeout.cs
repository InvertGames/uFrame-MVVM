using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Category("✫ Utility")]
	[Description("Will return true after a specific amount of time has passed and false while still counting down")]
	public class Timeout : ConditionTask {

		public BBFloat timeout = new BBFloat{value = 1};
		private float currentTime;

		protected override string info{
			get {return "Timed Out " + Mathf.Round( (currentTime/timeout.value) * 100 ) + "%";}
		}

		protected override bool OnCheck(){

			if (currentTime < timeout.value){

				if (currentTime == 0)
					StartCoroutine(Count());
			
				return false;
			}

			currentTime = 0;
			return true;
		}

		IEnumerator Count(){

			while (currentTime < timeout.value){
				currentTime += Time.deltaTime;
				yield return null;
			}
		}
	}
}