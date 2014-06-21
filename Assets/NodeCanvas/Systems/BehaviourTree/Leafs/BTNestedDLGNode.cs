using UnityEngine;
using NodeCanvas.DialogueTrees;

namespace NodeCanvas.BehaviourTrees{

	[AddComponentMenu("")]
	[Name("NestedDialogue")]
	[Category("Nested")]
	[Description("Will Execute the nested dialogue assigned and return Success or Failure depending on the Dialogue Tree EndState.\nBy default a Dialogue Tree ends in Success.")]
	[Icon("Dialogue")]
	public class BTNestedDLGNode : BTNodeBase, INestedNode {

		[SerializeField]
		private DialogueTree _nestedDLG;

		private DialogueTree nestedDLG{
			get {return _nestedDLG;}
			set
			{
				_nestedDLG = value;
				if (_nestedDLG != null){
					_nestedDLG.agent = graphAgent;
					_nestedDLG.blackboard = graphBlackboard;
				}
			}
		}

		public Graph nestedGraph{
			get {return nestedDLG;}
			set {nestedDLG = (DialogueTree)value;}
		}

		public override string nodeName{
			get {return "DIALOGUE";}
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (nestedDLG == null || nestedDLG.primeNode == null)
				return NodeStates.Failure;

			if (nodeState == NodeStates.Resting || nestedDLG.isPaused){
				nodeState = NodeStates.Running;
				nestedDLG.StartGraph(agent, blackboard, OnDLGFinished);
			}

			return nodeState;
		}

		private void OnDLGFinished(){
			if (nodeState == NodeStates.Running)
				nodeState = (NodeStates)nestedDLG.endState;
		}

		protected override void OnReset(){
			if (nestedDLG)
				nestedDLG.StopGraph();
		}

		public override void OnGraphPaused(){
			if (nestedDLG)
				nestedDLG.PauseGraph();
		}


		////////////////////////////
		//////EDITOR AND GUI////////
		////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){

		    if (nestedDLG){

		    	GUILayout.Label("'" + nestedDLG.graphName + "'");
			    	
		    	if (GUILayout.Button("EDIT"))
		    		graph.nestedGraphView = nestedDLG;

			} else {
				
				if (GUILayout.Button("CREATE NEW")){
					nestedDLG = (DialogueTree)Graph.CreateNested(this, typeof(DialogueTree), "Dialogue Tree");
					nestedDLG.transform.parent = null;
					if (graphAgent != null && graphAgent.GetComponent<DialogueActor>() == null){
						if (UnityEditor.EditorUtility.DisplayDialog("Nested Dialogue Node", "The current agent doesn't have a DialogueActor component. Add one?", "Yes", "No")){
							var newActor = graphAgent.gameObject.AddComponent<DialogueActor>();
							UnityEditor.Undo.RegisterCreatedObjectUndo(newActor, "Created Dialogue Actor");
							newActor.blackboard = graphBlackboard;
						}
					}
				}
			}
		}

		protected override void OnNodeInspectorGUI(){
			
			nestedDLG = UnityEditor.EditorGUILayout.ObjectField("Dialogue Tree", nestedDLG, typeof(DialogueTree), true) as DialogueTree;

			if (nestedDLG != null)
		    	nestedDLG.graphName = UnityEditor.EditorGUILayout.TextField("Name", nestedDLG.graphName);
		}

		#endif
	}
}