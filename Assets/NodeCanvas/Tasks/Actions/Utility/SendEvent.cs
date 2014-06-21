using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Utility")]
	[Description("Send a graph event. If global is true, all graphs in scene will receive this event. Use along with the 'Check Event' Condition")]
	[AgentType(typeof(GraphOwner))]
	public class SendEvent : ActionTask {

		[RequiredField]
		public BBString eventName;
		public BBFloat delay;
		public bool global;

		protected override string info{
			get{ return (global? "Global " : "") + "Send [" + eventName + "]" + (delay.value > 0? " after " + delay + " sec." : "" );}
		}

		protected override void OnUpdate(){

			if (elapsedTime > delay.value){
				if (global){
					Graph.SendGlobalEvent(eventName.value);
				} else {
					(agent as GraphOwner).SendEvent(eventName.value);
				}
				EndAction();
			}
		}
	}
}