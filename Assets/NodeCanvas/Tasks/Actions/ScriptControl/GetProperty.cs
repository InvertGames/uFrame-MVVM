#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Script Control")]
	[Description("Get a property of a script and save it to the blackboard")]
	[AgentType(typeof(Transform))]
	public class GetProperty : ActionTask {

		public BBVariableSet saveAs = new BBVariableSet{blackboardOnly = true};

		[SerializeField]
		private string methodName;
		[SerializeField]
		private string scriptName;

		private Component script;
		private MethodInfo method;

		protected override string info{
			get
			{
				if (string.IsNullOrEmpty(methodName))
					return "No Property Selected";

				return (saveAs.selectedBBVariable + " = " + agentInfo + "." + methodName);
			}
		}

		//store the method info on init for performance
		protected override string OnInit(){
			script = agent.GetComponent(scriptName);
			if (script == null)
				return "Missing Component '" + scriptName + "' on Agent '" + agent.gameObject.name + "' . Did the agent changed at runtime?";
			method = script.GetType().GetMethod(methodName, System.Type.EmptyTypes);
			return null;
		}

		//do it by invoking method
		protected override void OnExecute(){
			
			if (method != null){
				
				saveAs.selectedObjectValue = method.Invoke(script, null);
				EndAction(true);
			
			} else {

				EndAction(false);
			}
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnTaskInspectorGUI(){

			if (agent == null){
				EditorGUILayout.HelpBox("This Action needs the Agent to be known. Currently the Agent is unknown.\nConsider overriding the Agent or using SendMessage instead.", MessageType.Error);
				return;
			}

			if (agent.GetComponent(scriptName) == null){
				scriptName = null;
				methodName = null;
				saveAs.selectedType = null;
			}

			if (GUILayout.Button("Select Property")){

				EditorUtils.ShowMethodSelectionMenu(agent.gameObject, saveAs.availableTypes, new List<System.Type>(), delegate(MethodInfo method){
					scriptName = method.ReflectedType.Name;
					methodName = method.Name;
					saveAs.selectedType = method.ReturnType;
					if (Application.isPlaying)
						OnInit();
				}, true );
			}

			if (!string.IsNullOrEmpty(methodName)){
				GUILayout.BeginVertical("box");
				EditorGUILayout.LabelField("Selected Component", scriptName);
				EditorGUILayout.LabelField("Selected Property", methodName);
				EditorGUILayout.LabelField("Property Type", EditorUtils.TypeName(saveAs.selectedType) );
				GUILayout.EndVertical();
			}

			if (saveAs.selectedType != null)
				EditorUtils.BBVariableField("Save As", saveAs.selectedBBVariable);
		}

		#endif
	}
}