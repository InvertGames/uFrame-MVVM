//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Reflection;

/// <summary>
/// Interface for delegate classes which want to override the default rendering process of <see clink="UTInspectorRenderer"/>.
/// </summary>
public interface UTInspectorRendererDelegate
{
	
	/// <summary>
	/// Determines whether or not the given field should be visible.
	/// </summary>
	/// <returns>
	/// <c>Undetermined</c> of the delegate cannot decide whether or not the field should be visible. This will
	/// delegate the visibility decision to the inspector renderer. <c>Visible</c> if the field should be visible.
	/// <c>Invisible</c> if the field should not be visible. Return <c>Undetermined</c> for all fields not 
	/// specifically handled by your implementation.
	/// </returns>
	/// <param name='field'>
	/// The field to check.
	/// </param>
	UTVisibilityDecision IsVisible (FieldInfo field);
	
	/// <summary>
	/// Draws an array member using the given wrapper. Usually this only needs to be implemented if the delegate
	/// should control how array members are drawn. If you simply want to do a renderer for a certain
	/// type, do it in <c>DrawProperty</c> instead as <c>DrawProperty</c> will be called by the default
	/// implementation for rendering parts of an array.
	/// </summary>
	/// <returns>
	/// True if the delegate has rendered this field, false if the default implementation should render it.
	/// </returns>
	/// <param name='wrapper'>
	/// The wrapper around this field. 
	/// </param>
	/// <param name='deleteMember'>
	/// If set to true the array member will be deleted after the rendering method finishes. Use this
	/// for driving a delete button in the rendering output.
	/// </param>
	bool DrawArrayMember (UTFieldWrapper wrapper, out bool deleteMember);
	
	/// <summary>
	/// Draws the property specified by the given wrapper. There is always a BeginHorizontal/EndHorizontal wrapped
	/// around this by the default implementation. If the given property is a part of an array
	/// </summary>
	/// <returns>
	/// True if the delegate has rendered this field, false if the default implementation should render it.
	/// </returns>
	/// <param name='wrapper'>
	/// The wrapper describing the field.
	/// </param>
	bool DrawProperty (UTFieldWrapper wrapper);
	
}

