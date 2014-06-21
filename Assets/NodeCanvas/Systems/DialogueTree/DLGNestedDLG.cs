#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.DialogueTrees{

	[AddComponentMenu("")]
	[Name("Nested Dialogue")]
	[Category("Nested")]
	[Description("The Nested Dialogue Tree will execute. When that nested Dialogue Tree is finished, this node will continue instead, if it has a connection. Useful for making reusable and contained Dialogue Trees.")]
	[Icon("Dialogue")]
	public class DLGNestedDLG : DLGNodeBase, INestedNode{

		[SerializeField]
		private DialogueTree _nestedDLG;

		private DialogueTree nestedDLG{
			get {return _nestedDLG;}
			set {_nestedDLG = value;}
		}

		public Graph nestedGraph{
			get {return nestedDLG;}
			set {nestedDLG = (DialogueTree)value;}
		}

		public override string nodeName{
			get {return "#" + ID + " Nested Dialogue";}
		}

		protected override NodeStates OnExecute(){

			if (!nestedDLG){
				DLGTree.StopGraph();
				return Error("No Nested Dialogue Tree assigned!");
			}


			DLGTree.currentNode = this;

			CopyActors();

			nestedDLG.StartGraph(graphAgent, graphBlackboard, Continue );
			return NodeStates.Running;
		}

		public override void OnGraphStoped(){

			if (nestedDLG)
				nestedDLG.StopGraph();
		}

		public override void OnGraphPaused(){

			if (nestedDLG)
				nestedDLG.PauseGraph();
		}

		private void CopyActors(){
			foreach (string actorName in this.DLGTree.dialogueActorNames){
				if (!nestedDLG.dialogueActorNames.Contains(actorName))
					nestedDLG.dialogueActorNames.Add(actorName);
			}
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			if (nestedDLG){

				GUILayout.Label(nestedDLG.graphName);

				if (GUILayout.Button("EDIT"))
					DLGTree.nestedGraphView = nestedDLG;
			
			} else {

				if (GUILayout.Button("CREATE")){
					nestedDLG = (DialogueTree)Graph.CreateNested(this, typeof(DialogueTree), "Nested Dialogue");
					CopyActors();
				}
			}
		}

		protected override void OnNodeInspectorGUI(){

			nestedDLG = EditorGUILayout.ObjectField("Nested Dialogue Tree", nestedDLG, typeof(DialogueTree), true) as DialogueTree;
			if (nestedDLG == DLGTree){
				Debug.LogWarning("Nested DialogueTree can't be itself! Please select another");
				nestedDLG = null;
			}
		}

		#endif
	}
}