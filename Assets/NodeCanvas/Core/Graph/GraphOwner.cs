using UnityEngine;
using System.Collections.Generic;
using System;

namespace NodeCanvas{

	///The base class where BehaviourTreeOwner and FSMOwner derive from. GraphOwners simply wrap the execution of a Graph and act as a front-end to the user.
	abstract public class GraphOwner : MonoBehaviour {

		public enum EnableAction{
			StartBehaviour,
			DoNothing
		}

		public enum DisableAction{
			StopBehaviour,
			PauseBehaviour,
			DoNothing
		}

		public class GraphEntry{
			public Graph graph;
			public string name;
		}

		[System.Obsolete("Use 'onEnable = EnableAction.StartBehaviour")]
		public bool executeOnStart{
			get {return onEnable == EnableAction.StartBehaviour;}
			set {onEnable = EnableAction.StartBehaviour;}
		}

		///What will happen OnEnable
		public EnableAction onEnable = EnableAction.StartBehaviour;
		///What will happen OnDisable
		public DisableAction onDisable = DisableAction.StopBehaviour;

		[SerializeField]
		private Blackboard _blackboard;
		private Dictionary<Graph, Graph> instances = new Dictionary<Graph, Graph>();
		private static bool isQuiting;

		///Is the assigned graph currently running?
		public bool isRunning{
			get {return graph != null? graph.isRunning : false;}
		}

		///Is the assigned graph currently paused?
		public bool isPaused{
			get {return graph != null? graph.isPaused : false;}
		}

		///The blackboard that the assigned graph will be Started with
		public Blackboard blackboard{
			get {return _blackboard;}
			set {_blackboard = value; if (graph != null) graph.blackboard = value;}
		}

		abstract public Graph graph{ get; set; }
		abstract public System.Type graphType{ get; }

		///Start the graph assigned
		public void StartGraph(){
			
			graph = GetInstance(graph);
			if (graph != null)
				graph.StartGraph(this, blackboard);
		}

		///Start the graph assigned providing a callback for when it ends
		public void StartGraph(Action callback){

			graph = GetInstance(graph);
			if (graph != null)
				graph.StartGraph(this, blackboard, callback);
		}

		///Start a new graph on this owner
		public void StartGraph(Graph newGraph){
			SwitchGraph(newGraph);
		}

		///Stop the graph assigned
		public void StopGraph(){
			if (graph != null)
				graph.StopGraph();
		}

		///Pause the assigned Behaviour. Same as Graph.PauseGraph
		public void PauseGraph(){
			if (graph != null)
				graph.PauseGraph();
		}

		///Use to switch or set graphs at runtime.
		public void SwitchGraph(Graph newGraph){
			
			if (newGraph.GetType() != graphType){
				Debug.LogWarning("Incompatible graph types." + this.GetType().Name + " can be assigned graphs of type " + graphType.Name);
				return;
			}

			StopGraph();
			graph = newGraph;
			StartGraph();
		}

		///Send an event through the graph (To be used with CheckEvent for example). Same as Graph.SendEvent
		public void SendEvent(string eventName){
			if (graph != null)
				graph.SendEvent(eventName);
		}

		///Thats the same as calling the static Graph.SendGlobalEvent function
		public void SendGlobalEvent(string eventName){
			Graph.SendGlobalEvent(eventName);
		}

		new public void SendMessage(string name){
			SendMessage(name, null);
		}

		///Sends a message to all tasks in the graph as well as this gameobject as usual. Same as Graph.SendMessage
		new public void SendMessage(string name, object arg){
			if (graph != null)
				graph.SendMessage(name, arg);
		}

		public void SendTaskMessage(string name){
			SendTaskMessage(name, null);
		}

		///Sends a message to all Task of the assigned graph. Same as Graph.SendTaskMessage
		public void SendTaskMessage(string name, object arg){
			if (graph != null)
				graph.SendTaskMessage(name, arg);
		}

		///Gets the instance graph for this owner of the provided graph
		Graph GetInstance(Graph originalGraph){

			if (!Application.isPlaying)
				return originalGraph;

			if (originalGraph == null)
				return null;

			Graph instance;

			//it means that the graph is not used as template
			if (originalGraph.transform.parent == this.transform){
			
				instance = originalGraph;
			
			} else {

				if (instances.ContainsKey(originalGraph)){
					instance = instances[graph];

				} else {

					instance = (Graph)Instantiate(originalGraph, transform.position, transform.rotation);
					instance.transform.parent = this.transform; //organization
					instances[originalGraph] = instance;
				}
			}

			instance.gameObject.hideFlags = Graph.doHide? HideFlags.HideInHierarchy : 0;
			return instance;
		}

		protected void OnEnable(){
			
			if (onEnable == EnableAction.StartBehaviour)
				StartGraph();
		}

		protected void OnDisable(){

			if (isQuiting)
				return;

			if (onDisable == DisableAction.StopBehaviour)
				StopGraph();

			if (onDisable == DisableAction.PauseBehaviour)
				PauseGraph();
		}

		protected void OnApplicationQuit(){
			isQuiting = true;
		}

		protected void Reset(){

			blackboard = gameObject.GetComponent<Blackboard>();
			if (blackboard == null)
				blackboard = gameObject.AddComponent<Blackboard>();		
		}

		protected void OnDrawGizmos(){
			Gizmos.DrawIcon(transform.position, "GraphOwner.png", true);
		}
	}
}