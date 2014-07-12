//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//
using System;
using UnityEngine;
using UnityEditor;
using UObject = UnityEngine.Object;
using System.Collections.Generic;

/// <summary>
/// The underlying data model for the node editor. Basically allows modifying
/// the automation plan with functions suitable for visual editing. This model transparently handles
/// Undo/Redo.
/// </summary>
public class UTNodeEditorModel
{
	 
	private UTAutomationPlan data;
	private UTGraph graphData;
	private List<UTNode> selectedNodes = new List<UTNode> ();
	private UTNode highlightedNode;

	/// <summary>
	/// Is there currently a plan loaded?
	/// </summary>
	public bool HasPlan {
		get {
			return data != null && graphData != null;
		}
	}

#if UTOMATE_DEMO
	public bool CanAddEntriesToPlan {
		get {
			if (!HasPlan) {
				return false;
			}
			return graphData.Count < UTomate.MaxEntriesForDemo;
		}
	}

	public bool ExceedsDemoEntryLimit {
		get {
			if (!HasPlan) {
				return false;
			}
			return graphData.Count > UTomate.MaxEntriesForDemo;
		}
	}
#endif

	/// <summary>
	/// Gets the graph data holding the visual representation of the automation plan.
	/// </summary>
	public UTGraph Graph {
		get {
			return graphData;
		}
	}
	
	/// <summary>
	/// Gets the plan that is currently loaded.
	/// </summary>
	public UTAutomationPlan Plan {
		get {
			return data;
		}
	}
	
	/// <summary>
	/// Gets the full path to the currently loaded plan.
	/// </summary>
	private string PlanPath {
		get {
			return data != null ? AssetDatabase.GetAssetPath (data) : null;
		}
	}
	
	/// <summary>
	/// Gets the currently selected nodes.
	/// </summary>
	public List<UTNode> SelectedNodes {
		get {
			return selectedNodes;
		}
	}
	
	/// <summary>
	/// Gets the currently selected entry. If no or multiple nodes are selected, returns null.
	/// </summary>
	public UTAutomationPlanEntry SelectedEntry {
		get {
			if (selectedNodes != null) {
				if (selectedNodes.Count == 1) {
					return selectedNodes [0].Data;
				}
			}
			return null;
		}
	}
	
	/// <summary>
	/// Gets the currently highlighted node. A node is highlighted, when an automation plan is executed.
	/// The highlighted node is the visual representation of the currently executed automation plan entry.
	/// </summary>
	public UTNode HighlightedNode {
		get {
			return highlightedNode;
		}
	}
	
	/// <summary>
	/// Sets the highlighted entry. The given entry is currently being executed.
	/// </summary>
	public UTAutomationPlanEntry HighlightedEntry {
		set {
			if (HasPlan) {
				if (value == null) {
					highlightedNode = null;
				} else {
					// might be null if the loaded plan is different from the currently executed plan.
					highlightedNode = graphData.GetNodeFor (value);
				}
			}
		}
	}
	
	/// <summary>
	/// Loads a plan into this model.
	/// </summary>
	/// <param name='plan'>
	/// The plan to load.
	/// </param>
	public void LoadPlan (UTAutomationPlan plan)
	{
		if (plan != null) {
			data = plan;
			string path = PlanPath;
			graphData = null;
			selectedNodes.Clear ();
				
			// objects with hide fagl are not returned by LoadAssetAtPath
			UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath (path);
			foreach (UnityEngine.Object asset in assets) {
				if (asset is UTGraph) {						
					graphData = (UTGraph)asset;
					break;
				}
			}
			if (graphData == null) {
				graphData = UTils.AddAssetOfType<UTGraph> (path, true);
				graphData.name = "Graph";
				EditorUtility.SetDirty (graphData);
			}
			if (plan.firstEntry != null) {
				SelectNode (graphData.GetNodeFor (plan.firstEntry), SelectionMode.Add);
			}
		} else {
			data = null;
			graphData = null;
			selectedNodes.Clear ();
			highlightedNode = null;
		}
	}
	
	/// <summary>
	/// Adds an action at the given location. Internally creates a new automation plan entry and adds this to the 
	/// automation plan. Also creates a <see cref="UTNode"/> instance representing the new automation plan entry.
	/// </summary>
	/// <param name='action'>
	/// Action to add.
	/// </param>
	/// <param name='position'>
	/// Position at which the action should be added.
	/// </param>
	public void AddAction (UTAction action, Vector2 position)
	{
		var entry = AddEntry<UTAutomationPlanSingleActionEntry> (position);
		entry.action = action;
		
	}
	
	/// <summary>
	/// Adds a plan as a subplan to the automation plan at the given location. Internally creates a new automation
	/// plan entry and adds it to the auotmation plan. Also creates a <see cref="UTNode"/> instance representing the 
	/// new automation plan entry.
	/// </summary>
	/// <param name='plan'>
	/// The plan to add.
	/// </param>
	/// <param name='position'>
	/// Position at which the plan should be added.
	/// </param>
	public void AddSubPlan (UTAutomationPlan plan, Vector2 position)
	{
		var entry = AddEntry<UTAutomationPlanPlanEntry> (position);
		entry.plan = plan;
	}
	
	/// <summary>
	/// Adds an arbitrary entry at the given position.
	/// </summary>
	/// <returns>
	/// The entry that was added.
	/// </returns>
	/// <param name='position'>
	/// Position at which the entry should be added.
	/// </param>
	/// <typeparam name='T'>
	/// The type of the entry to be added.
	/// </typeparam>
	public T AddEntry<T> (Vector2 position) where T:UTAutomationPlanEntry
	{
		CUUndoUtility.RegisterUndo (new UObject[]{data, graphData}, "Add Node");
		var entry = UTils.AddAssetOfType<T> (PlanPath, true);
		
		entry.automationPlanEntryId = Guid.NewGuid ().ToString ();
		if (data.firstEntry == null) {
			data.firstEntry = entry;
		}
		
		UTNode node = new UTNode ();
		node.Data = entry;
		node.Bounds = new Rect (position.x, position.y, 200, 200);	
		graphData.AddNode (node);
			
		// add offset for next node
		position.x += 50;
		position.y += 50;
		EditorUtility.SetDirty (entry);
		EditorUtility.SetDirty (graphData);
		SelectNode (node, SelectionMode.Replace);
		return entry;
	}
	
	/// <summary>
	/// Deletes a set of nodes. Also removes the related automation plan entries from the automation plan.
	/// </summary>
	/// <param name='nodes'>
	/// Nodes to be deleted.
	/// </param>
	public void DeleteNodes (IList<UTNode> nodes)
	{
		List<UTNode> copy = new List<UTNode> (nodes);
		List<UObject> toUndo = new List<UObject> ();
		toUndo.Add (data);
		toUndo.Add (graphData);
		
		foreach (var node in copy) {
			var targetRefs = graphData.GetTargetReferences (node.Data);
			foreach (var targetRef in targetRefs) {
				if (!toUndo.Contains (targetRef.Source)) {
					toUndo.Add (targetRef.Source);
				}
			}
		}
		CUUndoUtility.RegisterUndo (toUndo.ToArray (), "Delete nodes");
		
		foreach (var node in copy) {
			var targetRefs = graphData.GetTargetReferences (node.Data);
			
			foreach (var reference in targetRefs) {
				// kill reference in node
				SetProperty (reference.Source, reference.SourceProperty, null);
				var modifiedNode = graphData.GetNodeFor (reference.Source);
				modifiedNode.Recalc ();
			} 
			
			graphData.DeleteNode (node);
			
			if (data.firstEntry == node.Data) {
				data.firstEntry = null;
				EditorUtility.SetDirty (data);
			}
		
			selectedNodes.Remove (node);
			// UObject.DestroyImmediate(node.Data, true);

		}
		EditorUtility.SetDirty (graphData);
	}
	
	/// <summary>
	/// Adds a connection between a node's connector and another node.
	/// </summary>
	/// <param name='sourceConnector'>
	/// Source connector.
	/// </param>
	/// <param name='targetNode'>
	/// Target node.
	/// </param>
	public void AddConnection (UTNode.Connector sourceConnector, UTNode targetNode)
	{
		var sourceEntry = sourceConnector.owner.Data;
		var property = sourceConnector.property;
		var targetEntry = targetNode.Data;
		var sourceNode = sourceConnector.owner;
		
		CUUndoUtility.RegisterUndo (new UObject[]{sourceEntry, graphData}, "Add connector");
		var reference = graphData.GetReference (sourceEntry, property);
		if (reference == null) {
			reference = new UTReference ();
			reference.Source = sourceEntry;
			reference.SourceProperty = property;
			graphData.AddReference (reference);
		}
		reference.Target = targetEntry;
		SetProperty (sourceEntry, property, targetEntry);
		sourceNode.Recalc ();
		EditorUtility.SetDirty (sourceEntry);
		EditorUtility.SetDirty (graphData);
	}
	
	/// <summary>
	/// Deletes the connection between a node's connector and the target node.
	/// </summary>
	/// <param name='connector'>
	/// Connector for which the connection should be deleted.
	/// </param>
	public void DeleteConnection (UTNode.Connector connector)
	{
		var sourceEntry = connector.owner.Data;
		var sourceNode = connector.owner;
		CUUndoUtility.RegisterUndo (new UObject[]{sourceEntry, graphData}, "Remove connector");
		
		var reference = graphData.GetReference (connector.owner.Data, connector.property);
		SetProperty (sourceEntry, connector.property, null);
		sourceNode.Recalc ();
		graphData.DeleteReference (reference);
		EditorUtility.SetDirty (sourceEntry);
		EditorUtility.SetDirty (graphData);
	}
	
	/// <summary>
	/// Reflectively sets a property. Used by the functions adding and removing connectors, to
	/// set the references between automation plan entries.
	/// </summary>
	/// <param name='source'>
	/// The object on which the property should be set.
	/// </param>
	/// <param name='property'>
	/// Property to set.
	/// </param>
	/// <param name='val'>
	/// Value of the property.
	/// </param>
	private static void SetProperty (object source, string property, object val)
	{
		var sourceType = source.GetType ();
		var sourceProperty = sourceType.GetField (property);
		sourceProperty.SetValue (source, val);
		
	}
	
	/// <summary>
	/// Called by the node editor before moving nodes. Creates an undo stack frame, so the movement can be undone later.
	/// </summary>
	public void StartMovingNodes ()
	{
		CUUndoUtility.RegisterUndo (new UObject[]{data, graphData}, "Move nodes");
		EditorUtility.SetDirty (data);
		EditorUtility.SetDirty (graphData);
	}
	
	/// <summary>
	/// Relayouts the plan.
	/// </summary>
	/// <seealso cref="UTGraphLayouter"/>
	public void RelayoutPlan ()
	{
		CUUndoUtility.RegisterUndo (new UObject[]{data, graphData}, "Layout automation plan");
		UTGraphLayouter layouter = new UTGraphLayouter ();
		var node = graphData.GetNodeFor (data.firstEntry);
		layouter.Relayout (graphData, node);
		EditorUtility.SetDirty (data);
		EditorUtility.SetDirty (graphData);
	}
	
	/// <summary>
	/// Gets all nodes currently known.
	/// </summary>
	/// <returns>
	/// The nodes.
	/// </returns>
	public IList<UTNode> GetNodes ()
	{
		if (graphData != null) {
			return graphData.Nodes;
		}
		return new List<UTNode> ();
	}
	
	/// <summary>
	/// Gets all references currently known.
	/// </summary>
	/// <returns>
	/// The references.
	/// </returns>
	public IList<UTReference> GetReferences ()
	{
		if (graphData != null) {
			return graphData.References;
		}
		return new List<UTReference> ();
	}
	
	/// <summary>
	/// Gets the node referenced from a certain node by a connector representing a certain property.
	/// </summary>
	/// <returns>
	/// The referenced node or null if no node is referenced.
	/// </returns>
	/// <param name='source'>
	/// The source node.
	/// </param>
	/// <param name='sourceProperty'>
	/// The property of the automation plan entry represented by the node that holds the reference to another node/entry.
	/// </param>
	public UTNode GetReferencedNode (UTNode source, string sourceProperty)
	{
		var reference = graphData.GetReference (source.Data, sourceProperty);
		if (reference != null) {
			return graphData.GetNodeFor (reference.Target);
		}
		return null;
	}
	
	
	/// <summary>
	/// Determines whether the given node is the first node in the automation plan..
	/// </summary>
	/// <param name='node'>
	/// The node to check.
	/// </param>
	public bool IsFirstNode (UTNode node)
	{
		return data.firstEntry == node.Data;
	}
	
	/// <summary>
	/// Sets the first node  (entry) of the automation plan.
	/// </summary>
	/// <param name='node'>
	/// Node which should be the first node.
	/// </param>
	public void SetFirstNode (UTNode node)
	{
		data.firstEntry = node.Data;
		node.Recalc ();
		EditorUtility.SetDirty (data);
	}
	
	/// <summary>
	/// Selects the nodes in the given rect.
	/// </summary>
	/// <param name='rect'>
	/// Rect.
	/// </param>
	/// <param name='mode'>
	/// The selection mode.
	/// </param>
	public void SelectNodesInRect (Rect rect, SelectionMode mode)
	{
		var nodes = graphData.FindNodesInRect (rect);
		if (mode == SelectionMode.Replace) {
			selectedNodes.Clear ();
			mode = SelectionMode.Add;
		}
		
		foreach (var node in nodes) {
			SelectNode (node, mode);
		}
	}
	
	/// <summary>
	/// Selects a single node
	/// </summary>
	/// <param name='node'>
	/// Node to select.
	/// </param>
	/// <param name='mode'>
	/// The selection node.
	/// </param>
	public void SelectNode (UTNode node, SelectionMode mode)
	{
		switch (mode) {
		case SelectionMode.Replace:
			selectedNodes.Clear ();
			selectedNodes.Add (node);
			SelectNodeInInspector (node);
			break;
		case SelectionMode.Add:
			selectedNodes.Add (node);
			break;
		case SelectionMode.Subtract:
			selectedNodes.Remove (node);
			break;
		}
	}
	
	public void SelectNodeInInspector (UTNode node)
	{
		var actionEntry = node.Data as UTAutomationPlanSingleActionEntry;
		if (actionEntry != null) {
			Selection.activeObject = actionEntry.action;
			return;
		} 
		
		var planEntry = node.Data as UTAutomationPlanPlanEntry;
		if (planEntry != null) {
			Selection.activeObject = planEntry.plan;
		}
	}
	
	
	public enum SelectionMode
	{
		/// <summary>
		/// Add the node(s) to the current selection.
		/// </summary>
		Add,
		/// <summary>
		/// Subtract the node(s) from the current selection.
		/// </summary>
		Subtract,
		/// <summary>
		/// Replace the current selection with the given node(s).
		/// </summary>
		Replace
	}
	
}


