using UnityEngine;
using System.Collections;

//Every system must derive Graph. Here are some important inherited properties:
//agent 			the agent of the graph
//blackboard 		the blackboard of the graph
//isRunning 		is the graph currently running
//isPaused 			is the graph currently paused
//primeNode 		the Start Node of this graph
//allNodes 			a list of all the nodes in this graph

namespace NodeCanvas.MySystem{

	public class MySystem : Graph{

		//Return the base node type
		public override System.Type baseNodeType{
			get {return typeof(MySystemNode);}
		}

		//This is called when the graph starts
		protected override void OnGraphStarted(){
			
		}

		//This is called every frame while the graph is running
		protected override void OnGraphUpdate(){

		}

		//Called once the graph stops
		protected override void OnGraphStoped(){

		}

		//Called when the graph is paused
		protected override void OnGraphPaused(){

		}
	}
}