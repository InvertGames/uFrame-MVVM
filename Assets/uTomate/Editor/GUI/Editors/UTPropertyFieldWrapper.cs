//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using System.Reflection;

/// <summary>
/// FieldWrapper which works on UTProperties.
/// </summary>
public class UTPropertyFieldWrapper : UTFieldWrapper
{
	private UTPropertyBase theProperty;
	private GUIContent label;
	private FieldInfo fieldInfo;
	
	public void SetUp(GUIContent label, FieldInfo fieldInfo, UTPropertyBase theProperty) {
		this.label = label;
		this.theProperty = theProperty;
		this.fieldInfo = fieldInfo;
	}
	
	public string Expression {
		get {
			return theProperty.Expression;
		}
		set {
			theProperty.Expression = value;
		}
	}
	
	public bool IsAsset {
		get {
			return theProperty.WrappedType.IsSubclassOf(typeof(UnityEngine.Object));
		}
	}
	
	public Type AssetType {
		get {
			return theProperty.WrappedType;
		}
	}
	
	public bool IsBool {
		get {
			return theProperty is UTBool;
		}
	}
	
	
	public bool IsString {
		get {
			return theProperty is UTString;
		}
	}

	public bool IsFloat {
		get {
			return theProperty is UTFloat;
		}
	}
	
	public bool IsInt {
		get {
			return theProperty is UTInt;
		}
	}

	public bool IsEnum {
		get {
			// crappy hack around C#s generics system.
			// in java this would be instanceof UTEnum, but doesn't work with UTEnum<T> since
			// C# has no type wildcards. Won't work for deeper hierarchies but for now
			// it should be sufficient.
			return theProperty.GetType().BaseType.Name.StartsWith("UTEnum");  
		}
	}
	
	public bool IsColor {
		get {
			return theProperty is UTColor;
		}
	}
	
	public UnityEngine.GUIContent Label {
		get {
			return  label;
		}
	}
	
	public bool SupportsExpressions {
		get {
			return true; // all UTProperty stuff supports expressions.
		}
	}
	
	public bool UseExpression {
		get {
			return theProperty.UseExpression;
		}
		set {
			theProperty.UseExpression = value;
		}
	}
	
	public object Value {
		get {
			return theProperty.ObjectValue;
		}
		set {
			theProperty.ObjectValue = value;
		}
	}
	
	public UTInspectorHint InspectorHint {
		get {
			return UTInspectorHint.GetFor(fieldInfo);
		}
	}
	
	
	public Type FieldType {
		get {
			return fieldInfo.FieldType;
		}
	}
	
	public string FieldName {
		get {
			return fieldInfo.Name;
		}
	}
}

