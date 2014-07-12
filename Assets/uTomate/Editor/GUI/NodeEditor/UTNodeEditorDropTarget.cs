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

/// <summary>
/// Drop target implementation for the node editor.
/// </summary>
public class UTNodeEditorDropTarget {
	
	private DropActionsDelegate dropActionsHandler;
	private UTNodeEditorModel model;
	
	public UTNodeEditorDropTarget(DropActionsDelegate dropActionsHandler, UTNodeEditorModel model) {
		this.dropActionsHandler = dropActionsHandler;
		this.model = model;
	}
	
	public bool CanDrop() {
		if (!model.HasPlan) {
			return false;
		}

		UnityEngine.Object[] dragContent = DragAndDrop.objectReferences;
		if (dragContent == null || dragContent.Length == 0) {
			return false;
		}
		
		foreach(UnityEngine.Object content in dragContent) {
			if (!(content is UTAction)  && !(content is UTAutomationPlan)) {
				return false;
			}
		}
		DragAndDrop.visualMode = DragAndDropVisualMode.Link;
		return true;
	}
	
	public void AcceptDrop(Vector2 position) {
		if (CanDrop()) {
			UObject[] items = new UObject[DragAndDrop.objectReferences.Length];
			for(int i = 0; i < DragAndDrop.objectReferences.Length; i++) {
				items[i] = (UObject) DragAndDrop.objectReferences[i];
			}
			
			dropActionsHandler(items, position);
			DragAndDrop.AcceptDrag();
		}

	}
	
	public delegate void DropActionsDelegate(UObject[] items, Vector2 position);

}

