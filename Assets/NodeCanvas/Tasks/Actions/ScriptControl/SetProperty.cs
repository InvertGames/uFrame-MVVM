#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Script Control")]
	[Description("Set a property on a script")]
	[AgentType(typeof(Transform))]
	public class SetProperty : ActionTask {

		public BBVariableSet setValue = new BBVariableSet();

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

				return agentInfo + "." + methodName + " = " + setValue.selectedBBVariable;
			}
		}

		//store the method info on init for performance
		protected override string OnInit(){
			script = agent.GetComponent(scriptName);
			if (script == null)
				return "Missing Component '" + scriptName + "' on Agent '" + agent.gameObject.name + "' . Did the agent changed at runtime?";
			method = script.GetType().GetMethod(methodName, new System.Type[]{setValue.selectedType});
			return null;
		}

		//do it by invoking method
		protected override void OnExecute(){
			
			if (method != null){
				method.Invoke(script, new object[]{setValue.selectedObjectValue} );
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
				setValue.selectedType = null;
			}

			if (GUILayout.Button("Select Property")){
				EditorUtils.ShowMethodSelectionMenu(agent.gameObject, new List<System.Type>{typeof(void)}, setValue.availableTypes, delegate(MethodInfo method){
					scriptName = method.ReflectedType.Name;
					methodName = method.Name;
					setValue.selectedType = method.GetParameters()[0].ParameterType;
					if (Application.isPlaying)
						OnInit();
				}, true);
			}

			if (!string.IsNullOrEmpty(methodName)){
				GUILayout.BeginVertical("box");
				EditorGUILayout.LabelField("Selected Component", scriptName);
				EditorGUILayout.LabelField("Selected Property", methodName);
				EditorGUILayout.LabelField("Set Type", EditorUtils.TypeName(setValue.selectedType) );
				GUILayout.EndVertical();
			}

			if (setValue.selectedType != null)
				EditorUtils.BBVariableField("Set Value", setValue.selectedBBVariable);
		}

		#endif
	}
}