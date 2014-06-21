using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Utility")]
	[Description("Display a UI label on the agent's position if seconds to run is not 0, else simply logs the message")]
	[AgentType(typeof(Transform))]
	public class DebugAction : ActionTask{

		public string log;
		public float YOffset;
		public float secondsToRun;
		public bool actionReturn = true;

		private Texture2D _tex;
		private Texture2D tex{
			get
			{
				if (!_tex){
					_tex = new Texture2D(1,1);
					_tex.SetPixel(0, 0, Color.white);
					_tex.Apply();
				}
				return _tex;			
			}
		}

		protected override string info{
			get {return (secondsToRun > 0? "UI Log '" : "Log '") + log + "'" + (secondsToRun > 0? " for " + secondsToRun + " sec." : ""); }
		}

		protected override void OnExecute(){

			if (secondsToRun <= 0){
				Debug.Log(log);
				EndAction(actionReturn);
			}

			useGUILayout = !string.IsNullOrEmpty(log);
		}

		protected override void OnUpdate(){

			if (elapsedTime >= secondsToRun){
				EndAction(actionReturn);
			}
		}

		void OnGUI(){
			
			if (Camera.main == null || string.IsNullOrEmpty(log) || agent == null)
				return;

			Vector2 point = Camera.main.WorldToScreenPoint(agent.transform.position + new Vector3(0, YOffset, 0));
			Vector2 finalSize = new GUIStyle("label").CalcSize(new GUIContent(log));
			Rect r = new Rect(0, 0, finalSize.x, finalSize.y);
			point.y = Screen.height - point.y;
			r.center = point;
			GUI.color = new Color(1f,1f,1f,0.5f);
			r.width += 8;
			GUI.DrawTexture(r, tex);
			GUI.color = new Color(0.2f, 0.2f, 0.2f, 1);
			r.x += 4;
			GUI.Label(r, log);
			GUI.color = Color.white;
		}
	}
}