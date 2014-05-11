using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Height of the rectangle.")]
[UBCategory("Rect")]
public class GetRectHeight : UBAction {

	[UBRequired] public UBRect _Rect = new UBRect();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rect.GetValue(context).height);
		}

	}

	public override string ToString(){
	return string.Format("Get height from {0}", _Rect.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Rect#.height");
	}

}