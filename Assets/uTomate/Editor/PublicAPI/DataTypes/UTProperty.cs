//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;

/// <summary>
/// Subclass of UTProperty base which allows generic typing of properties and enables type casting. This is the class
/// you want to derive from when creating new property types.
/// </summary>
[Serializable]
public abstract class UTProperty<T> : UTPropertyBase
{
	
	/// <summary>
	/// Same as <code>ObjectValue</code> but returns a typed object instead of a generic object.
	/// </summary>
	public abstract T Value {
		get; set;
	}
	
	
	public UTProperty() {
		if (!GetType().IsSerializable) {
			Debug.LogWarning("You need to attach a [Serializable] attribute to class " + GetType().FullName + ". If you wrote this data type please fix it, otherwise please report this issue.");
		}
	}
	
	public override object ObjectValue {
		get {
			return Value;
		}
		set {
			Value = (T) value;
		}
	}
	
	public override Type WrappedType {
		get {
			return typeof(T);
		}
	}
	
	
	/// <summary>
	/// Property indicating whether or not the <code>CustomCast</code> method should be called regardless of the type
	/// that the expression yields. When this is set to true, then the result of an evaluated expression will be
	/// sent to the <code>CustomCast</code> function. If set to false, then the <code>CustomCast</code> function will
	/// only be called if the object that was returned by the expression is not of the type that this property expects.
	/// </summary>
	public virtual bool AlwaysCast {
		get {
			return false;
		}
	}
	
	/// <summary>
	/// Called by EvaluateIn and allows to customize the casting of expressions that yield a different
	/// type than the desired target type of this property. Default behaviour is to fail the build if this
	/// happens. You can override this in subclasses to convert an object into the target type instead of
	/// failing the build.
	/// </summary>
	protected virtual T CustomCast(object val) {
		if (UseExpression) {
			throw new UTFailBuildException("Expression '" + Expression + "' does not evaluate to type '" + typeof(T).Name + "'", null);
		} else {
			throw new UTFailBuildException("Value '" + val + "' does not evaluate to type '" + typeof(T).Name + "'", null);
		}
	}
	
	/// <summary>
	/// Logging method which allows logging a conversion of an object. You should call this function whenever you convert
	/// an object in the <code>CustomCast</code> function. This method will log diagnostic output about the conversion
	/// to Unity's console when uTomate is in debug mode. This helps an end-user to see when and how objects get
	/// converted and will greatly aid debugging.
	/// </summary>
	/// <param name='src'>
	/// The object before conversion.
	/// </param>
	/// <param name='trg'>
	/// The object after conversion.
	/// </param>
	protected void LogConversion(object src, object trg) {
		if (UTPreferences.DebugMode) {
			Debug.Log("Implicit conversion: " + src + (src != null ? " [" + src.GetType().Name+"]" : "") + " to " + 
				trg + (trg != null ? " [" + trg.GetType().Name + "]" : "")); 
		}
	}
	
	/// <summary>
	/// Evaluates this property in the given context.
	/// </summary>
	/// <returns>
	/// The value of the evaluated property.
	/// </returns>
	/// <param name='context'>
	/// The context to evaluate the property in.
	/// </param>
	public T EvaluateIn(UTContext context) {
		if (UseExpression) {
			var result = context.Evaluate(Expression);
			if (!(result is T) ||  AlwaysCast) {
				result = CustomCast(result);
			}
			if (result == null) {
				throw new UTFailBuildException("Internal Error: An evaluated expression yielded null.", null);
			}
			return (T)result;
		}
		else {
			if (AlwaysCast) {
				return CustomCast(Value);
			}
			return Value;
		}
	}
}

