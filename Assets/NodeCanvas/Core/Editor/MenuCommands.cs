using UnityEditor;
using UnityEngine;
using NodeCanvas;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.StateMachines;
using NodeCanvas.DialogueTrees;

namespace NodeCanvasEditor{

	public class MenuCommands {

		[MenuItem("NC/Create BehaviourTree")]
		public static void CreateBehaviourTree(){
			BehaviourTree newBT = new GameObject("BehaviourTree").AddComponent(typeof(BehaviourTree)) as BehaviourTree;
			Selection.activeObject = newBT;
		}

		[MenuItem("NC/Create FSM")]
		public static void CreateFSM(){

			FSM newFSM= new GameObject("FSM").AddComponent(typeof(FSM)) as FSM;
			Selection.activeObject = newFSM;
		}

		[MenuItem("NC/Create Dialogue Tree")]
		public static void CreateDialogueTree(){
			DialogueTree newDLG = new GameObject("DialogueTree").AddComponent(typeof(DialogueTree)) as DialogueTree;
			Selection.activeObject = newDLG;
		}
	}
}