using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Creates a rectangle from min/max coordinate values.")]
[UBCategory("Rect")]
public class MinMaxRect : UBAction {

	
	[UBRequired] public UBFloat _Left = new UBFloat();
	
	[UBRequired] public UBFloat _Top = new UBFloat();
	
	[UBRequired] public UBFloat _Right = new UBFloat();
	
	[UBRequired] public UBFloat _Bottom = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBRect _Result = new UBRect();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Rect.MinMaxRect(_Left.GetValue(context),_Top.GetValue(context),_Right.GetValue(context),_Bottom.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s MinMaxRect w/ {1}, {2}, {3} and {4}", "Rect", _Left.ToString(RootContainer), _Top.ToString(RootContainer), _Right.ToString(RootContainer), _Bottom.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Rect.MinMaxRect(#_Left#, #_Top#, #_Right#, #_Bottom#)");
	}

}