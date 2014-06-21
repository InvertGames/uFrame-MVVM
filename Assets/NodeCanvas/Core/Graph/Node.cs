#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Variables;

namespace NodeCanvas{

	public enum NodeStates {
		
		Failure  = 0,
		Success  = 1,
		Running  = 2,
		Resting  = 3,
		Error    = 4,
	}

	///The base class for all nodes that can live in NodeCanvas' graph systems
	abstract public class Node : MonoBehaviour{

		[SerializeField]
		private List<Connection> _inConnections = new List<Connection>();
		[SerializeField]
		private List<Connection> _outConnections = new List<Connection>();
		[SerializeField]
		private Graph _graph;

		private NodeStates _nodeState = NodeStates.Resting;
		private float _elapsedTime;
		private int _ID;
		private bool isChecked;

		//store to avoid spamming reflection
		private string _nodeName;
		private string _nodeDescription;
		//

		////
		////

		///The title name of the node shown in the window if editor is not in Icon Mode
		virtual public string nodeName{
			get
			{
				if (string.IsNullOrEmpty(_nodeName) ){
					NameAttribute nameAtt = this.GetType().GetCustomAttributes(typeof(NameAttribute), false).FirstOrDefault() as NameAttribute;
					if (nameAtt != null){
						_nodeName = nameAtt.name;
					} else {

						#if UNITY_EDITOR
						_nodeName = EditorUtils.CamelCaseToWords(GetType().Name);
						#endif
						#if !UNITY_EDITOR
						_nodeName = GetType().Name;
						#endif
					}
				}
				return _nodeName;
			}
		}

		///The node description shown within the inspector if Editor Node Info is on.
		virtual public string nodeDescription{
			get
			{
				if (string.IsNullOrEmpty(_nodeDescription)){
					DescriptionAttribute descAtt = this.GetType().GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
					if (descAtt != null){
						_nodeDescription = descAtt.description;
					} else {
						_nodeDescription = "No Description";
					}
				}
				return _nodeDescription;
			}
		}

		///The numer of possible inputs. -1 for infinite
		virtual public int maxInConnections{
			get {return -1;}
		}

		///The numer of possible outputs. -1 for infinite
		virtual public int maxOutConnections{
			get {return -1;}
		}

		///The output connection Type this node has
		virtual public System.Type outConnectionType{
			get {return typeof(ConditionalConnection);}
		}

		///Can this node be set as prime (Start)?
		virtual public bool allowAsPrime{
			get {return true;}
		}

		///All incomming connections to this node
		public List<Connection> inConnections{
			get {return _inConnections;}
			protected set {_inConnections = value;}
		}

		///All outgoing connections from this node
		public List<Connection> outConnections{
			get {return _outConnections;}
			protected set {_outConnections = value;}
		}

		///The graph this node belongs to
		public Graph graph{
			get {return _graph;}
			private set {_graph = value;}
		}

		///The current state of the node
		public NodeStates nodeState{
			get {return _nodeState;}
			protected set {_nodeState = value;}
		}

		//The elapsed time this node runs
		public float elapsedTime{
			get {return _elapsedTime;}
			protected set {_elapsedTime = value;}
		}

		///The node's ID
		public int ID{
			get {return _ID;}
			private set {_ID = value;}
		}

		///The current agent of the graph this node belongs to
		protected Component graphAgent{
			get {return graph != null? graph.agent : null;}
		}

		///The current blackboard of the graph this node belongs to
		protected Blackboard graphBlackboard{
			get {return graph != null? graph.blackboard : null;}
		}

		/////////////////////
		/////////////////////
		/////////////////////

		public NodeStates Execute(){
			return Execute(graphAgent, graphBlackboard);
		}

		public NodeStates Execute(Component agent){
			return Execute(agent, graphBlackboard);
		}

		///The main execution function of the node. Execute the node for the agent and blackboard provided. Default = graphAgent and graphBlackboard
		public NodeStates Execute(Component agent, Blackboard blackboard){
			
			if (isChecked)
				return Error("Infinite Loop Detected. Node ID: " + ID +"  will not execute to prevent stack overflow.");
				
			isChecked = true;
			nodeState = OnExecute(agent, blackboard);
			isChecked = false;
			
			return nodeState;
		}

		///A little helper function to log errors more easily
		protected NodeStates Error(string log){
			Debug.LogError("<b>Graph Error:</b> '" + log + "' On node '" + nodeName + "' ID " + ID + " | On graph '" + graph.graphName + "'", graph.gameObject);
			return NodeStates.Error;
		}

		///Override this to specify what the node does.
		virtual protected NodeStates OnExecute(Component agent, Blackboard blackboard){
			
			return OnExecute(agent);
		}

		virtual protected NodeStates OnExecute(Component agent){

			return OnExecute();
		}

		virtual protected NodeStates OnExecute(){

			return nodeState;
		}

		public void ResetNode(){
			ResetNode(true);
		}

		///Recursively reset the node and child nodes if it's node Resting already
		public void ResetNode(bool recursively){

			if (nodeState == NodeStates.Resting || isChecked)
				return;

			OnReset();
			nodeState = NodeStates.Resting;
			elapsedTime = 0;

			isChecked = true;
			for (int i = 0; i < outConnections.Count; i++)
				outConnections[i].ResetConnection(recursively);
			isChecked = false;
		}

		///Called when the node gets reseted
		virtual protected void OnReset(){

		}

		///Sends an event to the graph
		public void SendEvent(string name){
			graph.SendEvent(name);
		}

		//Nodes can start coroutine through MonoManager for even when they are disabled
		new protected Coroutine StartCoroutine(IEnumerator routine){
			return MonoManager.current.StartCoroutine(routine);
		}

		//Set the target blackboard for all BBVariables found on node. Done when creating node, OnValidate as well as when graphBlackboard set to a new value.
		public void UpdateNodeBBFields(Blackboard bb){
			BBVariable.SetBBFields(bb, this);
		}

		///Updates the node ID in it's current graph. This is called in the editor GUI for convenience, as well as whenever a change is made in the node graph and from the node graph.
		public int AssignIDToGraph(Graph toNodeGraph, int lastID){

			if (isChecked)
				return lastID;

			isChecked = true;

			lastID++;
			ID = lastID;

			if (gameObject.name != ID + "_" + nodeName)
				gameObject.name = ID + "_" + nodeName;

			for ( int i= 0; i < outConnections.Count; i++){
				Connection connection= outConnections[i];
				lastID = connection.targetNode.AssignIDToGraph(toNodeGraph, lastID);
				if (connection.gameObject.name != connection.sourceNode.ID + "_" + connection.targetNode.ID + " Connection")
					connection.gameObject.name = connection.sourceNode.ID + "_" + connection.targetNode.ID + " Connection";
			}

			return lastID;
		}

		public void ResetRecursion(){

			if (!isChecked)
				return;

			isChecked = false;
			
			for (int i = 0; i < outConnections.Count; i++)
				outConnections[i].targetNode.ResetRecursion();
		}

		///Fetch all child nodes of the node
		public List<Node> FetchAllChildNodes(bool includeThis){

			List<Node> childList = new List<Node>();

			if (includeThis)
				childList.Add(this);

			foreach (Connection connection in outConnections){
				childList.Add(connection.targetNode);
				childList.AddRange(connection.targetNode.FetchAllChildNodes(false));
			}

			return childList;
		}

		///Fetch all child nodes of this node with their depth in regards to this node.
		public Dictionary<Node, int> FetchAllChildNodesWithDepth(bool includeThis, int startIndex){

			var childList = new Dictionary<Node, int>();

			if (includeThis)
				childList[this] = startIndex;

			foreach (Connection connection in outConnections){
				childList[connection.targetNode] = startIndex + 1;
				foreach (KeyValuePair<Node, int> pair in connection.targetNode.FetchAllChildNodesWithDepth(false, startIndex + 1))
					childList.Add(pair.Key, pair.Value);
			}

			return childList;
		}

		///Called when a port is connected
		virtual public void OnPortConnected(int portIndex){

		}

		///Called when a port is disconnected but before it actually does
		virtual public void OnPortDisconnected(int portIndex){

		}

		///Called when the parent graph is started (not continued from pause). Use to init values or otherwise.
		virtual public void OnGraphStarted(){

		}

		///Called when the parent graph is stopped.
		virtual public void OnGraphStoped(){

		}

		///Called when the parent graph is paused.
		virtual public void OnGraphPaused(){

		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		[HideInInspector]
		public Rect nodeRect = new Rect(100,300,100,40);

		[SerializeField]
		private string nodeComment = string.Empty;

		private bool inResizeMode;
		private Texture2D _icon;
		private bool nodeIsClicked;

		public static Vector2 minSize = new Vector2(100, 40);
		private static Port clickedPort;

		public bool inIconMode{
			get {return icon != null && Graph.iconMode;}
		}

		//Icon is assigned either A: by Icon attribute or B: by class name.
		public Texture2D icon{
			get
			{
				if (_icon == null){
					var iconAtt = this.GetType().GetCustomAttributes(typeof(IconAttribute), true).FirstOrDefault() as IconAttribute;
					_icon = (Texture2D)Resources.Load( iconAtt != null? iconAtt.iconName : GetType().Name );
				}
				return _icon;			
			}
		}

		public static Color successColor{
			get {return new Color(0.4f, 0.7f, 0.2f);}
		}

		public static Color failureColor{
			get {return new Color(1.0f, 0.4f, 0.4f);}
		}

		public static Color runningColor{
			get {return Color.yellow;}
		}

		public static Color restingColor{
			get {return new Color(0.7f, 0.7f, 1f, 0.8f);}
		}

		////////////////

		virtual protected void Reset(){

		}

		//Called in editor as usual
		virtual protected void OnValidate(){
			UpdateNodeBBFields(graphBlackboard);
		}

		///Create a new Node
		public static Node Create(Graph ownerGraph, System.Type nodeType){

			Node newNode = new GameObject(nodeType.ToString()).AddComponent(nodeType) as Node;
			newNode.graph = ownerGraph;
			newNode.transform.parent = ownerGraph.nodesRoot;
			newNode.transform.localPosition = Vector3.zero;
			newNode.UpdateNodeBBFields(ownerGraph.blackboard);
			newNode.OnCreate();
			return newNode;
		}

		///Called when the node is created
		virtual protected void OnCreate(){
			
		}

		//Returns if a new connection should be allowed with the source node.
		public bool IsNewConnectionAllowed(Node sourceNode){
			
			if (this == sourceNode){
				Debug.LogWarning("Node can't connect to itself");
				return false;
			}

			if (sourceNode.outConnections.Count >= sourceNode.maxOutConnections && sourceNode.maxOutConnections != -1){
				Debug.LogWarning("Source node can have no more out connections.");
				return false;
			}

			if (this == graph.primeNode && maxInConnections == 1){
				Debug.LogWarning("Target node can have no more connections");
				return false;
			}

			if (maxInConnections <= inConnections.Count && maxInConnections != -1){
				Debug.LogWarning("Target node can have no more connections");
				return false;
			}

			return true;
		}

		//Moves the node to another graph.CAREFULL! Connections must be Relinked as well
		public void MoveToGraph(Graph newGraph){

			Undo.RecordObject(newGraph, "Re-Assign Node");
			Undo.RecordObject(graph, "Re-Assign Node");
			graph.allNodes.Remove(this);
			newGraph.allNodes.Add(this);
			graph = newGraph;
			
			Undo.SetTransformParent(transform, graph.nodesRoot, "Re-Assign Node");
			transform.localPosition = Vector3.zero;
		}

		//Sorts the connections based on the child nodes and this node X position. Possible only when not in play mode
		protected void SortConnectionsByPositionX(){
			
			if (!Application.isPlaying){

				if (isChecked)
					return;

				isChecked = true;

				Undo.RecordObject(this, "Re-Sort");
				outConnections = outConnections.OrderBy(c => c.targetNode.nodeRect.center.x ).ToList();
				foreach(Connection connection in inConnections)
					connection.sourceNode.SortConnectionsByPositionX();

				isChecked = false;
			}
		}

		//The main function for drawing a node's gui.Fires off others.
		public void ShowNodeGUI(){

			if (graph && graph.primeNode == this)
				GUI.Box(new Rect(nodeRect.x, nodeRect.y - 20, nodeRect.width, 20), "<b>START</b>");

			DrawNodeWindow();
			DrawNodeComments();
			DrawNodeConnections();
		}

		private void DrawNodeComments(){

			if (graph && graph.showComments){

				var commentsRect = new Rect();

				if (maxOutConnections == 0){
					var height = new GUIStyle("textArea").CalcHeight(new GUIContent(nodeComment), nodeRect.width);
					commentsRect = new Rect(nodeRect.x, nodeRect.yMax + 5, nodeRect.width, height);
				} else {
					commentsRect = new Rect(nodeRect.xMax + 5, nodeRect.yMin, 130, nodeRect.height);
				}

				if (!string.IsNullOrEmpty(nodeComment)){
					GUI.color = new Color(1,1,1,0.6f);
					GUI.backgroundColor = new Color(1f,1f,1f,0.2f);
					GUI.Box(commentsRect, nodeComment, new GUIStyle("textArea"));
					GUI.backgroundColor = Color.white;
					GUI.color = Color.white;
				}
			}
		}

		//Simple method that most importantly creates the node window + it put some color for debug in the graph editor.
		private void DrawNodeWindow(){

			if (Graph.currentSelection == this)
				GUI.color = new Color(0.95f, 0.95f, 1);

			GUI.Box(nodeRect, "", new GUIStyle("windowShadow") );
			GUI.color = new Color(1,1,1,0.3f);
			GUI.Box(new Rect(nodeRect.x+6, nodeRect.y+6, nodeRect.width, nodeRect.height), "", new GUIStyle("windowShadow") );

			GUI.color = Color.white;

			if (inIconMode){

				nodeRect = GUILayout.Window(ID, nodeRect, NodeWindowGUI, string.Empty, "compactWindow");

			} else {
			
				nodeRect = GUILayout.Window (ID, nodeRect, NodeWindowGUI, nodeName, "window");
			}

			if (Application.isPlaying){

				if (nodeState == NodeStates.Resting)
					GUI.color = restingColor;
				else if (nodeState == NodeStates.Success)
					GUI.color = successColor;
				else if (nodeState == NodeStates.Running)
					GUI.color = runningColor;
				else if (nodeState == NodeStates.Failure)
					GUI.color = failureColor;
				else if (nodeState == NodeStates.Error)
					GUI.color = Color.red;

				GUI.Box(nodeRect, "", new GUIStyle("windowHighlight"));
				
			} else {
				
				if (Graph.currentSelection == this){
					GUI.color = restingColor;
					GUI.Box(nodeRect, "", new GUIStyle("windowHighlight"));
				}
			}

			EditorGUIUtility.AddCursorRect(nodeRect, MouseCursor.Link);

			//should be done outside window callback
			if (Event.current.button == 2 && Event.current.type == EventType.MouseDrag)
				PanNode(Event.current.delta, false);

			GUI.color = Color.white;
		}

		//This is the callback function of the GUILayout.window. Everything here is INSIDE the node Window.
		private void NodeWindowGUI(int ID){

			if (this == null)
				return;

			if (inIconMode){
				GUI.backgroundColor = new Color(0,0,0,0.05f);
				GUILayout.Box(icon);
				GUI.backgroundColor = Color.white;
			}

			Event e = Event.current;

			if (Graph.currentSelection == this && e.keyCode == KeyCode.Delete && e.type == EventType.KeyDown){
				graph.RemoveNode(this);
				e.Use();
				return;
			}

		    Rect scaleNodeRect= new Rect(nodeRect.width-10,nodeRect.height-10, 8, 8);
		    GUI.Box(scaleNodeRect, "", "nodeScaleBtn");

		    if (e.button == 0 && e.type == EventType.MouseDown && scaleNodeRect.Contains(e.mousePosition)){
		    	inResizeMode = true;
		    	e.Use();
		    }

			if (e.button == 0 && e.type == EventType.MouseDown && Graph.allowClick){
				Graph.currentSelection = this;
			}

	    	if (e.type == EventType.MouseUp){
	    		inResizeMode = false;
		    	graph.SnapNodes();
	    	}

	    	if (inResizeMode){
		    	nodeRect.width = Mathf.Max(e.mousePosition.x+10, minSize.x);
		    	nodeRect.height = Mathf.Max(e.mousePosition.y+10, minSize.y);
		    }

	        ///STATUS ICONS
	        Rect markRect = new Rect(5, 5, 15, 15);
	        if (nodeState == NodeStates.Success){
	        	GUI.color = successColor;
	        	GUI.Box(markRect, "", new GUIStyle("checkMark"));

	        } else if (nodeState == NodeStates.Running){
	        	GUI.Box(markRect, "", new GUIStyle("clockMark"));

	        } else if (nodeState == NodeStates.Failure){
	        	GUI.color = failureColor;
	        	GUI.Box(markRect, "", new GUIStyle("xMark"));
	        }
	        ///

	        GUI.color = Color.white;
	        GUI.skin = null;
	        GUI.skin.label.richText = true;

			OnNodeGUI();

		    if (e.button == 1 && e.type == EventType.MouseDown){

	            GenericMenu menu = new GenericMenu();

	            if (graph.primeNode != this && allowAsPrime)
		            menu.AddItem (new GUIContent ("Make Start (CTRL+Click)"), false, delegate{graph.primeNode = this;});

				menu.AddItem (new GUIContent ("Duplicate (CTRL+D)"), false, delegate{Duplicate();});

	            OnContextMenu(menu);

				menu.AddSeparator("/");
	            menu.AddItem (new GUIContent ("Delete Node (DEL)"), false, delegate{graph.RemoveNode(this);});
	            menu.ShowAsContext();
	            e.Use();
		    } 

		    if (!inResizeMode){

				//Something is bugged. Commented until I find it out
				//if (!e.control && e.type == EventType.Repaint)
				//	nodeRect.height = GUILayoutUtility.GetLastRect().yMax;

		    	if (e.button == 0 && e.type == EventType.MouseDown){
		    		nodeIsClicked = true;
		    		OnNodePicked();
		    	}

		    	if (e.button == 0 && e.type == EventType.MouseUp){
		    		nodeIsClicked = false;
		    		OnNodeReleased();
		    	}

		    	if (e.type == EventType.MouseDown && e.clickCount == 2){
		    		if (this is INestedNode && (this as INestedNode).nestedGraph != null ){
	    				graph.nestedGraphView = (this as INestedNode).nestedGraph;
		    		} else {
			    		AssetDatabase.OpenAsset(MonoScript.FromMonoBehaviour(this));
		    		}
		    	}

		    	if (e.type == EventType.MouseDown && e.button == 0 && e.control){
		    		graph.primeNode = this;
		    		e.Use();
		    	}

		    	if (e.button != 2 && Graph.allowClick){
		    		Undo.RecordObject(this, "Move");
			    	if (nodeIsClicked && e.button == 0 && e.type == EventType.MouseDrag)
			    		PanNode(e.delta, e.shift);
			    	GUI.DragWindow();
			    }
		    }
		}

		virtual protected void OnNodePicked(){

		}

		virtual protected void OnNodeReleased(){

		}

		//Function to pan the node recursively if need be. Called from the graph or from this
		private void PanNode(Vector2 delta, bool panChildren){

			Undo.RecordObject(this, "Move");
			nodeRect.center += delta;

			if (panChildren){

				for (int i= 0; i < outConnections.Count; i++){
					Node node = outConnections[i].targetNode;
					if (node.ID > this.ID)					
						node.PanNode(delta, true);
				}
			}
		}

		//Editor. Override to show controls within the node window
		virtual protected void OnNodeGUI(){

			GUILayout.Label("", GUILayout.Height(1));
		}

		public void ShowNodeInspectorGUI(){

			Undo.RecordObject(this, "Node Inspector");
			nodeComment = EditorGUILayout.TextField(nodeComment);
			EditorUtils.TextFieldComment(nodeComment);

			EditorUtils.Separator();

			OnNodeInspectorGUI();

			if (GUI.changed)
				EditorUtility.SetDirty(this);
		}

		//Editor. Override to show controls within the inline inspector or leave it to show an automatic editor
		virtual protected void OnNodeInspectorGUI(){

			DrawDefaultInspector();
		}

		///Draw an automatic editor inspector for this node.
		protected void DrawDefaultInspector(){

			EditorUtils.ShowAutoEditorGUI(this);	
		}

		//Editor. Override to add more entries to the right click context menu of the node
		virtual protected void OnContextMenu(GenericMenu menu){

		}

		//Duplicate node
		public Node Duplicate(){

			var newNode = Instantiate(this, this.transform.position, this.transform.rotation) as Node;
			Undo.RegisterCreatedObjectUndo(newNode.gameObject, "Duplicate");

			Undo.RecordObject(graph, "Duplicate");
			graph.allNodes.Add(newNode);

			Undo.RecordObject(newNode, "Duplicate");
			newNode.transform.parent = this.transform.parent;
			newNode.inConnections.Clear();
			newNode.outConnections.Clear();
			newNode.nodeRect.center += new Vector2(50,50);
			return newNode;
		}

		//Draw the connections line from this node, to all of its children. This is the default. Override in each system's base node class.
		virtual protected void DrawNodeConnections(){

			Event e = Event.current;

			//Receive connections
			if (clickedPort != null && e.type == EventType.MouseUp){

				var port = clickedPort;
				if (ID == graph.allNodes.Count)
					clickedPort = null;

				if (nodeRect.Contains(e.mousePosition)){
					if (graph.ConnectNode(port.parent, this, port.portIndex) != null)
						clickedPort = null;
				}
			}

			if (maxOutConnections == 0)
				return;

			var nodeOutputBox = new Rect(nodeRect.x, nodeRect.yMax - 4, nodeRect.width, 12);
			GUI.Box(nodeOutputBox, "", new GUIStyle("nodePortContainer"));
			
			if (outConnections.Count < maxOutConnections || maxOutConnections == -1){

				for (int i = 0; i < outConnections.Count + 1; i++){
					
					Rect portRect = new Rect(0, 0, 10, 10);
					portRect.center = new Vector2(((nodeRect.width / (outConnections.Count + 1)) * (i + 0.5f)) + nodeRect.xMin, nodeRect.yMax + 6);
					GUI.Box(portRect, "", "nodePortEmpty");

					EditorGUIUtility.AddCursorRect(portRect, MouseCursor.ArrowPlus);

					if (e.button == 0 && e.type == EventType.MouseDown && portRect.Contains(e.mousePosition))
						clickedPort = new Port(i, this, portRect.center);
				}
			}

			//draw the new connection line if in link mode
			if (clickedPort != null && clickedPort.parent == this)
				Handles.DrawBezier(clickedPort.pos, e.mousePosition, clickedPort.pos, e.mousePosition, restingColor, null, 2);

			//draw all connected lines
			for (int connectionIndex = 0; connectionIndex < outConnections.Count; connectionIndex++){
				
				var connection = outConnections[connectionIndex];
				if (connection != null){

					var sourcePos = new Vector2(((nodeRect.width / (outConnections.Count + 1)) * (connectionIndex + 1) ) + nodeRect.xMin, nodeRect.yMax + 6);
					var targetPos = new Vector2(connection.targetNode.nodeRect.center.x, connection.targetNode.nodeRect.y);

					Rect connectedPortRect = new Rect(0,0,12,12);
					connectedPortRect.center = sourcePos;
					GUI.Box(connectedPortRect, "", "nodePortConnected");
					connection.DrawConnectionGUI(sourcePos, targetPos);

					//On right click disconnect connection from the source.
					if (e.button == 1 && e.type == EventType.MouseDown && connectedPortRect.Contains(e.mousePosition)){
						graph.RemoveConnection(connection);
						e.Use();
						return;
					}
				}
			}
		}

		//EDITOR. Class for the nodeports
		class Port{

			public int portIndex;
			public Node parent;
			public Vector2 pos;

			public Port(int index, Node parent, Vector2 pos){
				this.portIndex = index;
				this.parent = parent;
				this.pos = pos;
			}
		}

		#endif
	}
}