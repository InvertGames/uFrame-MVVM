#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Reflection;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Script Control")]
	[AgentType(typeof(Transform))]
	[Description("Get a variable of a script and save it to the blackboard")]
	public class GetField : ActionTask {

		public BBVariableSet saveAs = new BBVariableSet{blackboardOnly = true};

		[SerializeField]
		private string fieldName;
		[SerializeField]
		private string scriptName;

		private Component script;
		private FieldInfo field;

		protected override string info{
			get
			{
				if (string.IsNullOrEmpty(fieldName))
					return "No Field Selected";

				return (saveAs.selectedBBVariable + " = " + agentInfo + "." + fieldName);
			}
		}

		protected override string OnInit(){
			script = agent.GetComponent(scriptName);
			if (script == null)
				return "Missing Component";
			field = script.GetType().GetField(fieldName);
			return null;
		}

		protected override void OnExecute(){

			if (field != null){
				saveAs.selectedObjectValue = field.GetValue(script);
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
				EditorGUILayout.HelpBox("This Action needs the Agent to be known. Currently the Agent is unknown.", MessageType.Error);
				return;
			}

			if (agent.GetComponent(scriptName) == null){
				scriptName = null;
				fieldName = null;
				saveAs.selectedType = null;
			}

			if (GUILayout.Button("Select Field")){
				EditorUtils.ShowFieldSelectionMenu(agent.gameObject, saveAs.availableTypes, delegate(FieldInfo field){
					scriptName = field.ReflectedType.Name;
					fieldName = field.Name;
					saveAs.selectedType = field.FieldType;
					if (Application.isPlaying)
						OnInit();
				});
			}

			if (!string.IsNullOrEmpty(fieldName)){
				GUILayout.BeginVertical("box");
				EditorGUILayout.LabelField("Selected Component", scriptName);
				EditorGUILayout.LabelField("Selected Field", fieldName);
				EditorGUILayout.LabelField("Field Type", EditorUtils.TypeName(saveAs.selectedType) );
				GUILayout.EndVertical();
			}

			if (saveAs.selectedType != null)
				EditorUtils.BBVariableField("Save As", saveAs.selectedBBVariable);
		}

		#endif
	}
}