using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace NodeCanvas{

	///Automaticaly added to the agent when needed.
	///Handles forwarding Unity event messages to the Tasks that need them as well as Custom event forwarding.
	///A task can listen to an event message by using [EventListener(param string[])] attribute.
	public class AgentUtilities : MonoBehaviour{

		private List<Listener> listeners = new List<Listener>();

		void OnAnimatorIK(int layerIndex){
			Send("OnAnimatorIK", layerIndex);
		}

		void OnBecameInvisible(){
			Send("OnBecameInvisible", null);
		}

		void OnBecameVisible(){
			Send("OnBecameVisible", null);
		}

		void OnCollisionEnter(Collision collisionInfo){
			Send("OnCollisionEnter", collisionInfo);
		}

		void OnCollisionExit(Collision collisionInfo){
			Send("OnCollisionExit", collisionInfo);
		}

		void OnCollisionStay(Collision collisionInfo){
			Send("OnCollisionStay", collisionInfo);
		}

		void OnCollisionEnter2D(Collision2D collisionInfo){
			Send("OnCollisionEnter2D", collisionInfo);
		}

		void OnCollisionExit2D(Collision2D collisionInfo){
			Send("OnCollisionExit2D", collisionInfo);
		}

		void OnCollisionStay2D(Collision2D collisionInfo){
			Send("OnCollisionStay2D", collisionInfo);
		}

		void OnTriggerEnter(Collider other){
			Send("OnTriggerEnter", other);
		}

		void OnTriggerExit(Collider other){
			Send("OnTriggerExit", other);
		}

		void OnTriggerStay(Collider other){
			Send("OnTriggerStay", other);
		}

		void OnTriggerEnter2D(Collider2D other){
			Send("OnTriggerEnter2D", other);
		}

		void OnTriggerExit2D(Collider2D other){
			Send("OnTriggerExit2D", other);
		}

		void OnTriggerStay2D(Collider2D other){
			Send("OnTriggerStay2D", other);
		}

		void OnMouseDown(){
			Send("OnMouseDown", null);
		}

		void OnMouseDrag(){
			Send("OnMouseDrag", null);
		}

		void OnMouseEnter(){
			Send("OnMouseEnter", null);
		}

		void OnMouseExit(){
			Send("OnMouseExit", null);
		}

		void OnMouseOver(){
			Send("OnMouseOver", null);
		}

		void OnMouseUp(){
			Send("OnMouseUp", null);
		}


		void OnCustomEvent(string eventName){
			Send("OnCustomEvent", eventName);
		}

		///Add a listener
		public void Listen(MonoBehaviour mono, string toMessage){

			foreach ( Listener listener in listeners){
				if (listener.mono == mono){
					if (!listener.messages.Contains(toMessage))
						listener.messages.Add(toMessage);
					return;
				}
			}

			listeners.Add(new Listener(mono, toMessage));
		}

		///Remove a listener completely
		public void Forget(MonoBehaviour mono){

			foreach ( Listener listener in listeners){
				if (listener.mono == mono){
					listeners.Remove(listener);
					return;
				}
			}
		}

		///Remove a listener from a message
		public void Forget(MonoBehaviour mono, string forgetMessage){

			foreach ( Listener listener in listeners){
				if (listener.mono == mono){
					listener.messages.Remove(forgetMessage);
					if (listener.messages.Count == 0)
						listeners.Remove(listener);
					return;
				}
			}
		}

		///Call the functions
		public void Send(string eventMessage, object arg){

			foreach ( Listener listener in listeners.ToArray()){

				foreach ( string msg in listener.messages.ToArray()){

					if (eventMessage == msg && listener.mono != null){

						var method = listener.mono.GetType().GetMethod(eventMessage, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (method == null){
							Debug.LogWarning("Method '" + eventMessage + "' not found on subscribed type " + listener.mono.GetType().Name);
							return;
						}
						var args = arg != null? new object[]{arg} : null;
						if (method.ReturnType == typeof(IEnumerator)){
							MonoManager.current.StartCoroutine( (IEnumerator)method.Invoke(listener.mono, args ));
						} else {
							method.Invoke(listener.mono, args);
						}
					}
				}
			}
		}

		///Describes a listener
		class Listener{

			public MonoBehaviour mono;
			public List<string> messages = new List<string>();

			public Listener(MonoBehaviour mono, string message){

				this.mono = mono;
				this.messages.Add(message);
			}
		}
	}
}