using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace NodeCanvas.DialogueTrees{

	[AddComponentMenu("")]
	///The base node for all Dialogue Tree system nodes.
	abstract public class DLGNodeBase : Node, ITaskSystem{

		[SerializeField]
		private string _actorName = "_Owner";

		public override string nodeName{
			get{return "#" + ID;}
		}

		public override int maxInConnections{
			get{return -1;}
		}

		public override int maxOutConnections{
			get{return 1;}
		}

		public override System.Type outConnectionType{
			get{return typeof(Connection);}
		}

		private string actorName{
			get
			{
				return _actorName;
			}
			set
			{
				_actorName = value;
				DLGTree.actorReferences[value] = DialogueActor.FindActorWithName(value);
				foreach (Task task in GetComponentsInChildren<Task>(true))
					task.SetOwnerSystem(this);
			}
		}

		protected DialogueTree DLGTree{
			get{return (DialogueTree)graph;}
		}

		private List<string> actorNames{
			get
			{
				List<string> names = new List<string>(DLGTree.dialogueActorNames);
				names.Insert(0, "_Owner");
				return names;
			}
		}

		///The actor name that will execute the node
		protected string finalActorName{
			get
			{
				if (!actorNames.Contains(actorName))
					return "<color=#d63e3e>*" + actorName + "*</color>";
				return actorName;
			}
		}

		///The DialogueActor that will execute the node
		protected DialogueActor finalActor{
			get
			{
				if (actorName == "_Owner" || string.IsNullOrEmpty(actorName))
					return graphAgent == null? null : graphAgent.GetComponent<DialogueActor>();

				if (!DLGTree.actorReferences.ContainsKey(actorName))
					DLGTree.actorReferences[actorName] = DialogueActor.FindActorWithName(actorName);

				return DLGTree.actorReferences[actorName];
			}
		}

		///The Blackboard that will be used when executing the node, taken from the finalActor that will be used
		protected Blackboard finalBlackboard{
			get {return finalActor == null? null : finalActor.blackboard;}
		}

		//Interface implementation. Returns finalActor
		public Component agent{
			get{return finalActor;}
		}

		//Interface implementation. Returns finalBlackbaord
		public Blackboard blackboard{
			get{return finalBlackboard;}
		}

		//Interface implementation
		public void SendTaskOwnerDefaults(){
			foreach (Task task in GetComponentsInChildren<Task>(true))
				task.SetOwnerSystem(this);
		}

		protected void Continue(){

			nodeState = NodeStates.Success;
			if (!DLGTree.isRunning)
				return;

			if (outConnections.Count == 0){
				DLGTree.StopGraph();
				return;
			}

			outConnections[0].Execute(finalActor, finalBlackboard);
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeReleased(){
			SortConnectionsByPositionX();
		}

		protected override void OnNodeGUI(){

		}

		protected override void OnNodeInspectorGUI(){

			GUI.backgroundColor = EditorUtils.lightBlue;
			actorName = EditorUtils.StringPopup(actorName, actorNames, false, false);
			GUI.backgroundColor = Color.white;
			
			if (graphAgent != null && actorName == "_Owner" && graphAgent.GetComponent<DialogueActor>() == null){

				GUILayout.BeginVertical("box");
				UnityEditor.EditorGUILayout.HelpBox("Dialogue Tree has an agent assigned, but it's game object has no DialogueActor component to be used as Owner for this node.", UnityEditor.MessageType.Warning);
				if (GUILayout.Button("Add Dialogue Actor")){
					var newActor = graphAgent.gameObject.AddComponent<DialogueActor>();
					UnityEditor.Undo.RegisterCreatedObjectUndo(newActor, "New Actor");
					newActor.blackboard = graphAgent.GetComponent<Blackboard>();
				}

				GUILayout.EndVertical();
			}

			if (finalActor != null){

				if (graph.blackboard != finalActor.blackboard)
					graph.blackboard = finalActor.blackboard;

			} else {

				graph.blackboard = null;
			}
		}
		
		#endif
	}
}