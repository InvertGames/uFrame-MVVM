#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using NodeCanvas.DialogueTrees;

namespace NodeCanvas.StateMachines{

	[AddComponentMenu("")]
	[Name("Nested Dialogue")]
	[Category("Nested")]
	[Description("Will execute the assigned dialogue tree on Enter and stop it on Exit. Optionaly an event can be send for whether the dialogue ended in Success or Failure.\nThis can be controled by using the 'End' Dialogue Node inside the Dialogue Tree.\nUse a 'CheckEvent' condition to make use of those events.")]
	public class FSMNestedDLGState : FSMState, INestedNode{

		[SerializeField]
		private DialogueTree _nestedDLG;
		[SerializeField]
		private string successEvent;
		[SerializeField]
		private string failureEvent;

		private DialogueTree nestedDLG{
			get {return _nestedDLG;}
			set {_nestedDLG = value;}
		}

		[HideInInspector]
		public Graph nestedGraph{
			get {return nestedDLG;}
			set {nestedDLG = (DialogueTree)value;}
		}

		public override string nodeName{
			get {return string.IsNullOrEmpty(stateName)? "DIALOGUE" : stateName;}
		}

		protected override void Enter(){

			if (!nestedDLG){
				Finish(false);
				return;
			}

			nestedDLG.StartGraph(graphAgent, graphBlackboard, OnDialogueFinished);
		}

		void OnDialogueFinished(){

			if (!string.IsNullOrEmpty(successEvent) && nestedDLG.endState == DialogueTree.EndState.Success)
				SendEvent(successEvent);

			if (!string.IsNullOrEmpty(failureEvent) && nestedDLG.endState == DialogueTree.EndState.Failure)
				SendEvent(failureEvent);

			Finish();
		}

		protected override void Exit(){
			if (nestedDLG && nestedDLG.isRunning)
				nestedDLG.StopGraph();
		}

		protected override void Pause(){
			if (nestedDLG)
				nestedDLG.PauseGraph();
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			base.OnNodeGUI();
			if (nestedDLG){
				
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

			base.OnNodeInspectorGUI();
			nestedDLG = EditorGUILayout.ObjectField("Dialogue Tree", nestedDLG, typeof(DialogueTree), true) as DialogueTree;

			if (nestedDLG == null)
				return;

			var alpha1 = string.IsNullOrEmpty(successEvent)? 0.5f : 1;
			var alpha2 = string.IsNullOrEmpty(failureEvent)? 0.5f : 1;
			GUILayout.BeginVertical("box");
			GUI.color = new Color(1,1,1,alpha1);
			successEvent = EditorGUILayout.TextField("Success Event", successEvent);
			GUI.color = new Color(1,1,1,alpha2);
			failureEvent = EditorGUILayout.TextField("Failure Event", failureEvent);
			GUILayout.EndVertical();
			GUI.color = Color.white;

			nestedDLG.graphName = nodeName;

			string names = string.Empty;
			foreach (string actorName in nestedDLG.dialogueActorNames)
				names += "\n" + actorName;
			GUILayout.Label("Dialogue Actors Defined:\n" + (string.IsNullOrEmpty(names)? "NONE" : names) );
		}
		
		#endif
	}
}