//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;

/// <summary>
/// Enum for visibility decisions.
/// </summary>
public enum UTVisibilityDecision
{
	/// <summary>
	/// The field is visible
	/// </summary>
	Visible,
	/// <summary>
	/// The field is not visible.
	/// </summary>
	Invisible,
	/// <summary>
	/// The delegate cannot determine whether or not the field is visible. This will trigger the default behaviour
	/// in UTInspectorRenderer.
	/// </summary>
	Undetermined

}

