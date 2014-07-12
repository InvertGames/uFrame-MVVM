//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;

/// <summary>
/// Interface for inspector property renderers.
/// </summary>
public interface UTIPropertyRenderer
{
	/// <summary>
	/// Renders the inspector property described by the given field wrapper. This renderer is only called
	/// when the property is not set to expression mode, so the renderer simply renders the non-expression
	/// mode specific property view. The renderer must read and set the Value property of the given field wrapper.
	/// 
	/// Undo/Redo is handled by the framework and does not need to be handled by the renderer.
	/// 
	/// </summary>
	/// <param name='fieldWrapper'>
	/// Field wrapper.
	/// </param>
	void Render (UTFieldWrapper fieldWrapper);
}

