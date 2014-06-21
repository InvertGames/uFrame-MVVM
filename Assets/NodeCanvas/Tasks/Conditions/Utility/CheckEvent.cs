using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Category("✫ Utility")]
	[Description("Check if an event is received and return true for one frame")]
	[EventListener("OnCustomEvent")]
	public class CheckEvent : ConditionTask {

		[RequiredField]
		public BBString eventName;

		protected override string info{
			get {return "[" + eventName + "]"; }
		}

		protected override bool OnCheck(){
			return false;
		}

		void OnCustomEvent(string receivedEvent){

			if (receivedEvent == eventName.value)
				YieldReturn(true);
		}


		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnTaskInspectorGUI(){

			DrawDefaultInspector();
			if (Application.isPlaying && GUILayout.Button("Debug Receive"))
				OnCustomEvent(eventName.value);
		}
		
		#endif
	}
}