using UnityEditor;
using UnityEngine;
using NodeCanvas;

namespace NodeCanvasEditor{

	public class ExternalInspector : EditorWindow {

		private object currentSelection;
		private Vector2 scrollPos;

		void OnEnable(){
	        title = "NC Inspector";
	        Graph.useExternalInspector = true;
		}

		void OnDestroy(){
			Graph.useExternalInspector = false;
		}

		void Update(){
			if (currentSelection != Graph.currentSelection)
				Repaint();
		}

		void OnGUI(){

			if (GraphEditor.currentGraph == null)
				return;
				
			if (EditorApplication.isCompiling){
				ShowNotification(new GUIContent("Compiling Please Wait..."));
				return;			
			}

			currentSelection = Graph.currentSelection;

			if (currentSelection == null)
				return;

			scrollPos = GUILayout.BeginScrollView(scrollPos);

			if (typeof(Node).IsAssignableFrom(currentSelection.GetType()) ){
				var node = currentSelection as Node;
				Title(node.nodeName );
				if (Graph.showNodeInfo){
					GUI.backgroundColor = new Color(0.8f,0.8f,1);
					EditorGUILayout.HelpBox(node.nodeDescription, MessageType.None);
					GUI.backgroundColor = Color.white;
				}
				node.ShowNodeInspectorGUI();
			}
			
			if (typeof(Connection).IsAssignableFrom(currentSelection.GetType() )){
				Title("Connection");
				(currentSelection as Connection).ShowConnectionInspectorGUI();
			}

			GUILayout.EndScrollView();
		}

		void Title(string text){

			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("<b><size=16>" + text + "</size></b>");
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			EditorUtils.BoldSeparator();
		}

	    [MenuItem("NC/External Inspector")]
	    public static void OpenWindow() {

	        var window = GetWindow(typeof(ExternalInspector)) as ExternalInspector;
	        window.Show();
	    }
	}
}