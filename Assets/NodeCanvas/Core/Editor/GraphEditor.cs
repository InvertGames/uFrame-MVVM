using UnityEngine;
using System.Collections;
using UnityEditor;
using NodeCanvas;

namespace NodeCanvasEditor{

	public class GraphEditor : EditorWindow{

		public static GraphEditor current;
		public static Graph currentGraph;

		private Graph _rootGraph;
		private int rootGraphID;

		private GraphOwner _targetOwner;
		private int targetOwnerID;

		private Rect canvas= new Rect(0, 0, 2000, 2000);
		private Rect bottomRect;
		private GUISkin guiSkin;
		private Vector2 scrollPos= Vector2.zero;
		private float topMargin = 20;
		private float bottomMargin = 5;
		private int repaintCounter;

		private Graph rootGraph{
			get
			{
				if (_rootGraph == null)
					_rootGraph = EditorUtility.InstanceIDToObject(rootGraphID) as Graph;
				return _rootGraph;
			}
			set
			{
				_rootGraph = value;
				if (value != null)
					rootGraphID = value.GetInstanceID();
			}
		}

		private GraphOwner targetOwner{
			get
			{
				if (_targetOwner == null)
					_targetOwner = EditorUtility.InstanceIDToObject(targetOwnerID) as GraphOwner;
				return _targetOwner;
			}
			set
			{
				_targetOwner = value;
				targetOwnerID = value != null? value.GetInstanceID() : 0;
			}
		}

		void OnEnable(){
			
			current = this;
			title = "NodeCanvas";
			guiSkin = EditorGUIUtility.isProSkin? (GUISkin)Resources.Load("NodeCanvasSkin") : (GUISkin)Resources.Load("NodeCanvasSkinLight");
			Repaint();
		}

		void OnInspectorUpdate(){
			Repaint();
		}

		void OnGUI(){

			if (EditorApplication.isCompiling){
				ShowNotification(new GUIContent("Compiling Please Wait..."));
				return;			
			}

			if (targetOwner != null)
				rootGraph = targetOwner.graph;

			if (rootGraph == null){
				ShowNotification(new GUIContent("Please select a GameObject with a Graph Owner or a Graph itself"));
				return;
			}

			currentGraph = rootGraph.CurrentlyShowingGraph();

	        if (PrefabUtility.GetPrefabType(currentGraph) == PrefabType.Prefab){
	            ShowNotification(new GUIContent("Editing is not allowed when prefab asset is selected for safety. Please place the prefab in a scene, edit and apply it"));
	            //return;
	        }

			GUI.skin = guiSkin;
			Event e = Event.current;

			if (mouseOverWindow == this && (e.isMouse || e.isKey) )
				repaintCounter += 2;

			if (e.type == EventType.ValidateCommand && e.commandName == "UndoRedoPerformed"){
                GUIUtility.hotControl = 0;
                GUIUtility.keyboardControl = 0;
				return;
			}

			if (e.type == EventType.MouseDown || e.type == EventType.MouseUp || e.type == EventType.KeyUp){
				if (PrefabUtility.GetPrefabType(currentGraph) == PrefabType.PrefabInstance){
					ShowNotification(new GUIContent("Prefab Disconnected. Apply when done."));
					PrefabUtility.DisconnectPrefabInstance(currentGraph);
				}
			}

			//Canvas Scroll pan
			if (e.button == 0 && e.isMouse && e.type == EventType.MouseDrag && e.alt){
				scrollPos += e.delta * 2;
				e.Use();
			}

			Graph.scrollOffset = scrollPos;

			//Get and set canvas limits for the nodes
			Vector2 canvasLimits= currentGraph.GetCanvasLimits();
			canvas.width = canvasLimits.x;
			canvas.height = canvasLimits.y;

			Rect actualCanvas= new Rect(5, topMargin, position.width - 10, position.height - (topMargin + bottomMargin));
			GUI.Box(actualCanvas, "NodeCanvas v1.5.0", "canvasBG");


			//Begin windows and ScrollView for the nodes.
			scrollPos = GUI.BeginScrollView (actualCanvas, scrollPos, canvas);
			BeginWindows();
			currentGraph.ShowNodeGraphWindows();
			EndWindows();
			GUI.EndScrollView();
			//End windows and scrollview for the nodes.

			//Hierarchy
			var hRect = new Rect(20, 25, Screen.width, Screen.height);
			GUILayout.BeginArea(hRect);
			rootGraph.ShowNodeGraphHierarchy();
			GUILayout.EndArea();
			//

			currentGraph.ShowInlineInspectorGUI();
			currentGraph.ShowBlackboardGUI();
			currentGraph.ShowNodeGraphControls();


			GUI.Box(actualCanvas,"", "canvasBorders");

			if (repaintCounter > 0 || currentGraph.isRunning){
				repaintCounter = Mathf.Max (repaintCounter -1, 0);
				Repaint();
			}

			GUI.skin = null;
			GUI.color = Color.white;
			GUI.backgroundColor = Color.white;
		}

		//Change viewing graph
		void OnSelectionChange(){
			
			if (Selection.activeGameObject != null){
				var foundOwner = Selection.activeGameObject.GetComponent<GraphOwner>();
				if (!Graph.isLocked && foundOwner != null){
					targetOwner = foundOwner;
					Graph.currentSelection = null;
				}
			}
		}

	    //Opeining the window for a graph owner
	    public static GraphEditor OpenWindow(GraphOwner owner){
	    	var window = OpenWindow(owner.graph, owner, owner.blackboard);
	    	window.targetOwner = owner;
	    	return window;
	    }

	    //For opening the window from gui button in the nodegraph's Inspector.
	    public static GraphEditor OpenWindow(Graph newGraph){
	    	return OpenWindow(newGraph, newGraph.agent, newGraph.blackboard);
	    }

	    public static GraphEditor OpenWindow(Graph newGraph, Component agent, Blackboard blackboard) {

	        GraphEditor window = GetWindow(typeof(GraphEditor)) as GraphEditor;
	        newGraph.agent = agent;
	        newGraph.blackboard = blackboard;
	        newGraph.SendTaskOwnerDefaults();
	        newGraph.nestedGraphView = null;
	        newGraph.UpdateNodeIDsInGraph();

	        window.rootGraph = newGraph;
	        window.targetOwner = null;
	        Graph.currentSelection = null;
	        return window;
	    }
	}
}