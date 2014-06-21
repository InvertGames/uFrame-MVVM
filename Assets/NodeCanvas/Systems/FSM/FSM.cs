using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas.StateMachines{

	[AddComponentMenu("")]
	///The actual State Machine
	public class FSM : Graph{

		private FSMState currentState;
		private FSMState lastState;
		private List<FSMAnyState> anyStates = new List<FSMAnyState>();

		///The current state name. Null if none
		public string currentStateName{
			get {return currentState != null? currentState.stateName : null; }
		}

		///The last state name. Not the current! Null if none
		public string lastStateName{
			get	{return lastState != null? lastState.stateName : null; }
		}

		public override System.Type baseNodeType{
			get {return typeof(FSMState);}
		}

		protected override void OnGraphStarted(){

			anyStates.Clear();
			foreach(Node node in allNodes){

				if (node.GetType() == typeof(FSMConcurrentState))
					node.Execute(agent, blackboard);

				if (node.GetType() == typeof(FSMAnyState))
					anyStates.Add(node as FSMAnyState);
			}

			EnterState(lastState == null? primeNode as FSMState : lastState);
		}

		protected override void OnGraphUpdate(){

			foreach(FSMAnyState anyState in anyStates)
				anyState.UpdateAnyState();

			currentState.OnUpdate();
		}

		protected override void OnGraphStoped(){

			lastState = null;
			currentState = null;
		}

		protected override void OnGraphPaused(){
			lastState = currentState;
			currentState = null;
		}

		///Enter a state providing the state itself
		public void EnterState(FSMState state){

			if (!isRunning){
				Debug.LogWarning("Tried to EnterState on an FSM that was not running", gameObject);
				return;
			}

			if (currentState != null){
				
				currentState.ResetNode();
				
				//for editor..
				foreach (Connection inConnection in currentState.inConnections)
					inConnection.connectionState = NodeStates.Resting;
				///
			}

			lastState = currentState;
			currentState = state;
			currentState.Execute(agent, blackboard);
		}

		///Trigger a state to enter by it's name
		public void TriggerState(string stateName){

			foreach (Node node in allNodes){
				if (node.allowAsPrime && (node as FSMState).stateName == stateName ){
					EnterState(node as FSMState);
					return;
				}
			}

			Debug.LogWarning("No State with name '" + stateName + "' found on FSM '" + graphName + "'");
		}

		///Get all State Names. Un-named states are not included.
		public List<string> GetStateNames(){

			var names = new List<string>();
			foreach(FSMState node in allNodes){
				if (node.allowAsPrime && !string.IsNullOrEmpty(node.stateName))
					names.Add(node.stateName);
			}
			return names;
		}
	}
}