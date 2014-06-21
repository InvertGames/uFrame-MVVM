#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Category("✫ Script Control")]
	[Description("Call a boolean function on a script and return whether it returned true or false")]
	[AgentType(typeof(Transform))]
	public class CheckFunction : ConditionTask {

		public BBBool boolCheck = new BBBool{value = true};

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
					return "No Method Selected";

				return agentInfo + "." + methodName + " == " + boolCheck;
			}
		}

		//store the method info on agent set for performance
		protected override string OnInit(){
			script = agent.GetComponent(scriptName);
			if (script == null)
				return "Missing Component '" + scriptName + "' on Agent '" + agent.gameObject.name + "' . Did the agent changed at runtime?";
			method = script.GetType().GetMethod(methodName, System.Type.EmptyTypes);
			return null;
		}

		//do it by invoking method
		protected override bool OnCheck(){

			if (method != null)
				return (bool)method.Invoke(script, null) == boolCheck.value;

			return false;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnTaskInspectorGUI(){

			if (agent == null){
				EditorGUILayout.HelpBox("This Condition needs the Agent to be known. Currently the Agent is unknown.\nConsider overriding the Agent.", MessageType.Error);
				return;
			}

			if (agent.GetComponent(scriptName) == null){
				scriptName = null;
				methodName = null;
			}

			if (GUILayout.Button("Select Method")){
				EditorUtils.ShowMethodSelectionMenu(agent.gameObject, new List<System.Type>{typeof(bool)}, new List<System.Type>(), delegate(MethodInfo method){
					scriptName = method.ReflectedType.Name;
					methodName = method.Name;
					if (Application.isPlaying)
						OnInit();
				});
			}

			if (!string.IsNullOrEmpty(methodName)){
				GUILayout.BeginVertical("box");
				EditorGUILayout.LabelField("Selected Component", scriptName);
				EditorGUILayout.LabelField("Selected Method", methodName);
				GUILayout.EndVertical();
			}

			EditorUtils.BBVariableField("Check Bool", boolCheck);
		}

		#endif
	}
}