using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Rect")]
public class ToStringByFormat : UBAction {

	[UBRequired] public UBRect _Rect = new UBRect();
	[UBRequired] public UBString _Format = new UBString();
	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rect.GetValue(context).ToString(_Format.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s ToString w/ {1}", _Rect.ToString(RootContainer), _Format.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rect#.ToString(#_Format#)");
	}

}