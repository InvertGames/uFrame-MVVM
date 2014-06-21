#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas.DialogueTrees{

	[AddComponentMenu("")]
	///A Dialogue Tree container
	public class DialogueTree : Graph{

		public enum EndState
		{
			Failure = 0,
			Success = 1,
			None    = 3
		}

		public EndState endState = EndState.None;

		[SerializeField]
		private List<string> _dialogueActorNames = new List<string>();
		public Dictionary<string, DialogueActor> actorReferences = new Dictionary<string, DialogueActor>();

		private DLGNodeBase _currentNode;


		public DLGNodeBase currentNode{
			get {return _currentNode;}
			set {_currentNode = value;}
		}

		///The actor names that are inputed and available to act
		public List<string> dialogueActorNames{
			get {return _dialogueActorNames;}
			set {dialogueActorNames = value;}
		}

		public override System.Type baseNodeType{
			get {return typeof(DLGNodeBase);}
		}

		protected override bool allowNullAgent{
			get{return true;}
		}

		protected override void OnGraphStarted(){

			if (agent != null){

				var actor = agent.GetComponent<DialogueActor>();
				if (actor == null){
					Debug.Log("Dialogue Agent has no Dialogue Actor. Adding one now...", agent.gameObject);
					actor = agent.gameObject.AddComponent<DialogueActor>();
					actor.blackboard = actor.GetComponent<Blackboard>();
					if (actor.blackboard == null){
						Debug.Log("Dialogue Agent game object has now Blackboard. Adding one now and assigning it to the Dialogue Actor...", agent.gameObject);
						actor.blackboard = actor.gameObject.AddComponent<Blackboard>();
					}
					actor.actorName = actor.gameObject.name;
					agent = actor;
				}
			
			} else {

				Debug.Log("Dialogue Started with null Agent to be used as 'Owner'. A default one will be created now...", this.gameObject);
				var actor = this.gameObject.AddComponent<DialogueActor>();
				actor.blackboard = this.gameObject.AddComponent<Blackboard>();
				actor.actorName = "Default";
				agent = actor;
			}

			if (blackboard == null)
				blackboard = (agent as DialogueActor).blackboard;

			actorReferences.Clear();

			foreach (string actorName in dialogueActorNames)
				actorReferences[actorName] = DialogueActor.FindActorWithName(actorName);

			if (dialogueActorNames.Count != actorReferences.Keys.Count){
				Debug.LogError("Not all Dialogue Actors were found for the Dialogue '" + graphName + "'", gameObject);
				StopGraph();
				return;
			}

			//DLGNodes implement ITaskDefaults to provide defaults for the tasks they contain based on the dialogue actor selected for the node
			//This SendTaskOwnerDefaults is send after the graph's SendTaskOwnerDefaults so in essence it overrides it
			foreach (DLGNodeBase node in allNodes)
				node.SendTaskOwnerDefaults();

			EventHandler.Dispatch(DLGEvents.OnDialogueStarted, this);
			
			currentNode = currentNode != null? currentNode : (DLGNodeBase)primeNode;
			currentNode.Execute();
		}

		protected override void OnGraphStoped(){

			endState = currentNode? (EndState)currentNode.nodeState : EndState.None;

			EventHandler.Dispatch(DLGEvents.OnDialogueFinished, this);
			actorReferences.Clear();
			currentNode = null;
		}

		protected override void OnGraphPaused(){

			EventHandler.Dispatch(DLGEvents.OnDialoguePaused, this);
		}
	}
}