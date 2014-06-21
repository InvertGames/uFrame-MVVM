#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas {

	///Base class for connections. This can be used as an identity connection
	[AddComponentMenu("")]
	public class Connection : MonoBehaviour{

		[SerializeField]
		private Node _sourceNode;
		[SerializeField]
		private Node _targetNode;
		[SerializeField]
		private bool _isDisabled;
		private NodeStates _connectionState = NodeStates.Resting;

		
		///The source node of the connection
		public Node sourceNode{
			get {return _sourceNode; }
			private set {_sourceNode = value;}
		}

		///The target node of the connection
		public Node targetNode{
			get {return _targetNode; }
			private set {_targetNode = value;}
		}

		///The connection state
		public NodeStates connectionState{
			get {return _connectionState;}
			set {_connectionState = value;}
		}

		///Is the connection disabled?
		public bool isDisabled{
			get {return _isDisabled;}
			set
			{
				_isDisabled = value;
				if (value == true)
					ResetConnection();
			}
		}

		///The graph this connection belongs to taken from the source node.
		protected Graph graph{
			get {return sourceNode.graph;}
		}

		///The current agent of the graph taken from the source node.
		protected Component graphAgent{
			get {return graph != null? graph.agent : null;}
		}

		///The current blackboard of the graph taken from the source node.
		protected Blackboard graphBlackboard{
			get {return graph != null? graph.blackboard : null;}
		}

		///////////
		///////////

		public NodeStates Execute(){

			return Execute(graphAgent, graphBlackboard);
		}

		public NodeStates Execute(Component agent){

			return Execute(agent, graphBlackboard);
		}

		///Execute the conneciton for the specified agent and blackboard.
		public NodeStates Execute(Component agent, Blackboard blackboard){

			if (isDisabled)
				return NodeStates.Resting;

			connectionState = OnExecute(agent, blackboard);
			return connectionState;
		}

		///Overide to derived connection types. By default returns whatever targetNode.Execute returns
		virtual protected NodeStates OnExecute(Component agent, Blackboard blackboard){

			return targetNode.Execute(agent, blackboard);
		}

		public void ResetConnection(){
			ResetConnection(true);
		}

		///Resets the connection and its targetNode, optionaly recursively
		public void ResetConnection(bool recursively){

			connectionState = NodeStates.Resting;
			OnReset();

			if (recursively)
				targetNode.ResetNode(recursively);
		}

		///Called when the connection is reset
		virtual protected void OnReset(){

		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		protected Rect areaRect        = new Rect(0,0,50,10);
		
		[SerializeField]
		bool isVirtualConnection       = false;
		NodeStates lastConnectionState = NodeStates.Resting;
		Color connectionColor          = new Color(0.5f,0.5f,0.8f,0.8f);
		float lineSize                 = 2;
		bool nowSwitchingColors        = false;
		Vector3 lineFromTangent        = Vector3.zero;
		Vector3 lineToTangent          = Vector3.zero;
		bool isRelink;

		///Create a new Connection
		public static Connection Create(Node source, Node target, int sourceIndex){

			Connection newConnection = new GameObject(source.ID + "_" + target.ID + "_Connection").AddComponent(source.outConnectionType) as Connection;
			newConnection.transform.parent = source.transform;
			newConnection.transform.localPosition = Vector3.zero;
			newConnection.sourceNode = source;
			newConnection.targetNode = target;
			newConnection.sourceNode.outConnections.Insert(sourceIndex, newConnection);
			newConnection.targetNode.inConnections.Add(newConnection);
			newConnection.OnCreate(sourceIndex, target.inConnections.IndexOf(newConnection));
			return newConnection;
		}

		///Called when connection is created
		virtual protected void OnCreate(int sourceIndex, int targetIndex){

		}


		//Relink connection to other target node
		public void Relink(Node newNode){

			Undo.RecordObject(targetNode, "Relink");
			Undo.RecordObject(newNode, "Relink");
			Undo.RecordObject(this, "Relink");
			targetNode.inConnections.Remove(this);
			newNode.inConnections.Add(this);
			targetNode = newNode;
		}

		//Draw them
		public void DrawConnectionGUI(Vector3 lineFrom, Vector3 lineTo){

			//curveMode 0 is smooth
			var mlt = Graph.curveMode == 0? 0.85f : 1f;
			var tangentX = Mathf.Abs(lineFrom.x - lineTo.x) * mlt;
			var tangentY = Mathf.Abs(lineFrom.y - lineTo.y) * mlt;

			GUI.color = connectionColor;
			var arrowRect = new Rect(0,0,20,20);
			arrowRect.center = lineTo;


			if (lineFrom.x <= sourceNode.nodeRect.x)
				lineFromTangent = new Vector3(-tangentX, 0, 0);

			if (lineFrom.x >= sourceNode.nodeRect.xMax)
				lineFromTangent = new Vector3(tangentX, 0, 0);

			if (lineFrom.y <= sourceNode.nodeRect.y)
				lineFromTangent = new Vector3(0, -tangentY, 0);

			if (lineFrom.y >= sourceNode.nodeRect.yMax)
				lineFromTangent = new Vector3(0, tangentY, 0);

			if (lineTo.x <= targetNode.nodeRect.x){
				lineToTangent = new Vector3(-tangentX, 0, 0);
				GUI.Box(arrowRect, "", "nodeInputLeft");
			}

			if (lineTo.x >= targetNode.nodeRect.xMax){
				lineToTangent = new Vector3(tangentX, 0, 0);
				GUI.Box(arrowRect, "", "nodeInputRight");
			}

			if (lineTo.y <= targetNode.nodeRect.y){
				lineToTangent = new Vector3(0, -tangentY, 0);
				GUI.Box(arrowRect, "", "nodeInputTop");
			}

			if (lineTo.y >= targetNode.nodeRect.yMax){
				lineToTangent = new Vector3(0, tangentY, 0);
				GUI.Box(arrowRect, "", "nodeInputBottom");
			}

			GUI.color = Color.white;

			if (isVirtualConnection){
				lineTo = lineFrom + (lineFromTangent.normalized * 60);
				lineFromTangent = Vector2.zero;
				lineToTangent = Vector2.zero;
				GUI.backgroundColor = new Color(1f,1f,1f,0.5f);
				if (GUI.Button( new Rect(lineTo.x - 25, lineTo.y, 50, 20), "#" + targetNode.ID, new GUIStyle("box")))
					Graph.currentSelection = targetNode;
				GUI.backgroundColor = Color.white;
			}

			///

			Event e = Event.current;

			Rect outPortRect = new Rect(0,0,12,12);
			outPortRect.center = lineFrom;

			if (!Application.isPlaying){
				connectionColor = Node.restingColor;
				lineSize = Graph.currentSelection == this? 4 : 2;
			}

			//Ability to relink connection to other nodes
			if (areaRect.Contains(e.mousePosition) && e.type == EventType.MouseDown && e.button == 0)
				isRelink = true;

			if (isRelink){
				Handles.DrawBezier(areaRect.center, e.mousePosition, areaRect.center, e.mousePosition, new Color(1,1,1,0.5f), null, 2);
				if (e.type == EventType.MouseUp){
					foreach(Node node in graph.allNodes){
						if (node.nodeRect.Contains(e.mousePosition) && node.IsNewConnectionAllowed(sourceNode) ){
							Relink(node);
							break;
						}
					}
					isRelink = false;
				} 
			}

			//On click select this connection
			if ( (Graph.allowClick && e.type == EventType.MouseDown && e.button == 0) && (areaRect.Contains(e.mousePosition) || outPortRect.Contains(e.mousePosition) )){
				Graph.currentSelection = this;
				e.Use();
				return;
			}

			//with delete key, remove connection
			if (Graph.currentSelection == this && e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete && GUIUtility.keyboardControl == 0){
				graph.RemoveConnection(this);
				e.Use();
				return;
			}

			connectionColor = isDisabled? new Color(0.3f, 0.3f, 0.3f) : connectionColor;

			//check this != null for when in playmode user removes a running connection
			if (Application.isPlaying && this != null && connectionState != lastConnectionState && !nowSwitchingColors && !isDisabled){
				MonoManager.current.StartCoroutine(ChangeLineColorAndSize());
				lastConnectionState = connectionState;
			}

			Handles.color = connectionColor;
			if (Graph.curveMode == 0){
				var shadow = new Vector3(3.5f,3.5f,0);
				Handles.DrawBezier(lineFrom, lineTo+shadow, lineFrom+shadow + lineFromTangent+shadow, lineTo+shadow + lineToTangent, new Color(0,0,0,0.1f), null, lineSize+10f);
				Handles.DrawBezier(lineFrom, lineTo, lineFrom + lineFromTangent, lineTo + lineToTangent, connectionColor, null, lineSize);
			} else {
				var shadow = new Vector3(1,1,0);
				Handles.DrawPolyLine(lineFrom, lineFrom + lineFromTangent*0.5f, lineTo + lineToTangent*0.5f, lineTo);
				Handles.DrawPolyLine(lineFrom+shadow, (lineFrom + lineFromTangent*0.5f)+shadow, (lineTo + lineToTangent*0.5f)+shadow, lineTo+shadow);
			}
			Handles.color = Color.white;

			//Find the mid position of the connection to draw GUI stuff there
			Vector2 midPosition;

			var t = 0.5f;
			var B1 = t*t*t;
			var B2 = 3*t*t*(1-t);
			var B3 = 3*t*(1-t)*(1-t);
			var B4 = (1-t)*(1-t)*(1-t);
			var C1 = lineFrom;
			var C2 = lineFrom - (lineFromTangent/2);
			var C3 = lineTo - (lineToTangent/2);
			var C4 = lineTo;
			var posX = C1.x * B1 + C2.x * B2 + C3.x * B3 + C4.x * B4;
			var posY = C1.y * B1 + C2.y * B2 + C3.y * B3 + C4.y * B4;

			midPosition = new Vector2(posX, posY);

			if (!isVirtualConnection)
				midPosition += (Vector2)(lineFromTangent + lineToTangent) /2;

			areaRect.center = midPosition;
			OnConnectionGUI();
		}

		
		//Editor. Override to show custom controls in the rectangle area in the mid position of the connection
		virtual protected void OnConnectionGUI(){

			areaRect.width = 0;
			areaRect.height = 0;
		}

		public void ShowConnectionInspectorGUI(){

			GUILayout.BeginHorizontal();
			GUI.color = new Color(1,1,1,0.5f);

			if (GUILayout.Button("<", GUILayout.Height(14), GUILayout.Width(20)))
				Graph.currentSelection = sourceNode;

			if (GUILayout.Button(">", GUILayout.Height(14), GUILayout.Width(20)))
				Graph.currentSelection = targetNode;

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("X", GUILayout.Height(14), GUILayout.Width(20))){
				graph.RemoveConnection(this);
				return;
			}

			GUI.color = Color.white;
			GUILayout.EndHorizontal();

			Undo.RecordObject(this, "Connection Value Change");

			GUILayout.BeginHorizontal();
			isDisabled = EditorGUILayout.ToggleLeft("Disable Connection", isDisabled, GUILayout.Width(150));
			isVirtualConnection = EditorGUILayout.ToggleLeft("Shortcut", isVirtualConnection);
			GUILayout.EndHorizontal();

			EditorUtils.BoldSeparator();
			OnConnectionInspectorGUI();

			if (GUI.changed)
				EditorUtility.SetDirty(this);
		}

		//Editor.Override to show controls in the editor panel when connection is selected
		virtual protected void OnConnectionInspectorGUI(){
			
		}

		//Simple tween to enhance the GUI line for debugging.
		private IEnumerator ChangeLineColorAndSize(){

			float effectLength = 0.2f;
			float timer = 0;

			//no tween when its going to become resting
			if (connectionState == NodeStates.Resting){
				connectionColor = Node.restingColor;
				yield break;
			}

			if (connectionState == NodeStates.Success)
				connectionColor = Node.successColor;

			if (connectionState == NodeStates.Failure)
				connectionColor = Node.failureColor;

			if (connectionState == NodeStates.Running)
				connectionColor = Node.runningColor;

			nowSwitchingColors = true;
				
			while(timer < effectLength){

				timer += Time.deltaTime;
				lineSize = Mathf.Lerp(5, 2, timer/effectLength);
				yield return null;
			}

			if (connectionState == NodeStates.Resting)
				connectionColor = Node.restingColor;

			if (connectionState == NodeStates.Success)
				connectionColor = Node.successColor;

			if (connectionState == NodeStates.Failure)
				connectionColor = Node.failureColor;

			if (connectionState == NodeStates.Running)
				connectionColor = Node.runningColor;
			
			nowSwitchingColors = false;
		}

		#endif
	}
}