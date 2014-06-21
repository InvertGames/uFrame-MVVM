using UnityEngine;

namespace NodeCanvas.Actions{

	[Category("✫ Utility")]
	[Description("Alternative use to set boolean, int and float blackboard variables")]
	public class ExecuteExpression : ActionTask {

		public string expression;

		private string leftVar;
		private string operation;
		private string rightVar;

		private System.Type type;
		private object leftValue;
		private object rightValue;

		private string error;

		protected override string info{
			get {return string.IsNullOrEmpty(error)? "'" + expression + "'" : error;}
		}

		protected override void OnExecute(){

			string[] words = expression.Split(' ');
			if (words.Length != 3 || string.IsNullOrEmpty(words[2]) ){
				Error("Wrong format");
				return;
			}

			leftVar        = words[0];
			operation      = words[1];
			rightVar       = words[2];

			rightValue = null;
			var tempData = blackboard.GetData(rightVar, type);
			if (tempData != null)
				rightValue = tempData.objectValue;

			leftValue = blackboard.GetDataValue<object>(leftVar);
			if (leftValue == null){
				Error("No variable exists");
				return;
			}

			type = leftValue.GetType();

			if (type != typeof(bool) && type != typeof(float) && type != typeof(int)){
				Error("Unsupported Variable Type");
				return;
			}

			error = null;

			try
			{
				if (type == typeof(bool))
					SetBool();

				if (type == typeof(float))
					SetFloat();

				if (type == typeof(int))
					SetInt();
			}
			catch
			{
				Error("Parsing Error");
				return;
			}

			EndAction();
		}

		void Error(string err){
			error = "<color=#d63e3e>" + err + "</color>";
			EndAction(false);
		}

		void SetBool(){

			if (rightValue == null)
				rightValue = bool.Parse(rightVar);

			if (operation == "=")
				blackboard.SetDataValue(leftVar, (bool)rightValue);
			else if (operation == "!=")
				blackboard.SetDataValue(leftVar, !(bool)rightValue);
			else Error("Wrong Format");
		}

		void SetFloat(){

			if (rightValue == null)
				rightValue = float.Parse(rightVar);

			if (operation == "=")
				blackboard.SetDataValue(leftVar, rightValue);
			else if (operation == "+=")
				blackboard.SetDataValue(leftVar, blackboard.GetDataValue<float>(leftVar) + (float)rightValue);
			else if (operation == "-=")
				blackboard.SetDataValue(leftVar, blackboard.GetDataValue<float>(leftVar) - (float)rightValue);
			else if (operation == "*=")
				blackboard.SetDataValue(leftVar, blackboard.GetDataValue<float>(leftVar) * (float)rightValue);
			else if (operation == "/=")
				blackboard.SetDataValue(leftVar, blackboard.GetDataValue<float>(leftVar) / (float)rightValue);
			else Error("Wrong Format");
		}

		void SetInt(){

			if (rightValue == null)
				rightValue = int.Parse(rightVar);

			if (operation == "=")
				blackboard.SetDataValue(leftVar, rightValue);
			else if (operation == "+=")
				blackboard.SetDataValue(leftVar, blackboard.GetDataValue<int>(leftVar) + (int)rightValue);
			else if (operation == "-=")
				blackboard.SetDataValue(leftVar, blackboard.GetDataValue<int>(leftVar) - (int)rightValue);
			else if (operation == "*=")
				blackboard.SetDataValue(leftVar, blackboard.GetDataValue<int>(leftVar) * (int)rightValue);
			else if (operation == "/=")
				blackboard.SetDataValue(leftVar, blackboard.GetDataValue<int>(leftVar) / (int)rightValue);
			else Error("Wrong Format");
		}


		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnTaskInspectorGUI(){
			DrawDefaultInspector();
			GUILayout.Label("<i>For Example:\n'myFloat += myOtherFloat'\n'myBool = true'</i>");
		}
		
		#endif
	}
}