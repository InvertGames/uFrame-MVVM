using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Left coordinate of the rectangle.")]
[UBCategory("Rect")]
public class GetRectXMin : UBAction {

	[UBRequired] public UBRect _Rect = new UBRect();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rect.GetValue(context).xMin);
		}

	}

	public override string ToString(){
	return string.Format("Get xMin from {0}", _Rect.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Rect#.xMin");
	}

}