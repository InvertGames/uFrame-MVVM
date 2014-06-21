#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using NodeCanvas.BehaviourTrees;

namespace NodeCanvas.StateMachines{

	[AddComponentMenu("")]
	[Name("Nested Behavior Tree")]
	[Category("Nested")]
	[Description("This will execute a Behaviour Tree graph on Enter. On Exit, the Behavior Tree graph will be stoped. You can optionaly specify a Success Event and a Failure Event which will be sent when the BT's root node state returns either of the 2. Use alongside with a CheckEvent on Transition.")]
	public class FSMNestedBTState : FSMState, INestedNode{

		private enum ExecutionMode {RunOnce, RunForever}
		[SerializeField]
		private ExecutionMode executionMode = ExecutionMode.RunForever;
		[SerializeField]
		private float updateInterval = 0f;
		[SerializeField]
		private BehaviourTree _nestedBT;
		[SerializeField]
		private string successEvent;
		[SerializeField]
		private string failureEvent;
		
		private bool instanceChecked;
		private bool BTIsFinished;

		private BehaviourTree nestedBT{
			get {return _nestedBT;}
			set {_nestedBT = value;}
		}

		[HideInInspector]
		public Graph nestedGraph{
			get {return nestedBT;}
			set {nestedBT = (BehaviourTree)value;}
		}

		public override string nodeName{
			get{return string.IsNullOrEmpty(stateName)? "BEHAVIOUR" : stateName;}
		}

		protected override void Enter(){

			if (!nestedBT){
				Finish(false);
				return;
			}

			CheckInstance();

			BTIsFinished = false;
			nestedBT.runForever = (executionMode == ExecutionMode.RunForever);
			nestedBT.updateInterval = updateInterval;
			nestedBT.StartGraph(graphAgent, graphBlackboard, delegate{BTIsFinished = true;});
		}

		protected override void Stay(){

			if (!string.IsNullOrEmpty(successEvent) && nestedBT.rootState == NodeStates.Success)
				SendEvent(successEvent);

			if (!string.IsNullOrEmpty(failureEvent) && nestedBT.rootState == NodeStates.Failure)
				SendEvent(failureEvent);
			
			if (BTIsFinished)
				Finish();
		}

		protected override void Exit(){
			if (nestedBT && nestedBT.isRunning)
				nestedBT.StopGraph();
		}

		protected override void Pause(){
			if (nestedBT)
				nestedBT.PauseGraph();
		}

		private void CheckInstance(){

			if (!instanceChecked && nestedBT != null && nestedBT.transform.parent != graph.transform){
				nestedBT = (BehaviourTree)Instantiate(nestedBT, transform.position, transform.rotation);
				nestedBT.transform.parent = graph.transform;
				instanceChecked = true;
			}
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){
			
			base.OnNodeGUI();
			if (nestedBT){
			
				GUILayout.Label(executionMode.ToString());
				if (GUILayout.Button("EDIT"))
					graph.nestedGraphView = nestedBT;

			} else {

				if (GUILayout.Button("CREATE NEW"))
					nestedBT = (BehaviourTree)Graph.CreateNested(this, typeof(BehaviourTree), "Nested BT");
			}
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();
			nestedBT = EditorGUILayout.ObjectField("Behaviour Tree", nestedBT, typeof(BehaviourTree), true) as BehaviourTree;

			if (nestedBT == null)
				return;

			executionMode = (ExecutionMode)EditorGUILayout.EnumPopup("Execution Mode", executionMode);
			
			if (executionMode == ExecutionMode.RunForever)
				updateInterval = EditorGUILayout.FloatField("Update Interval", updateInterval);

			var alpha1 = string.IsNullOrEmpty(successEvent)? 0.5f : 1;
			var alpha2 = string.IsNullOrEmpty(failureEvent)? 0.5f : 1;
			GUILayout.BeginVertical("box");
			GUI.color = new Color(1,1,1,alpha1);
			successEvent = EditorGUILayout.TextField("Success Event", successEvent);
			GUI.color = new Color(1,1,1,alpha2);
			failureEvent = EditorGUILayout.TextField("Failure Event", failureEvent);
			GUILayout.EndVertical();
			GUI.color = Color.white;

			nestedBT.graphName = nodeName;
		}

		#endif
	}
}