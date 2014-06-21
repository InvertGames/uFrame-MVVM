#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas{

	///Base class for all Conditions. Conditions dont span multiple frames like actions and return true or false immediately on execution. Derive this to create your own
	abstract public class ConditionTask : Task{

		[SerializeField]
		private bool invertCondition;
		private int yieldReturn = -1;

		sealed override public string taskInfo{
			get {return (agentIsOverride? "* " : "") + (invertCondition? "If <b>!</b> ":"If ") + info;}
		}

		///Editor: Override to provide the condition info to show in editor whenever needed
		virtual protected string info{
			get {return taskName;}
		}

		///Check the condition for the provided agent
		public bool CheckCondition(Component agent){
			return CheckCondition(agent, this.blackboard);
		}

		///Check the condition for the provided agent and with the provided blackboard
		public bool CheckCondition(Component agent, Blackboard blackboard){

			if (!Set(agent, blackboard))
				return false;

			if (yieldReturn != -1){
				return invertCondition? !(yieldReturn == 1) : (yieldReturn == 1);
			}

			return invertCondition? !OnCheck() : OnCheck();
		}

		///Override in your own conditions and return whether the condition is true or false. The result will be inverted if Invert is checked.
		virtual protected bool OnCheck(){
			return true;
		}

		///Helper method that holds the return value provided for one frame, for the condition to return.
		protected void YieldReturn(bool value){
			yieldReturn = value? 1 : 0;
			StartCoroutine(Fade());
		}

		IEnumerator Fade(){
			yield return null;
			yieldReturn = -1;
		}

		//////////////////////////////////
		/////////GUI & EDITOR STUFF///////
		//////////////////////////////////
		#if UNITY_EDITOR

		///Editor: Show the editor GUI controls
		override public void ShowInspectorGUI(){

			GUI.color = invertCondition? Color.white : new Color(1f,1f,1f,0.5f);
			invertCondition = EditorGUILayout.ToggleLeft("Invert Condition", invertCondition);
			GUI.color = Color.white;

			Undo.RecordObject(this, "Condition Value Change");
			base.ShowInspectorGUI();
			OnTaskInspectorGUI();

			if (GUI.changed && this != null)
				EditorUtility.SetDirty(this);
		}

		///Editor: Optional override to show custom controls whenever the ShowInspectorGUI is called. By default controls will automaticaly show.
		virtual protected void OnTaskInspectorGUI(){

			DrawDefaultInspector();
		}
/*
		public static string CreateNewConditionScript(string name){
			
			string template =
			"using UnityEngine;\n" +
			"using NodeCanvas;\n" +
			"using NodeCanvas.Variables;\n\n" + 
			"public class " + name + " : ConditionTask {\n\n" +
			"\tprotected override bool OnCheck(){\n" +
			"\t\treturn true;\n" +
			"\t}\n" + 
			"}";

			bool dirExists = System.IO.Directory.Exists(Application.dataPath + "/MyNCCoditions/");
			if (!dirExists)
				System.IO.Directory.CreateDirectory(Application.dataPath + "/MyNCCoditions/");

			var scriptPath = Application.dataPath + "/MyNCCoditions/" + name + ".cs";
			System.IO.File.WriteAllText(scriptPath, template);
			UnityEditor.AssetDatabase.ImportAsset("Assets/MyNCCoditions/" + name + ".cs");
			return scriptPath;
		}
*/
		#endif
	}
}