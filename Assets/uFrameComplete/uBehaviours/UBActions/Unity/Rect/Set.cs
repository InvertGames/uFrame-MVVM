using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Rect")]
public class Set : UBAction {

	[UBRequired] public UBRect _Rect = new UBRect();
	[UBRequired] public UBFloat _Left = new UBFloat();
	[UBRequired] public UBFloat _Top = new UBFloat();
	[UBRequired] public UBFloat _Width = new UBFloat();
	[UBRequired] public UBFloat _Height = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		_Rect.GetValue(context).Set(_Left.GetValue(context),_Top.GetValue(context),_Width.GetValue(context),_Height.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s Set w/ {1}, {2}, {3} and {4}", _Rect.ToString(RootContainer), _Left.ToString(RootContainer), _Top.ToString(RootContainer), _Width.ToString(RootContainer), _Height.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rect#.Set(#_Left#, #_Top#, #_Width#, #_Height#)");
	}

}