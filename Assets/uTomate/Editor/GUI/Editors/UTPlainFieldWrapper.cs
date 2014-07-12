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
/// FieldWrapper which works on plain fields.
/// </summary>
public class UTPlainFieldWrapper : UTFieldWrapper
{
	private GUIContent label;
	private FieldInfo fieldInfo;
	private object holder;
	
	
	public void SetUp(GUIContent label, FieldInfo fieldInfo, object holder) {
		this.label = label;
		this.fieldInfo = fieldInfo;
		this.holder = holder;
	}
	
	public string Expression {
		get {
			throw new NotImplementedException("Plain fields do not support expressions.");
		}
		set {
			throw new NotImplementedException("Plain fields do not support expressions.");
		}
	}
	
	public bool IsAsset {
		get {
			return fieldInfo.FieldType.IsSubclassOf(typeof(UnityEngine.Object));
		}
	}
	
	public Type AssetType {
		get {
			if (!IsAsset) {
				throw new NotImplementedException("Field is not an asset.");
			}
			return fieldInfo.FieldType;
		}
	}
	
	public bool IsBool {
		get {
			return fieldInfo.FieldType == typeof(bool);
		}
	}
	
	public bool IsString {
		get {
			return fieldInfo.FieldType == typeof(string);
		}
	}
	
	public bool IsFloat {
		get {
			return fieldInfo.FieldType == typeof(float);
		}
	}
	
	public bool IsInt {
		get {
			return fieldInfo.FieldType == typeof(int);
		}
	}
	
	public bool IsEnum {
		get {
			return fieldInfo.FieldType.IsEnum;
		}
	}
	
	public bool IsColor {
		get {
			return fieldInfo.FieldType == typeof(Color);
		}
	}
	
	
	public bool SupportsExpressions {
		get {
			return false; // plain properties do not support expressions.
		}
	}
	
	public bool UseExpression {
		get {
			throw new NotImplementedException("Plain fields do not support expressions.");
		}
		set {
			throw new NotImplementedException("Plain fields do not support expressions.");
		}
	}
	
	public object Value {
		get {
			return fieldInfo.GetValue(holder);
		}
		set {
			fieldInfo.SetValue(holder, value);
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
	
	public GUIContent Label {
		get {
			return label;
		}
	}
	
	public UTInspectorHint InspectorHint {
		get {
			return UTInspectorHint.GetFor(fieldInfo);
		}
	}
	
}

