#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Reflection;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Script Control")]
	[Description("Set a variable on a script")]
	[AgentType(typeof(Transform))]
	public class SetField : ActionTask {

		public BBVariableSet setValue = new BBVariableSet();

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

				return agentInfo + "." + fieldName + " = " + setValue.selectedBBVariable;
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
				field.SetValue(script, setValue.selectedObjectValue);
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
				fieldName = null;
				setValue.selectedType = null;
			}

			if (GUILayout.Button("Select Field")){
				EditorUtils.ShowFieldSelectionMenu(agent.gameObject, setValue.availableTypes, delegate(FieldInfo field){
					scriptName = field.ReflectedType.Name;
					fieldName = field.Name;
					setValue.selectedType = field.FieldType;
					if (Application.isPlaying)
						OnInit();
				});
			}

			if (!string.IsNullOrEmpty(fieldName)){
				GUILayout.BeginVertical("box");
				EditorGUILayout.LabelField("Selected Component", scriptName);
				EditorGUILayout.LabelField("Selected Field", fieldName);
				EditorGUILayout.LabelField("Field Type", EditorUtils.TypeName(setValue.selectedType) );
				GUILayout.EndVertical();
			}

			if (setValue.selectedType != null)
				EditorUtils.BBVariableField("Set Value", setValue.selectedBBVariable);
		}

		#endif
	}
}