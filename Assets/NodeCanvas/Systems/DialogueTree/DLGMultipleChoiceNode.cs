#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NodeCanvas.DialogueTrees{

	[AddComponentMenu("")]
	[Name("✫Multiple Choice")]
	[Description("This node will prompt a Dialogue Choice. A choice will be available if the connection's condition is true or there is no condition on that connection. The Actor selected here is only used for the Condition checks as well as will Say the selection if the option is checked.")]
	public class DLGMultipleChoiceNode : DLGNodeBase{

		//wraped into a Choice class for possible future functionality
		[System.Serializable]
		public class Choice{

			public bool isUnfolded = true;
			public Statement statement;

			public Choice(Statement statement){
				this.statement = statement;
			}
		}

		public float availableTime = 0;
		public bool saySelection = false;

		[SerializeField] [HideInInspector]
		private List<Choice> possibleOptions = new List<Choice>();
		private bool isWaitingChoice = false;


		public override string nodeName{
			get{return base.nodeName + " " + finalActorName;}
		}

		public override int maxOutConnections{
			get{return -1;}
		}

		public override System.Type outConnectionType{
			get{return typeof(ConditionalConnection);}
		}


		protected override NodeStates OnExecute(){

			if (outConnections.Count == 0){
				DLGTree.StopGraph();
				return Error("There are no connections to the Multiple Choice Node!");
			}

			if (!finalActor){
				DLGTree.StopGraph();
				return Error("Actor not found");
			}

			DLGTree.currentNode = this;

			Dictionary<Statement, int> finalOptions = new Dictionary<Statement, int>();
			for (int i= 0; i < outConnections.Count; i++){
				if ( (outConnections[i] as ConditionalConnection).CheckCondition(finalActor, finalBlackboard)){
					var finalStatement = possibleOptions[i].statement.BlackboardReplace(finalBlackboard);
					finalOptions[finalStatement] = i;
				}
			}

			if (finalOptions.Count == 0){
				Debug.Log("Multiple Choice Node has no available options. Dialogue Ends");
				DLGTree.StopGraph();
				return NodeStates.Failure;
			}

			if (availableTime > 0)
				StartCoroutine(CountDown());

			EventHandler.Dispatch(DLGEvents.OnDialogueOptions, new DialogueOptionsInfo(finalOptions, availableTime, OnOptionSelected));

			return NodeStates.Running;
		}

		private IEnumerator CountDown(){

			isWaitingChoice = true;
			float timer = 0;
			while (timer < availableTime){
				
				if (!DLGTree.isRunning)
					yield break;

				if (!isWaitingChoice)
					yield break;

				timer += Time.deltaTime;
				yield return null;
			}

			for (int i= outConnections.Count - 1; i >= 0; i--){
				ConditionalConnection connection= outConnections[i] as ConditionalConnection;
				if (connection.CheckCondition(finalActor, finalBlackboard)){
					OnOptionSelected(i);
					yield break;
				}
			}
		}

		private void OnOptionSelected(int index){

			nodeState = NodeStates.Success;
			if (!DLGTree.isRunning)
				return;

			isWaitingChoice = false;
			System.Action Finalize = delegate {outConnections[index].Execute(finalActor, finalBlackboard); };

			if (saySelection && finalActor != null){
				var finalStatement = possibleOptions[index].statement.BlackboardReplace(finalBlackboard);
				finalActor.Say(finalStatement, Finalize);
			} else {
				Finalize();
			}
		}

		public override void OnPortConnected(int index){

			if (possibleOptions.Count < outConnections.Count)
				possibleOptions.Insert(index, new Choice(new Statement("...")));
		}

		public override void OnPortDisconnected(int index){

		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			if (outConnections.Count == 0){
				GUILayout.Label("Connect Possible Outcomes");
				return;
			}

			for (int i= 0; i < outConnections.Count; i++){

				GUILayout.BeginHorizontal("box");
				GUILayout.Label("#" + outConnections[i].targetNode.ID.ToString() + ") " + possibleOptions[i].statement.text );
				GUILayout.EndHorizontal();
			}

			GUILayout.BeginHorizontal();
			if (availableTime > 0)
				GUILayout.Label("Choose in '" + availableTime + "' seconds");
			if (saySelection)
				GUILayout.Label("Say Selection");
			GUILayout.EndHorizontal();
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();

			if (outConnections.Count == 0){
				EditorUtils.CoolLabel("No Choices");
				return;
			}

			EditorUtils.CoolLabel("Possible Choices");

			var e = Event.current;

			for (int i= 0; i < outConnections.Count; i++){

				GUILayout.BeginHorizontal("box");

					string arrow = possibleOptions[i].isUnfolded? "▼ " : "► ";

					ConditionalConnection connection= outConnections[i] as ConditionalConnection;
					if (connection.condition){
						GUILayout.Label(arrow + connection.condition.taskInfo);
					} else {
						GUILayout.Label(arrow + "Always");
					}

					Rect titleRect = GUILayoutUtility.GetLastRect();
					if (e.type == EventType.MouseUp && titleRect.Contains(e.mousePosition)){
						possibleOptions[i].isUnfolded = !possibleOptions[i].isUnfolded;
						e.Use();
					}
					
					if (GUILayout.Button(">", GUILayout.Width(20)))
						Graph.currentSelection = connection;

				GUILayout.EndHorizontal();

				if (!possibleOptions[i].isUnfolded)
					continue;

				GUILayout.BeginVertical("box");

					possibleOptions[i].statement.text = EditorGUILayout.TextField(possibleOptions[i].statement.text);
					possibleOptions[i].statement.audio = EditorGUILayout.ObjectField("Audio File", possibleOptions[i].statement.audio, typeof(AudioClip), false) as AudioClip;
					possibleOptions[i].statement.meta = EditorGUILayout.TextField("Meta Data", possibleOptions[i].statement.meta);
					EditorGUILayout.Space();

				GUILayout.EndVertical();
				GUILayout.Space(10);
			}

			availableTime = EditorGUILayout.Slider("Available Time", availableTime, 0, 20);
			saySelection = EditorGUILayout.Toggle("Say Selection", saySelection);
		}

		#endif
	}
}