//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class for auto-layouting an automation plan.
/// </summary>
public class UTGraphLayouter
{
	
	private UTGraph graph;
	private List<List<UTNode>> columns = new List<List<UTNode>>();
	private	List<List<UTNode>> rows = new List<List<UTNode>>();
	private HashSet<UTNode> visitedNodes = new HashSet<UTNode>();
	
	
	// Layout-Rules:
	// Comments stay where they are
	// unconnected nodes stay where they are
	// Decision nodes are centered above their following nodes.
	// For-Each/For-Each-File: Subplan is put right
	// all other nodes are layouted top down.

	public void Relayout(UTGraph graph, UTNode startingNode) {
		if ( startingNode == null) {
			return;
		}

		this.graph = graph;
		columns.Clear();
		rows.Clear();
		visitedNodes.Clear();
		ProcessNode(startingNode, 0, 0);
		DoLayout();
	}
	
	private void DoLayout() {
		var offset = 20f;
		foreach(var nodes in columns) {
			var maxWidth = 0f;
			foreach(var node in nodes) {
				maxWidth = Mathf.Max(maxWidth, node.Bounds.width);
			}
			foreach(var node in nodes) {
				node.Bounds = new Rect(offset + ( maxWidth - node.Bounds.width) / 2, node.Bounds.y, node.Bounds.width, node.Bounds.height);
			}
			offset += maxWidth + 20f;
		}
		
		offset = 40f;
		foreach(var nodes in rows) {
			var maxHeight = 0f;
			foreach(var node in nodes) {
				maxHeight = Mathf.Max(maxHeight, node.Bounds.height);
			}
			foreach(var node in nodes) {
				node.Bounds = new Rect(node.Bounds.x, offset, node.Bounds.width, node.Bounds.height);
			}
			offset += maxHeight + 20f;
		}
		
		foreach( var node in visitedNodes) {
			node.Recalc();
		}
	}
	
	private int ProcessNode(UTNode node, int currentRow, int currentColumn) {
		if (visitedNodes.Contains(node)) {
			return currentRow;
		}
		visitedNodes.Add(node);
		var entry = node.Data;
		
		if ( entry is UTAutomationPlanForEachEntry || entry is UTAutomationPlanForEachFileEntry) {
			return ProcessForEachNode(node, currentRow, currentColumn);
		}
		else
		if ( entry is UTAutomationPlanDecisionEntry ) {
			return ProcessDecisionNode(node, currentRow, currentColumn);
		}
		else {
			return ProcessSimpleEntry(node, currentRow, currentColumn);
		}
	}
	
	private int ProcessSimpleEntry(UTNode node, int currentRow, int currentColumn) {
		AddToColumn(node, currentColumn);
		AddToRow(node, currentRow);
		
		UTAutomationPlanEntry entry = node.Data;
		var nextEntry = entry.NextEntry;
		if ( nextEntry != null) {
			return ProcessNode(graph.GetNodeFor(nextEntry), currentRow+1, currentColumn);
		}
		return currentRow;
	}
	
	private int ProcessForEachNode(UTNode node, int currentRow, int currentColumn) {
		AddToColumn(node, currentColumn);
		AddToRow(node, currentRow);

		var entry = node.Data;
		UTAutomationPlanEntry subTree = null;
		if (entry is UTAutomationPlanForEachEntry) {
			subTree = ((UTAutomationPlanForEachEntry)entry).startOfSubtree;
		}
		if (entry is UTAutomationPlanForEachFileEntry) {
			subTree = ((UTAutomationPlanForEachFileEntry)entry).startOfSubtree;
		}
		if (subTree != null) {
			var subTreeNode = graph.GetNodeFor(subTree);
			EnsureEmptyColumn(currentColumn+1);
			currentRow = ProcessNode(subTreeNode, currentRow, currentColumn+1);
		}
		
		var nextEntry = entry.NextEntry;
		if ( nextEntry != null) {
			return ProcessNode(graph.GetNodeFor(nextEntry), currentRow+1, currentColumn);
		}
		return currentRow;
	}
	
	private int ProcessDecisionNode(UTNode node, int currentRow, int currentColumn) {
		AddToColumn(node, currentColumn);
		AddToRow(node, currentRow);

		var entry = node.Data;
		var leftEntry = ((UTAutomationPlanDecisionEntry)entry).entryIfTrue;
		var rightEntry = ((UTAutomationPlanDecisionEntry)entry).entryIfFalse;
		
		var deepestRow = currentRow;
		
		if (rightEntry != null) {
			var subTreeNode = graph.GetNodeFor(rightEntry);
			EnsureEmptyColumn(currentColumn+1);
			deepestRow = ProcessNode(subTreeNode, currentRow+1, currentColumn+1);
		}
		
		if ( leftEntry != null) {
			var subTreeNode = graph.GetNodeFor(leftEntry);
			EnsureEmptyColumn(currentColumn);
			deepestRow = Math.Max(deepestRow, ProcessNode(subTreeNode, currentRow+1, currentColumn));
		}
		return deepestRow;
	}
	
	private void EnsureEmptyColumn(int index) {
		while(columns.Count <= index) {
			columns.Add (new List<UTNode>());
		}
		if (columns[index].Count != 0) {
			columns.Insert(index, new List<UTNode>());
		}
	}
	
	private void AddToRow(UTNode node, int row) {
		AddToNodeList(rows, row, node);
	}
	
	private void AddToColumn(UTNode node, int column) {
		AddToNodeList(columns, column, node);
	}
	
	private static void AddToNodeList(List<List<UTNode>> nodeList, int index, UTNode node) {
		while (nodeList.Count <= index) {
			nodeList.Add (new List<UTNode>());
		}
		List<UTNode> nodes = nodeList[index];
		nodes.Add (node);
	}
	
}

