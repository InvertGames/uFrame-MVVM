//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;

/// <summary>
/// Wrapper around a field. Used for simplifying the drawing algorithms in UTActionEditorBase.
/// </summary> 
public interface UTFieldWrapper
{
	
	GUIContent Label {get; }
	
	/// <summary>
	/// Is the wrapped field of boolean type?
	/// </summary>
	/// 
	bool IsBool {get;}
	
	/// <summary>
	/// Is the wrapped field of string type?
	/// </summary>
	bool IsString {get; }
	
	/// <summary>
	/// Is the wrapped field of float type?
	/// </summary>
	bool IsFloat {get; }
	
	/// <summary>
	/// Is the wrapped field of int type?
	/// </summary>
	bool IsInt {get; }
	
	/// <summary>
	/// Is the wrapped field an asset type?
	/// </summary>a
	bool IsAsset{get; }
	
	/// <summary>
	/// Gets the type of the asset. Only available if IsAsset is true.
	/// </summary>
	Type AssetType {get; }
	
	/// <summary>
	/// Is the wrapepd field of an enum type?
	/// </summary>
	bool IsEnum{get;}
	
	/// <summary>
	/// Is the wrapepd field of color type?
	/// </summary>
	bool IsColor{get; }
	
	/// <summary>
	/// Supports the wrapped field expressions?
	/// </summary>
	bool SupportsExpressions{get; }
	
	/// <summary>
	/// The value of the wrapped field
	/// </summary>
	object Value{ get; set; }
	
	/// <summary>
	/// The expression of the wrapped field. Only get/set if SupportsExpressions is true.
	/// </summary>
	string Expression{ get; set; }
	
	/// <summary>
	/// Whether the expression or the plain value should be used. Only get/set if SupportsExpressions is true.
	/// </summary>
	bool UseExpression{ get; set; }
	
	/// <summary>
	/// Gets the type of the field wrapped by this wrapper
	/// </summary>
	/// <value>
	/// The type of the field.
	/// </value>
	Type FieldType { get;}
	
	/// <summary>
	/// Gets the name of the field.
	/// </summary>
	/// <value>
	/// The name of the field.
	/// </value>
	string FieldName {get;}
	
	/// <summary>
	/// Gets the inspector hint.
	/// </summary>
	UTInspectorHint InspectorHint {
		get;
	}
}


