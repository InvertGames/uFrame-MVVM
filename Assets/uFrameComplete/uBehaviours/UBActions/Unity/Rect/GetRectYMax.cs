using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Bottom coordinate of the rectangle.")]
[UBCategory("Rect")]
public class GetRectYMax : UBAction {

	[UBRequired] public UBRect _Rect = new UBRect();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rect.GetValue(context).yMax);
		}

	}

	public override string ToString(){
	return string.Format("Get yMax from {0}", _Rect.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Rect#.yMax");
	}

}