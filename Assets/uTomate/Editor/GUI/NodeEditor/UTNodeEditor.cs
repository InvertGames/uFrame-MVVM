//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// The actual node editor.
/// </summary>
public class UTNodeEditor
{
	
	private static int ControlHint = "nodeEditor".GetHashCode ();
	
	public static UTNodeEditorData NodeEditor (UTNodeEditorData editorData, UTNodeEditorModel editorModel, params GUILayoutOption[] options)
	{
		Rect inRect = EditorGUILayout.BeginHorizontal (options);
		int controlId = GUIUtility.GetControlID (ControlHint, FocusType.Passive);
		UTNodeEditorState state = (UTNodeEditorState)GUIUtility.GetStateObject (typeof(UTNodeEditorState), controlId);
		
		if (editorData == null) {
			editorData = new UTNodeEditorData ();
		}
		
		var evt = Event.current;
		var currentEventType = evt.type;		

		// reset drag and drop state 
		if (currentEventType == EventType.DragUpdated || currentEventType == EventType.DragPerform || currentEventType == EventType.DragExited) {
			state.isDropTarget = false;
		}
	
		if (currentEventType == EventType.Repaint) {
			UTEditorResources.GraphBackgroundStyle.Draw (inRect, false, false, false, false);
		}
		
		if (editorModel.HighlightedNode != null) {
			editorData.scrollPosition = Vector2.MoveTowards (editorData.scrollPosition, editorModel.HighlightedNode.Bounds.center, 2f);
		}
		
		editorData.scrollPosition = EditorGUILayout.BeginScrollView (editorData.scrollPosition);
		Vector2 mousePosition = evt.mousePosition;

		Vector2 requiredSize = CalculateRequiredSize (editorModel);
		// allocate space within the scroll view
		GUILayoutUtility.GetRect (requiredSize.x, requiredSize.y);
		DrawNodes (editorModel);
				
		if (state.sourceConnector != null) {
			UTLineUtils.DrawLine (state.sourceConnector.connectorPosition.center, state.lastMousePosition);
		}
		
		// we need to get it here as well so we get events limited to the scroll view.
		evt = Event.current;
		currentEventType = evt.type;		

		if (editorModel.HasPlan) {
			switch (currentEventType) {
			case EventType.Repaint:
				if (state.isDrawingSelection) {
					var selectionRect = ToRect (evt.mousePosition, state.lastMousePosition);
					GUI.Box (selectionRect, "");
				}
				break;
			case EventType.ContextClick: 
				if (GUIUtility.hotControl != 0 && GUIUtility.hotControl != controlId) {
					break;	
				}
				GUIUtility.hotControl = controlId;
				ShowPopup (evt, editorModel);	
				evt.Use ();
				break;
			case EventType.MouseUp:
				if (GUIUtility.hotControl != 0 && GUIUtility.hotControl != controlId) {
					break;	
				}
				if (evt.button == 0) {
					if (state.isDrawingSelection) {
						state.isDrawingSelection = false;
						var theRect = ToRect (evt.mousePosition, state.lastMousePosition);
					
						if (evt.alt) {
							editorModel.SelectNodesInRect (theRect, UTNodeEditorModel.SelectionMode.Subtract);
						} else if (evt.shift) {
							editorModel.SelectNodesInRect (theRect, UTNodeEditorModel.SelectionMode.Add);
						} else {
							editorModel.SelectNodesInRect (theRect, UTNodeEditorModel.SelectionMode.Replace);
						}
					} else {
						if (state.hotNode != null && state.sourceConnector != null) {
							if (state.hotNode != state.sourceConnector.owner) {
								editorModel.AddConnection (state.sourceConnector, state.hotNode);
							}
						}
						if (state.delayedSelectionMode) {
							if (evt.alt) {
								editorModel.SelectNode (state.hotNode, UTNodeEditorModel.SelectionMode.Subtract);
							} else {
								editorModel.SelectNode (state.hotNode, UTNodeEditorModel.SelectionMode.Replace);
							}
						
						}
					
						state.delayedSelectionMode = false;
						state.hotNode = null;
						state.sourceConnector = null;
					}
					// release lock if required
					GUIUtility.hotControl = 0;
					evt.Use ();
				}
				break;
			case EventType.MouseDown:
				if (GUIUtility.hotControl != 0 && GUIUtility.hotControl != controlId) {
					break;	
				}
				GUIUtility.hotControl = controlId;
				if (evt.button == 0) { //left click, only 
					if (state.sourceConnector == null) {
						state.sourceConnector = GetConnectorUnderMouse (editorModel, evt.mousePosition);
						state.lastMousePosition = evt.mousePosition;
					}
					if (state.sourceConnector == null) {
						state.hotNode = GetNodeUnderMouse (editorModel, evt.mousePosition);
						if (state.hotNode != null) {
							if (editorModel.SelectedNodes.Contains (state.hotNode)) {
								state.delayedSelectionMode = true;
							} else {
								if (evt.alt) {
									editorModel.SelectNode (state.hotNode, UTNodeEditorModel.SelectionMode.Subtract);
								} else if (evt.shift) {
									editorModel.SelectNode (state.hotNode, UTNodeEditorModel.SelectionMode.Add);
								} else {
									editorModel.SelectNode (state.hotNode, UTNodeEditorModel.SelectionMode.Replace);
								}
							}
						} else {
							// start selection rect
							state.lastMousePosition = evt.mousePosition;
							state.isDrawingSelection = true;
						}
					}
					state.dragThresholdReached = false;
					evt.Use ();
				}
				break;
			case EventType.MouseDrag:
				if (GUIUtility.hotControl != 0 && GUIUtility.hotControl != controlId) {
					break;	
				}
				var connector = state.sourceConnector;
				if (connector != null) {
					state.lastMousePosition = evt.mousePosition;
					state.hotNode = GetNodeUnderMouse (editorModel, evt.mousePosition);
					evt.Use ();
					break;
				}
			
				var node = state.hotNode;
				if (node != null && connector == null) {
					if (!state.dragThresholdReached) {
						state.dragThresholdReached = (evt.mousePosition - state.lastMousePosition).magnitude > 3;
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
						if (state.dragThresholdReached) {
							editorModel.StartMovingNodes ();
						}
#endif	
					} else {
#if !(UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
						editorModel.StartMovingNodes();
#endif
						var theDelta = evt.delta;
						if (theDelta.x < 0 || theDelta.y < 0) {			
							foreach (var aNode in editorModel.SelectedNodes) {
								if (theDelta.x < 0 && aNode.Bounds.x < (Mathf.Abs (theDelta.x))) {
									theDelta.x = -aNode.Bounds.x;
								}
								if (theDelta.y < 0 && aNode.Bounds.y < (Mathf.Abs (theDelta.y))) {
									theDelta.y = -aNode.Bounds.y;
								}
							}
						}
				
						foreach (var aNode in editorModel.SelectedNodes) {
							var newX = aNode.Bounds.x + theDelta.x;
							var newY = aNode.Bounds.y + theDelta.y;
							aNode.Bounds = new Rect (newX, newY, aNode.Bounds.width, aNode.Bounds.height);
						}
					}
					evt.Use ();
				}
			
				if (state.isDrawingSelection) {
					HandleUtility.Repaint ();
				}
			
				state.delayedSelectionMode = false;
				break;
			case EventType.DragUpdated:
			case EventType.DragPerform:
				if (editorData.IsDropSupported) {
					if (editorData.DropTarget.CanDrop ()) {
						state.isDropTarget = true;
						if (currentEventType == EventType.DragPerform) {
							editorData.DropTarget.AcceptDrop (mousePosition);
						}						
					}
		
					if (state.isDropTarget) {
						DragAndDrop.activeControlID = controlId;
						evt.Use ();
					}
				}
				break;
			case EventType.KeyDown:
				var delete = false;
				if (Application.platform == RuntimePlatform.OSXEditor) {
					if (evt.keyCode == KeyCode.Backspace && evt.command) {
						delete = true;
					}
				} else {
					if (evt.keyCode == KeyCode.Delete && evt.shift) {
						delete = true;
					}
				}
				if (delete) {
					editorModel.DeleteNodes (editorModel.SelectedNodes);
					
				}
				break;
			}
		}
		EditorGUILayout.EndScrollView ();
		EditorGUILayout.EndHorizontal ();
		return editorData;
	}
	
	private static Vector2 CalculateRequiredSize (UTNodeEditorModel editorModel)
	{
		float width = 0;
		float height = 0;
		foreach (UTNode node in editorModel.GetNodes()) {
			width = Mathf.Max (node.Bounds.xMax, width);
			height = Mathf.Max (node.Bounds.yMax, height);
		}
		return new Vector2 (width, height);		
	}
	
	private static UTNode GetNodeUnderMouse (UTNodeEditorModel editorModel, Vector2 mousePosition)
	{
		foreach (var node in editorModel.GetNodes()) {
			if (node.Bounds.Contains (mousePosition)) {
				return node;
			}	
		}
		return null;
	}
	
	private static UTNode.Connector GetConnectorUnderMouse (UTNodeEditorModel editorModel, Vector2 mousePosition)
	{
		foreach (var node in editorModel.GetNodes()) {
			foreach (var connector in node.Connectors) {
				if (connector.connectorPosition.Contains (mousePosition)) {
					return connector;
				}			
			}
		}
		return null;
	}
	
	private static void DrawNodes (UTNodeEditorModel editorModel)
	{
		// GUI.depth works on a per-script basis, that's why we need to draw the nodes
		// in 3 phases and cannot use GUI.depth here.
		// - Draw connectors
		// - Draw lines
		// - Draw actual nodes
		DrawNodes (editorModel, NodeDrawingPhase.DrawLines);
		DrawNodes (editorModel, NodeDrawingPhase.DrawNodes);
	}
	
	private static void DrawNodes (UTNodeEditorModel editorModel, NodeDrawingPhase phase)
	{
		foreach (UTNode node in editorModel.GetNodes()) {
			DrawNode (editorModel, node, phase);
		}
	}
	
	private static void DrawNode (UTNodeEditorModel editorModel, UTNode node, NodeDrawingPhase phase)
	{
		if (phase == NodeDrawingPhase.DrawNodes) {
			var style = UTEditorResources.GraphNodeStyle;
			if (node.Data is UTAutomationPlanNoteEntry) {
				style = UTEditorResources.GraphCommentStyle;
				if (editorModel.SelectedNodes.Contains (node)) {
					style = UTEditorResources.GraphCommentSelectedStyle;
				}
			} else {
				if (editorModel.SelectedNodes.Contains (node)) {
					style = UTEditorResources.GraphNodeSelectedStyle;
				}
				if (node == editorModel.HighlightedNode) {
					style = UTEditorResources.GraphNodeHighlightStyle;
				}
			}
			
			GUI.Box (node.Bounds, "", style);
			if (editorModel.IsFirstNode (node)) {
				GUI.DrawTexture (node.IndicatorBounds, UTEditorResources.FirstNodeTexture);
			}
			if (node.Data is UTAutomationPlanPlanEntry) {
				GUI.DrawTexture( node.SecondaryIndicatorBounds, UTEditorResources.ExecutePlanTexture);
			}
		}

		foreach (var connector in node.Connectors) {
			if (phase == NodeDrawingPhase.DrawNodes) {
				GUI.Label (connector.labelPosition, connector.label, connector.labelStyle);
				GUI.Box (connector.connectorPosition, "", 
					connector.isConnected ? UTEditorResources.GraphNodeConnectorStyle : 
					UTEditorResources.GraphNodeConnectorStyleEmpty);
			}
			if (phase == NodeDrawingPhase.DrawLines) {
				var targetNode = editorModel.GetReferencedNode (node, connector.property);
				if (targetNode != null) {
					UTLineUtils.DrawLine (connector.connectorPosition.center, targetNode.Bounds.center);
				}
			}
		}
		

		if (phase == NodeDrawingPhase.DrawNodes) {
			GUI.Label (node.Bounds, node.Label, UTEditorResources.GraphNodeHeaderStyle);
			if (!string.IsNullOrEmpty(node.Text)) {
				GUI.Label(node.TextBounds, node.Text, UTEditorResources.GraphNodeTextStyle);
			}
		}		
	}
	
	private static void AddDecisionNode (object userData)
	{
		var data = (NodeEventData)userData;
		data.model.AddEntry<UTAutomationPlanDecisionEntry> (data.evt.mousePosition);
	}

	private static void AddForEachNode (object userData)
	{
		var data = (NodeEventData)userData;
		data.model.AddEntry<UTAutomationPlanForEachEntry> (data.evt.mousePosition);
	}

	private static void AddForEachFileNode (object userData)
	{
		var data = (NodeEventData)userData;
		data.model.AddEntry<UTAutomationPlanForEachFileEntry> (data.evt.mousePosition);
	}
	
	private static void AddActionNode (object userData)
	{
		var data = (NodeEventData)userData;
		data.model.AddEntry<UTAutomationPlanSingleActionEntry> (data.evt.mousePosition);
	}
	
	private static void AddPlanNode (object userData)
	{
		var data = (NodeEventData)userData;
		data.model.AddEntry<UTAutomationPlanPlanEntry> (data.evt.mousePosition);
	}

	private static void AddNote (object userData)
	{
		var data = (NodeEventData)userData;
		data.model.AddEntry<UTAutomationPlanNoteEntry> (data.evt.mousePosition);
	}

	private static void DeleteNode (object userData)
	{
		var data = (NodeEventData)userData;
		data.model.DeleteNodes (data.model.SelectedNodes);
	}
	
	private static void DeleteConnection (object userData)
	{
		var data = (NodeEventData)userData;
		data.model.DeleteConnection (data.connector);
	}
	
	private static void SetAsFirstNode (object userData)
	{
		var data = (NodeEventData)userData;
		data.model.SetFirstNode (data.model.SelectedNodes [0]);
	}
	
	private static void ShowPopup (Event evt, UTNodeEditorModel model)
	{
		var menu = new GenericMenu ();
		if (!model.HasPlan) {
			menu.AddDisabledItem (new GUIContent ("Please select an automation plan."));
			menu.ShowAsContext ();
			return;
		}


		var hotNode = GetNodeUnderMouse (model, evt.mousePosition);
		var hotConnector = GetConnectorUnderMouse (model, evt.mousePosition);
		var data = new NodeEventData (evt, model, hotConnector);
		if (hotNode == null && hotConnector == null) {
#if UTOMATE_DEMO
			if (!model.CanAddEntriesToPlan) {
				menu.AddDisabledItem (new GUIContent ("You cannot add any more entries in the demo version."));
			}
			else {
#endif 
			menu.AddItem (new GUIContent ("Add Action Entry"), false, AddActionNode, data);
			menu.AddItem (new GUIContent ("Add Decision Entry"), false, AddDecisionNode, data);
			menu.AddItem (new GUIContent ("Add For-Each Entry"), false, AddForEachNode, data);
			menu.AddItem (new GUIContent ("Add For-Each File Entry"), false, AddForEachFileNode, data);
			menu.AddItem (new GUIContent ("Add Sub-Plan Entry"), false, AddPlanNode, data);
			menu.AddItem (new GUIContent ("Add Note"), false, AddNote, data);

#if UTOMATE_DEMO
			}
#endif
		}
		
		if (hotNode != null) {
			if (!model.SelectedNodes.Contains (hotNode)) {
				model.SelectNode (hotNode, UTNodeEditorModel.SelectionMode.Replace);
			}
			
			if (menu.GetItemCount () > 0) {
				menu.AddSeparator ("");
			}
			
			var isMultiSelection = model.SelectedNodes.Count > 1;
			
			if (!model.IsFirstNode (hotNode) && !isMultiSelection) {
				menu.AddItem (new GUIContent ("Set As First Entry"), false, SetAsFirstNode, data);
			}
			menu.AddItem (new GUIContent (isMultiSelection ? "Delete Entries" : "Delete Entry"), false, DeleteNode, data);
		}
		
		if (hotConnector != null && hotConnector.isConnected) {
			if (menu.GetItemCount () > 0) {
				menu.AddSeparator ("");
			}
			menu.AddItem (new GUIContent ("Delete Connection"), false, DeleteConnection, data);
		}
		menu.ShowAsContext ();
	}
	
	private static Rect ToRect (Vector2 v1, Vector2 v2)
	{
		return new Rect (Mathf.Min (v1.x, v2.x),
						Mathf.Min (v1.y, v2.y), 
						Mathf.Abs (v1.x - v2.x),
						Mathf.Abs (v1.y - v2.y));	
	}
	
	
	internal sealed class UTNodeEditorState
	{
		public bool isDropTarget;
		public UTNode hotNode;
		public UTNode.Connector sourceConnector;
		public Vector2 lastMousePosition;
		public bool isDrawingSelection;
		public bool delayedSelectionMode;
		public bool dragThresholdReached;
			
	}
	
	internal struct NodeEventData
	{
		public Event evt;
		public UTNodeEditorModel model;
		public UTNode.Connector connector;
		
		public NodeEventData (Event evt, UTNodeEditorModel model, UTNode.Connector connector)
		{
			this.evt = evt;
			this.model = model;
			this.connector = connector;
		}
		
	}
	
	internal enum NodeDrawingPhase
	{
		DrawLines,
		DrawNodes
	}
	
}

