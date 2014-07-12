//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class holding information about the node graph. It provides functions for finding nodes and 
/// references in the graph and modifying the graph. Note that this class is merely working on the
/// graphical representation of an automation plan and not the actual automation plan.
/// </summary>
public class UTGraph : ScriptableObject {
	
	/// <summary>
	/// The nodes in the graph.
	/// </summary>
	[SerializeField]
	public List<UTNode> nodes;
	
	/// <summary>
	/// The references in the graph (aka edges).
	/// </summary>
	[SerializeField]
	public List<UTReference> references;

#if UTOMATE_DEMO
	public int Count {
		get {
			if ( nodes == null) {
				return 0;
			}
			return nodes.Count;
		}
	}
#endif 

	public IList<UTNode> Nodes {
		get {
			if (nodes == null) {
				nodes = new List<UTNode>();
			}
			return nodes.AsReadOnly();
		}
	}
	
	public IList<UTReference> References {
		get {
			if (references == null) {
				references = new List<UTReference>();
			} 
			return references.AsReadOnly();
		}
	}
	
	/// <summary>
	/// Adds the node to the graph.
	/// </summary>
	/// <param name='node'>
	/// The node to add.
	/// </param>/
	public void AddNode(UTNode node) {
		if (nodes == null) {
			nodes = new List<UTNode>();
		}
		nodes.Add(node);
	}
	
	/// <summary>
	/// Deletes a node from the graph. Also deletes all references pointing from and to that node.
	/// </summary>
	/// <param name='node'>
	/// The node to delete.
	/// </param>
	public void DeleteNode(UTNode node) {
		if (nodes == null || !nodes.Contains(node)) {
			throw new ArgumentException("Trying to delete non-existing node.");
		}
		var sourceRefs = GetSourceReferences(node.Data);
		var targetRefs  = GetTargetReferences(node.Data);
		
		foreach(var reference in sourceRefs) {
			DeleteReference(reference);
		}
		foreach(var reference in targetRefs) {
			DeleteReference(reference);
		}
		
		nodes.Remove(node);
	}
	
	/// <summary>
	/// Adds a reference to the graph.
	/// </summary>
	/// <param name='reference'>
	/// Reference to be added.
	/// </param>
	public void AddReference(UTReference reference) {
		if (references == null) {
			references = new List<UTReference>();
		}
		references.Add(reference);
	}
	
	/// <summary>
	/// Deletes a reference from the graph.
	/// </summary>
	/// <param name='reference'>
	/// Reference to delete.
	/// </param>
	public void DeleteReference(UTReference reference) {
		if (references == null || !references.Contains(reference)) {
			throw new ArgumentException("Trying to delete non-existing reference.");
		}
		references.Remove(reference);
	}
	
	/// <summary>
	/// Gets the node representing a given automation plan entry.
	/// </summary>
	/// <returns>
	/// The node representing the given entry or null if no such node exists.
	/// </returns>
	/// <param name='entry'>
	/// The entry to look up.
	/// </param>
	public UTNode GetNodeFor(UTAutomationPlanEntry entry) {
		if (nodes == null) {
			nodes = new List<UTNode>();
		}
		return nodes.Find(node => node.Data == entry);
	}
	
	/// <summary>
	/// Gets all references which are originating from the given entry.
	/// </summary>
	public IList<UTReference> GetSourceReferences(UTAutomationPlanEntry entry) {
		if (references == null) {
			references = new List<UTReference>();
		}
		return references.FindAll(reference => reference.Source == entry);
	}
	
	/// <summary>
	/// Gets all references which are pointing to the given entry.
	/// </summary>
	public IList<UTReference> GetTargetReferences(UTAutomationPlanEntry entry) {
		if (references == null) {
			references = new List<UTReference>();
		}
		return references.FindAll(reference => reference.Target == entry);
	}
	
	/// <summary>
	/// Gets the reference which represents the given property of the given automation plan entry.
	/// </summary>
	/// <returns>
	/// The reference or null if no such reference exists.
	/// </returns>
	/// <param name='entry'>
	/// The automation plan entry to look the reference up for.
	/// </param>
	/// <param name='property'>
	/// The property of the automation plan entry which is represented by the reference.
	/// </param>
	public UTReference GetReference(UTAutomationPlanEntry entry, string property) {
		if (references == null) {
			references = new List<UTReference>();
		}
		return references.Find(reference => reference.Source == entry && reference.SourceProperty == property);
	}
	
	
	/// <summary>
	/// Finds the nodes which are located inside a given rect. Used for selecting nodes.
	/// </summary>
	/// <returns>
	/// The nodes in rect.
	/// </returns>
	/// <param name='rect'>
	/// The rect.
	/// </param>
	public IList<UTNode> FindNodesInRect(Rect rect) {
		return nodes.FindAll (node => 
			node.Bounds.xMin <= rect.xMax &&
			node.Bounds.xMax >= rect.xMin &&
			node.Bounds.yMin <= rect.yMax &&
			node.Bounds.yMax >= rect.yMin
		);
	}
}

