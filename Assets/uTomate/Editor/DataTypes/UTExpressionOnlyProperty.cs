// (C) 2013 Ancient Light Studios. All rights reserved.
using System;

/// <summary>
/// Base class for property types that only support expressions.
/// </summary>
[Serializable]
public abstract class UTExpressionOnlyProperty<T> : UTProperty<T>
{
	public UTExpressionOnlyProperty() {
		base.UseExpression = true;
	}
	
	public override T Value {
		get {
			throw new NotImplementedException (GetType().FullName + " does not support values.");
		}
		set {
			throw new NotImplementedException (GetType().FullName + " does not support values.");
		}
	}
	
	public override bool UseExpression {
		get {
			return true;
		}
		set {
		}
	}
}

