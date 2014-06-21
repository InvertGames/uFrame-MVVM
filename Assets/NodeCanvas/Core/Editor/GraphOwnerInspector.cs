using UnityEditor;
using UnityEngine;
using System.Collections;
using NodeCanvas;

namespace NodeCanvasEditor{

	[InitializeOnLoad]
	public class HierarchyIcons{
		
		static HierarchyIcons(){
/*
			if (!System.IO.Directory.Exists(Application.dataPath + "/Gizmos"))
				AssetDatabase.CreateFolder("Assets", "Gizmos");
			AssetDatabase.MoveAsset("Assets/NodeCanvas/GraphOwner.png", "Assets/Gizmos/GraphOwner.png");
*/
			EditorApplication.hierarchyWindowItemOnGUI += ShowIcon;
		}

		static void ShowIcon(int ID, Rect r){
			r.x = r.xMax - 18;
			r.width = 18;
			var go = EditorUtility.InstanceIDToObject(ID) as GameObject;
			if (go != null){
				if (go.GetComponent<GraphOwner>() != null)
					GUI.Label(r, "♟");
				if (go.GetComponent<Graph>() != null)
					GUI.Label(r, "⑆");
			}
		}
	}

	public class GraphOwnerInspector : Editor {

		private string debugEvent;

		GraphOwner owner{
			get{return target as GraphOwner;}
		}

		void OnDestroy(){

			if (owner == null){
				if (owner.graph != null){
					var selectedOption = EditorUtility.DisplayDialogComplex("Removing Graph Owner", "When removing Owner, it's assigned graph is not deleted automaticaly since it might be shared amongst many Owners.\nDo you want to delete the assigned Graph?", "Yes", "No", "No & Select Graph");
					if (selectedOption == 0)
						Undo.DestroyObjectImmediate(owner.graph.gameObject);
					if (selectedOption == 2)
						Selection.activeObject = owner.graph;
				}
			}
		}

		public override void OnInspectorGUI(){

			Undo.RecordObject(owner, "Owner Inspector");
			
			var label = owner.graphType.Name;

			if (owner.graph == null){
				
				EditorGUILayout.HelpBox(label + "Owner needs " + label + ". Assign or Create a new one", MessageType.Info);
				if (GUILayout.Button("CREATE NEW")){
				
					if (owner.graph == null){
						owner.graph = new GameObject(label).AddComponent(owner.graphType) as Graph;
						owner.graph.transform.parent = owner.transform;
						owner.graph.transform.localPosition = Vector3.zero;
						Undo.RegisterCreatedObjectUndo(owner.graph.gameObject, "New Graph");
					}

					owner.graph.agent = owner;
				}

				owner.graph = (Graph)EditorGUILayout.ObjectField(label, owner.graph, owner.graphType, true);
				return;
			}

			GUILayout.Space(10);

			owner.graph.graphName = EditorGUILayout.TextField(label + " Name", owner.graph.graphName);
            if (string.IsNullOrEmpty(owner.graph.graphName))
              owner.graph.graphName = owner.graph.gameObject.name;
			owner.graph.graphComments = GUILayout.TextArea(owner.graph.graphComments, GUILayout.Height(50));
			EditorUtils.TextFieldComment(owner.graph.graphComments);

			GUI.backgroundColor = EditorUtils.lightBlue;
			if (GUILayout.Button("OPEN BEHAVIOUR"))
				GraphEditor.OpenWindow(owner);
		
			GUI.backgroundColor = Color.white;
			GUI.color = new Color(1, 1, 1, 0.5f);
			owner.graph = (Graph)EditorGUILayout.ObjectField("Current " + label, owner.graph, owner.graphType, true);
			GUI.color = Color.white;

			owner.blackboard = (Blackboard)EditorGUILayout.ObjectField("Blackboard", owner.blackboard, typeof(Blackboard), true);
			owner.onEnable = (GraphOwner.EnableAction)EditorGUILayout.EnumPopup("On Enable", owner.onEnable);
			owner.onDisable = (GraphOwner.DisableAction)EditorGUILayout.EnumPopup("On Disable", owner.onDisable);

			OnExtraOptions();

			if (owner.graph != null && !(PrefabUtility.GetPrefabType(owner.graph) == PrefabType.Prefab) && Application.isPlaying){

				var graph = owner.graph;
				var pressed = new GUIStyle(GUI.skin.GetStyle("button"));
				pressed.normal.background = GUI.skin.GetStyle("button").active.background;

				GUILayout.BeginHorizontal("box");
				GUILayout.FlexibleSpace();

				if (GUILayout.Button(EditorUtils.playIcon, graph.isRunning || graph.isPaused? pressed : "button")){
					if (graph.isRunning || graph.isPaused) graph.StopGraph();
					else graph.StartGraph();
				}

				if (GUILayout.Button(EditorUtils.pauseIcon, graph.isPaused? pressed : "button")){	
					if (graph.isPaused) graph.StartGraph();
					else graph.PauseGraph();
				}

				//Unfotunately we check it like that for now
				if (owner.graphType == typeof(NodeCanvas.BehaviourTrees.BehaviourTree)){
					if (GUILayout.Button(EditorUtils.stepIcon)){	
						(owner as NodeCanvas.BehaviourTrees.BehaviourTreeOwner).Tick();
					}
				}

				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}


			EditorUtils.EndOfInspector();

			if (GUI.changed){
				EditorUtility.SetDirty(owner);
				EditorUtility.SetDirty(owner.graph);
			}
		}

		virtual protected void OnExtraOptions(){
			
		}
	}
}