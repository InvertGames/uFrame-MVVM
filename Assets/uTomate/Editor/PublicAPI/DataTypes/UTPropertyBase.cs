//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;

/// <summary>
/// Base class for action properties that support expressions.
/// </summary>
[Serializable]
public abstract class UTPropertyBase 
{
	
	[SerializeField]
	private bool useExpression = false;

	/// <summary>
	/// Property indicating whether or not  this property's value is determined by evaluating an expression.
	/// </summary>
	public virtual bool UseExpression {
		get {
			return useExpression;
		}
		set {
			useExpression = value;
		}
	}
	
	/// <summary>
	/// The value of this property as an untyped object.
	/// </summary>
	public abstract object ObjectValue {
		get; set;
	}
	
	public abstract Type WrappedType {
		get;
	}
	
	[SerializeField]
	private string expression = "";
	
	/// <summary>
	/// The expression that should be used to evaluate the value of this property. Only relevant if 
	/// <code>UseExpression</code> is set to true, otherwise the expression is ignored.
	/// </summary>
	public string Expression { 
		get {
			return expression;
		}
		set {
			expression = value;
		}
	}
}

