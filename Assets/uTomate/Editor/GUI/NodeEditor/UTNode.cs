//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// Class which represents a node in the node editor. This class calculates the location, size and labelling of 
/// a node and it's connectors. A node is the graphical representation of a <see cref="UTAutomationPlanEntry"/>
/// </summary> 
[Serializable]
public class UTNode {
	
	[SerializeField]
	private Rect bounds;
	
	// these are transient
	private bool initialized = false;
	private Connector[] connectors;
	private Rect indicatorBounds;
	private Rect secondaryIndicatorBounds;
	private Rect textBounds;
	
	public string Label {
		get {
			return Data.Label;
		}
	}
	
	public string Text {
		get {
			return Data.Text;
		}
	}
	
	
	public Rect Bounds {
		get { return bounds; }
		set { 
			bounds = value; 
			initialized = false;
		}
	}
	
	public Rect IndicatorBounds {
		get {
			return indicatorBounds;
		}
	}
	
	public Rect SecondaryIndicatorBounds {
		get {
			return secondaryIndicatorBounds;
		}
	}
	
	public Rect TextBounds {
		get {
			return textBounds;
		}
	}
	
	public Connector[] Connectors {
		get {
			if (!initialized) {
				Initialize();
			}
			return connectors;
		}
	}
	
	[SerializeField]
	private UTAutomationPlanEntry data;
	
	public UTAutomationPlanEntry Data {
		get {return data;}
		set {
			data = value;
			initialized = false;
		}
	}
	
	public void Recalc() {
		initialized = false;
	}
	
	
	private void Initialize() {
		if ( data == null) {
			return;
		}
		var type = data.GetType();
		var fields = type.GetFields();
		List<Connector> connectorList = new List<Connector>();
		
		var left = bounds.x;
		var top = bounds.y + 32;
		var connectorWidth = UTEditorResources.GraphNodeConnectorStyle.fixedWidth;
		var connectorHeight = UTEditorResources.GraphNodeConnectorStyle.fixedHeight;
		
		indicatorBounds = new Rect(bounds.x, bounds.y, 32, 32 );
		secondaryIndicatorBounds = new Rect(bounds.xMax - 23, bounds.y - 13, 36, 38);
		
		var connectorsLeft = new List<Connector>();
		var connectorsRight = new List<Connector>();
		var connectorsBottom = new List<Connector>();
		var maxLeftLabelWidth = 0;
		var maxRightLabelWidth = 0;
		var maxBottomLabelWidth = 0;
		
		var titleLabelWidth = (int)UTEditorResources.GraphNodeHeaderStyle.CalcSize(new GUIContent(Label)).x;
		
		
		foreach( var field in fields ) {
			if (field.FieldType == typeof(UTAutomationPlanEntry)) {
				var connector = new Connector();
				connector.label = ObjectNames.NicifyVariableName(field.Name);
				connector.property = field.Name;
				var val = field.GetValue(data);
				connector.isConnected = val != null;
				connector.owner = this;
				
				connectorList.Add (connector);
				
				var hint = UTConnectorHint.GetFor(field);
				
				Vector2 labelSize;
				switch(hint.connectorLocation) {
				case UTConnectorHint.ConnectorLocation.Left:
					labelSize = UTEditorResources.GraphNodeConnectorLabelLeftStyle.CalcSize(new GUIContent(connector.label));
					connector.labelStyle = UTEditorResources.GraphNodeConnectorLabelLeftStyle;
					connectorsLeft.Add(connector);
					maxLeftLabelWidth = Mathf.Max (maxLeftLabelWidth, (int)labelSize.x);
					break;
				case UTConnectorHint.ConnectorLocation.Right:
					labelSize = UTEditorResources.GraphNodeConnectorLabelRightStyle.CalcSize(new GUIContent(connector.label));
					connector.labelStyle = UTEditorResources.GraphNodeConnectorLabelRightStyle;
					connectorsRight.Add(connector);
					maxRightLabelWidth = Mathf.Max (maxRightLabelWidth, (int)labelSize.x);
					break;
				case UTConnectorHint.ConnectorLocation.Bottom:
					labelSize = UTEditorResources.GraphNodeConnectorLabelCenteredStyle.CalcSize(new GUIContent(connector.label));
					connector.labelStyle = UTEditorResources.GraphNodeConnectorLabelCenteredStyle;
					connectorsBottom.Add(connector);
					maxBottomLabelWidth = Mathf.Max (maxBottomLabelWidth, (int)labelSize.x);
					break;
				}
			}
		}
		
		var nodeWidth = Mathf.Max(200, Mathf.Max(maxLeftLabelWidth + maxRightLabelWidth, maxBottomLabelWidth* connectorsBottom.Count, titleLabelWidth));
		var textHeight = 0f;
		if (!string.IsNullOrEmpty(Text)) {
			textHeight = UTEditorResources.GraphNodeTextStyle.CalcHeight(new GUIContent(Text), nodeWidth);
		}
		
		var nodeHeight = Mathf.Max (25, 
				20 * ( Mathf.Max(connectorsLeft.Count, connectorsRight.Count) + (connectorsBottom.Count > 0 ? 1 : 0) ) +
				textHeight
			);
		
		
		var right = left + nodeWidth;
		
		var theTop = top;
		foreach( var connector in connectorsLeft) {
			connector.labelPosition = new Rect( left, theTop, maxLeftLabelWidth, 20);
			connector.connectorPosition = new Rect( left - connectorWidth/2, theTop, connectorWidth, connectorHeight);
			theTop += 20;
		}
		
		theTop = top;
		foreach( var connector in connectorsRight ) {
			connector.labelPosition = new Rect( right - maxRightLabelWidth, theTop, maxRightLabelWidth, 20);
			connector.connectorPosition = new Rect( right - connectorWidth/2, theTop, connectorWidth, connectorHeight);
			theTop += 20;
		}
		
		theTop = top;
		if (textHeight > 0f) {
			var topOffset = 20 * ( Mathf.Max(connectorsLeft.Count, connectorsRight.Count));
			textBounds = new Rect(left, theTop + topOffset, nodeWidth, textHeight );
		}
		
		if ( connectorsBottom.Count > 0) {
			var effectiveBottomLabelWidth = nodeWidth / connectorsBottom.Count;
			theTop = top + nodeHeight - 20;
			var theLeft = left;
			foreach( var connector in connectorsBottom) {
				connector.labelPosition = new Rect( theLeft, theTop, effectiveBottomLabelWidth, 20);
				connector.connectorPosition = new Rect( theLeft + effectiveBottomLabelWidth/2 - connectorWidth/2, theTop + 20 - connectorHeight / 2, connectorWidth, connectorHeight);
				theLeft += effectiveBottomLabelWidth;
			}
		}
		
		bounds.height = 32 + nodeHeight;
		bounds.width = nodeWidth;
		
		connectors = connectorList.ToArray();
	}
	
	/// <summary>
	/// Inner class containing information about the location, label and property of a node's connector.
	/// </summary>
	public class Connector {
		public string property;
		public string label;
		public GUIStyle labelStyle;
		public Rect labelPosition;
		public Rect connectorPosition;
		public UTNode owner;
		public bool isConnected;
	}
}

