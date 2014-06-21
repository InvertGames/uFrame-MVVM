#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Script Control")]
	[Description("Execute a function on a script, of up to 3 parameters and save the return if any")]
	[AgentType(typeof(Transform))]
	public class ExecuteFunction : ActionTask {

		public BBVariableSet paramValue1 = new BBVariableSet();
		public BBVariableSet paramValue2 = new BBVariableSet();
		public BBVariableSet paramValue3 = new BBVariableSet();
		public BBVariableSet returnValue = new BBVariableSet{blackboardOnly = true};

		[SerializeField]
		private string methodName;
		[SerializeField]
		private string scriptName;

		private Component script;
		private MethodInfo method;
		private List<System.Type> paramTypes;
		private int paramCount;

		protected override string info{
			get
			{
				if (string.IsNullOrEmpty(methodName))
					return "No Method Selected";

				string paramInfo = "";
				paramInfo += paramValue1.selectedType != null? paramValue1.selectedBBVariable.ToString() : "";
				paramInfo += paramValue2.selectedType != null? ", " + paramValue2.selectedBBVariable.ToString() : "";
				paramInfo += paramValue3.selectedType != null? ", " + paramValue3.selectedBBVariable.ToString() : "";
				return (returnValue.selectedType != null? returnValue.selectedBBVariable.ToString() + "= ": "") + agentInfo + "." + methodName + "(" + paramInfo + ")" ;
			}
		}

		//store the method info on init for performance
		protected override string OnInit(){
			script = agent.GetComponent(scriptName);
			if (script == null)
				return "Missing Component '" + scriptName + "' on Agent '" + agent.gameObject.name + "' . Did the agent changed at runtime?";
			
			paramCount = 0;
			paramTypes = new List<System.Type>();
			if (paramValue1.selectedType == null)
				paramTypes = System.Type.EmptyTypes.ToList();
			if (paramValue1.selectedType != null){
				paramTypes.Add(paramValue1.selectedType);
				paramCount++;
			}
			if (paramValue2.selectedType != null){
				paramTypes.Add(paramValue2.selectedType);
				paramCount++;
			}
			if (paramValue3.selectedType != null){
				paramTypes.Add(paramValue3.selectedType);
				paramCount++;
			}

			method = script.GetType().GetMethod(methodName, paramTypes.ToArray());
			return null;
		}

		//do it by invoking method
		protected override void OnExecute(){

			if (method != null){

				object o = null;
				if (paramCount == 0)
					o = method.Invoke(script, null);
				else if (paramCount == 1)
					o = method.Invoke(script, new object[]{paramValue1.selectedObjectValue});
				else if (paramCount == 2)
					o = method.Invoke(script, new object[]{paramValue1.selectedObjectValue, paramValue2.selectedObjectValue});
				else if (paramCount == 3)
					o = method.Invoke(script, new object[]{paramValue1.selectedObjectValue, paramValue2.selectedObjectValue, paramValue3.selectedObjectValue});

				if (method.ReturnType != typeof(void))
					returnValue.selectedObjectValue = o;

				EndAction(true);
			
			} else {

				EndAction(false);
			}
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		[SerializeField]
		private List<string> paramNames = new List<string>{"Param1","Param2","Param3"}; //init for update

		protected override void OnTaskInspectorGUI(){

			if (agent == null){
				EditorGUILayout.HelpBox("This Action needs the Agent to be known. Currently the Agent is unknown.\nConsider overriding the Agent.", MessageType.Error);
				return;
			}

			if (agent.GetComponent(scriptName) == null){
				scriptName = null;
				methodName = null;
				paramValue1.selectedType = null;
			}

			if (GUILayout.Button("Select Method")){
				
				EditorUtils.ShowMethodSelectionMenu(agent.gameObject, returnValue.availableTypes, paramValue1.availableTypes, delegate(MethodInfo method){
					scriptName = method.ReflectedType.Name;
					methodName = method.Name;
					paramNames = new List<string>();
					var parameters = method.GetParameters();
					foreach (ParameterInfo param in parameters)
						paramNames.Add(param.Name);

					paramValue1.selectedType = parameters.Length >= 1? parameters[0].ParameterType : null;
					paramValue2.selectedType = parameters.Length >= 2? parameters[1].ParameterType : null;
					paramValue3.selectedType = parameters.Length >= 3? parameters[2].ParameterType : null;
					returnValue.selectedType = method.ReturnType == typeof(void)? null : method.ReturnType;
					if (Application.isPlaying)
						OnInit();
				});
			}

			if (!string.IsNullOrEmpty(methodName)){
				GUILayout.BeginVertical("box");
				EditorGUILayout.LabelField("Selected Component", scriptName);
				EditorGUILayout.LabelField("Selected Method", methodName);
				if (paramValue1.selectedType != null)
					EditorGUILayout.LabelField(paramNames[0], EditorUtils.TypeName(paramValue1.selectedType));
				if (paramValue2.selectedType != null)
					EditorGUILayout.LabelField(paramNames[1], EditorUtils.TypeName(paramValue2.selectedType));
				if (paramValue3.selectedType != null)
					EditorGUILayout.LabelField(paramNames[2], EditorUtils.TypeName(paramValue3.selectedType));
				if (returnValue.selectedType != null)
					EditorGUILayout.LabelField("Return Type", EditorUtils.TypeName(returnValue.selectedType));
				GUILayout.EndVertical();
			}

			if (paramValue1.selectedType != null)
				EditorUtils.BBVariableField(paramNames[0], paramValue1.selectedBBVariable);

			if (paramValue2.selectedType != null)
				EditorUtils.BBVariableField(paramNames[1], paramValue2.selectedBBVariable);

			if (paramValue3.selectedType != null)
				EditorUtils.BBVariableField(paramNames[2], paramValue3.selectedBBVariable);

			if (returnValue.selectedType != null)
				EditorUtils.BBVariableField("Save Return Value", returnValue.selectedBBVariable);
		}

		#endif
	}
}