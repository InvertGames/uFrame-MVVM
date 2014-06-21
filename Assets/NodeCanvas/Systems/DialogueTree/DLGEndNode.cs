using UnityEngine;
using System.Collections;

namespace NodeCanvas.DialogueTrees{

	[Name("End")]
	[Description("This node is totaly optional and to be used only if you need to force the dialogue to end in Success or Failure.\nA Dialogue will end anyway if it has reached a node with no out connections.")]
	public class DLGEndNode : DLGNodeBase {

		public enum EndState{
			Failure = 0,
			Success = 1
		}

		public EndState endState = EndState.Success;

		public override string nodeName{
			get {return "END";}
		}

		public override int maxOutConnections{
			get {return 0;}
		}

		protected override NodeStates OnExecute(){

			DLGTree.currentNode = this;
			nodeState = (NodeStates)endState;
			DLGTree.StopGraph();
			return nodeState;
		}


		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){
			GUILayout.Label("<b>" + endState + "</b>");
		}

		protected override void OnNodeInspectorGUI(){
			DrawDefaultInspector();
		}

		#endif
	}
}