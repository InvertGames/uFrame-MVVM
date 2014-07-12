//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;

/// <summary>
/// Data which is persisted between two draws of the node editor.
/// </summary>
public class UTNodeEditorData {

	public Vector2 scrollPosition = Vector2.zero;
	public Vector2 inspectorScrollPosition = Vector2.zero;
	
	private UTNodeEditorDropTarget dropTarget;
	
	public bool IsDropSupported {
		get { return dropTarget != null; }
	}
	
	public UTNodeEditorDropTarget DropTarget {
		get { return dropTarget; }
		set { dropTarget = value; }
	}
	
}

