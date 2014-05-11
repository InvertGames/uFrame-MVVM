using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Rect")]
public class GetRectCenter : UBAction {

	[UBRequired] public UBRect _Rect = new UBRect();
	[UBRequireVariable] [UBRequired] public UBVector2 _Result = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rect.GetValue(context).center);
		}

	}

	public override string ToString(){
	return string.Format("Get center from {0}", _Rect.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Rect#.center");
	}

}