using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

namespace NodeCanvas{

	///Handles subscribers and dispatches messages. Works with MonoBehaviour subscribers and method subscribers as well.
	///If you want to debug events send and subscribers, add the EventDebugger component somewhere
	public static class EventHandler{

		public static bool logEvents;
		public static Dictionary<Enum, List<SubscribedMember>> subscribedMembers = new Dictionary<Enum, List<SubscribedMember>>();

		///Subscribes a MonoBehaviour to an Event along with options. When the Event is dispatched a funtion
		///with the same name as the Event will be called on the subscribed MonoBehaviour. Events are provided by an Enum.
		public static void Subscribe(MonoBehaviour mono, Enum toEvent, int invokePriority = 0, bool unsubscribeWhenReceive = false){

			MethodInfo method = mono.GetType().GetMethod(toEvent.ToString(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			if (method == null){
				Debug.LogError("EventHandler: No Method with name '" + toEvent.ToString() + "' exists on '" + mono.GetType().Name + "' Subscribed Type");
				return;
			}

			if (!subscribedMembers.ContainsKey(toEvent))
				subscribedMembers[toEvent] = new List<SubscribedMember>();

			foreach (SubscribedMember member in subscribedMembers[toEvent]){

				if (member.subscribedMono == mono){

					if (logEvents)
						Debug.Log("Mono " + mono + " is allready subscribed to " + toEvent);

					return;
				}
			}

			if (logEvents)
				Debug.Log("@@@ " + mono + " subscribed to " + toEvent);
			
			subscribedMembers[toEvent].Add(new SubscribedMember(mono, invokePriority, unsubscribeWhenReceive));
			subscribedMembers[toEvent] = subscribedMembers[toEvent].OrderBy(member => -member.invokePriority).ToList();
		}


		//Subscribe a function to an Event
		public static void SubscribeFunction(Action<System.Object> func, Enum toEvent){

			if (!subscribedMembers.ContainsKey(toEvent))
				subscribedMembers[toEvent] = new List<SubscribedMember>();

			foreach (SubscribedMember member in subscribedMembers[toEvent]) {
				
				if (member.subscribedFunction == func){
					
					if (logEvents)
						Debug.Log("Function allready subscribed to " + toEvent);
					
					return;
				}
			}

			subscribedMembers[toEvent].Add(new SubscribedMember(func, 0, false));
		}


		///Unsubscribe a MonoBehaviour member from all Events
		public static void Unsubscribe(MonoBehaviour mono){

			if (!mono)
				return;

			foreach (Enum evt in subscribedMembers.Keys){
				foreach (SubscribedMember member in subscribedMembers[evt].ToArray()){

					if (member.subscribedMono == mono){

						subscribedMembers[evt].Remove(member);

						if (logEvents)
							Debug.Log("XXX " + mono + "Unsubscribed from everything!");
					}
				}
			}
		}

		///Unsubscribes a MonoBehaviour member from an Event
		public static void Unsubscribe(MonoBehaviour mono, Enum fromEvent){

			if (!mono || !subscribedMembers.ContainsKey(fromEvent))
				return;

			foreach (SubscribedMember member in subscribedMembers[fromEvent].ToArray()){

				if (member.subscribedMono == mono){

					subscribedMembers[fromEvent].Remove(member);

					if (logEvents)
						Debug.Log("XXX Member " + mono + " Unsubscribed from " + fromEvent);

					return;
				}
			}

			if (logEvents)
				Debug.Log("You tried to Unsubscribe " + mono + " from " + fromEvent + ", but it was never subscribed there!");
		}

		//Unsubscribes a Function member from everything
		public static void UnsubscribeFunction(Action<System.Object> func){

			if (func == null)
				return;

			foreach (Enum evnt in subscribedMembers.Keys){
				foreach (SubscribedMember member in subscribedMembers[evnt].ToArray()){
					if (member.subscribedFunction != null && member.subscribedFunction.ToString() == func.ToString())
						subscribedMembers[evnt].Remove(member);
				}
			}

			if (logEvents)
				Debug.Log("XXX " + func.ToString() + " Unsubscribed from everything");
		}


		///Dispatches a new Event. On any subscribers listening, a function of the same name as the Event will be called. An Object may be passed as an argument.
		public static bool Dispatch(Enum evnt, System.Object theMessage = null){

			if (logEvents)
				Debug.Log(">>> Event " + evnt + " Dispatched. (" + theMessage.GetType() + ") Argument");

			if (!subscribedMembers.ContainsKey(evnt)){
				Debug.LogWarning("EventHandler: Event '" + evnt.ToString() + "' was not received by anyone!");
				return false;
			}

			foreach (SubscribedMember member in subscribedMembers[evnt].ToArray()){

				MonoBehaviour mono = member.subscribedMono;

				//clean up by-product
				if (mono == null && member.subscribedFunction == null){
					subscribedMembers[evnt].Remove(member);
					continue;
				}

				if (logEvents)
					Debug.Log("<<< Event " + evnt + " Received by " + mono);

				if (member.unsubscribeWhenReceive)
					Unsubscribe(mono, evnt);

				if (member.subscribedFunction != null){
					member.subscribedFunction(theMessage);
					continue;
				}
				
				mono.enabled = true;
				mono.gameObject.SetActive(true);
				MethodInfo method = mono.GetType().GetMethod(evnt.ToString(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
				var parameters = method.GetParameters();

				if (parameters.Length > 1){
					Debug.LogError("Subscribed function to call '" + method.Name + "' has more than one parameter on " + mono + ". It should only have one.", mono.gameObject);
					continue;
				}

				if (parameters.Length == 0){

					method.Invoke(mono, null);

				} else {

					if (theMessage != null && (parameters[0].ParameterType).IsAssignableFrom( theMessage.GetType() ) == false){
						Debug.LogError("Dispatched message object(" + theMessage.GetType() + ") can not be received by subscribed function (" + parameters[0].ParameterType + ") for '" + evnt + "' on " + mono, mono.gameObject);
						continue;
					}

					method.Invoke(mono, new System.Object[] {theMessage});
				}
			}

			return true;
		}

		///Describes a member to be handled by the EventHandler.
		public class SubscribedMember{

			public MonoBehaviour subscribedMono;
			public Action<System.Object> subscribedFunction;
			public int invokePriority = 0;
			public bool unsubscribeWhenReceive;

			public SubscribedMember(MonoBehaviour mono, int invokePriority, bool unsubscribeWhenReceive){

				this.subscribedMono = mono;
				this.invokePriority = invokePriority;
				this.unsubscribeWhenReceive = unsubscribeWhenReceive;
			}

			public SubscribedMember(Action<System.Object> func, int invokePriority, bool unsubscribeWhenReceive){

				this.subscribedFunction = func;
				this.invokePriority = invokePriority;
				this.unsubscribeWhenReceive = unsubscribeWhenReceive;
			}
		}

	}
}