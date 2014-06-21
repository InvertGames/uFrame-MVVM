using UnityEngine;
using System.Collections;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	[Name("NestedBT")]
	[Category("Nested")]
	[Description("Nested Tree can be assigned an entire Behavior Tree graph. The prime node of that graph will be considered child node of this node and will return whatever the child returns")]
	[Icon("BT")]
	public class BTNestedBTNode : BTNodeBase, INestedNode{

		[SerializeField]
		private BehaviourTree _nestedTree;
		private bool instanceChecked;

		private BehaviourTree nestedTree{
			get {return _nestedTree;}
			set
			{
				_nestedTree = value;
				if (_nestedTree != null){
					_nestedTree.agent = graphAgent;
					_nestedTree.blackboard = graphBlackboard;
				}
			}
		}

		public Graph nestedGraph{
			get {return nestedTree;}
			set {nestedTree = (BehaviourTree)value;}
		}

		public override string nodeName{
			get {return "BEHAVIOUR";}
		}

		/////////
		/////////

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			CheckInstance();

			if (nestedTree && nestedTree.primeNode)
				return nestedTree.primeNode.Execute(agent, blackboard);

			return NodeStates.Success;
		}

		protected override void OnReset(){

			if (nestedTree && nestedTree.primeNode)
				nestedTree.primeNode.ResetNode();
		}

		public override void OnGraphStarted(){
			if (nestedTree){
				foreach(Node node in nestedTree.allNodes)
					node.OnGraphStarted();				
			}
		}

		public override void OnGraphStoped(){
			if (nestedTree){
				foreach(Node node in nestedTree.allNodes)
					node.OnGraphStoped();				
			}			
		}

		public override void OnGraphPaused(){
			if (nestedTree){
				foreach(Node node in nestedTree.allNodes)
					node.OnGraphPaused();
			}
		}

		private void CheckInstance(){

			if (!instanceChecked && nestedTree != null && nestedTree.transform.parent != graph.transform){
				nestedTree = (BehaviourTree)Instantiate(nestedTree, transform.position, transform.rotation);
				nestedTree.transform.parent = graph.transform;
				instanceChecked = true;	
			}
		}

		////////////////////////////
		//////EDITOR AND GUI////////
		////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){
		    
		    if (nestedTree){

		    	GUILayout.Label("'" + nestedTree.graphName + "'");
		    	if (GUILayout.Button("EDIT"))
		    		graph.nestedGraphView = nestedTree;

			} else {
				
				if (GUILayout.Button("CREATE NEW"))
					nestedTree = (BehaviourTree)Graph.CreateNested(this, typeof(BehaviourTree), "Nested BT");
			}
		}

		protected override void OnNodeInspectorGUI(){

		    nestedTree = UnityEditor.EditorGUILayout.ObjectField("Behaviour Tree", nestedTree, typeof(BehaviourTree), true) as BehaviourTree;
	    	if (nestedTree == this.graph){
		    	Debug.LogWarning("You can't have a Graph nested to iteself! Please select another");
		    	nestedTree = null;
		    }

		    if (nestedTree != null)
		    	nestedTree.graphName = UnityEditor.EditorGUILayout.TextField("Name", nestedTree.graphName);
		}

		#endif
	}
}