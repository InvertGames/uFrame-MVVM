using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas{

	///Place this on a game object to debug the EventHandler
	public class EventDebugger : MonoBehaviour{

		public bool logEvents = false;

		public Dictionary<System.Enum, List<EventHandler.SubscribedMember>> subscribedMembers{
			get{return EventHandler.subscribedMembers;}
		}

		void Awake(){
			EventHandler.logEvents = logEvents;
		}
	}
}