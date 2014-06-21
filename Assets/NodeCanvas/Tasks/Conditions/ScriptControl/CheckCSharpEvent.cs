using UnityEngine;
using System.Reflection;

namespace NodeCanvas.Conditions{

	[Category("✫ Script Control")]
	[Description("Will subscribe to an event of EventHandler type or custom handler with zero parameters and return type of void")]
	[AgentType(typeof(Transform))]
	public class CheckCSharpEvent : ConditionTask {

		[RequiredField]
		public Component script;
		[RequiredField]
		public string eventName;
		
		protected override string info{
			get {return "'" + eventName + "' Raised";}
		}

		protected override string OnInit(){

			var eventInfo = script.GetType().GetEvent(eventName);
			MethodInfo m;
			System.Delegate handler;
			if (eventInfo.EventHandlerType == typeof(System.EventHandler)){
				m = this.GetType().GetMethod("DefaultRaised", BindingFlags.Instance | BindingFlags.NonPublic);
				handler = System.Delegate.CreateDelegate(eventInfo.EventHandlerType, this, m);
			} else {
				m = this.GetType().GetMethod("Raised", BindingFlags.Instance | BindingFlags.NonPublic);
				handler = System.Delegate.CreateDelegate(eventInfo.EventHandlerType, this, m);
			}
			eventInfo.AddEventHandler(script, handler);
			return null;
		}

		void DefaultRaised(object sender, System.EventArgs e){
			Raised();
		}

		void Raised(){
			YieldReturn(true);
		}

		protected override bool OnCheck(){
			return false;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnTaskInspectorGUI(){

			if (agent == null){
				UnityEditor.EditorGUILayout.HelpBox("This Condition needs the Agent to be known. Currently the Agent is unknown.\nConsider overriding the agent.", UnityEditor.MessageType.Error);
				return;
			}

			if (script == null || script.gameObject != agent.gameObject){
				script = agent.transform;
				eventName = null;
			}

			script = EditorUtils.ComponentField("Script", script, typeof(Component), false);
			eventName = EditorUtils.StringPopup("Event", eventName, EditorUtils.GetAvailableEvents(script.GetType()));
		}
		
		#endif
	}
}