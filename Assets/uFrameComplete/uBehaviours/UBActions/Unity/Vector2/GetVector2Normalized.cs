using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Vector2")]
public class GetVector2Normalized : UBAction {

	[UBRequired] public UBVector2 _Vector2 = new UBVector2();
	[UBRequireVariable] [UBRequired] public UBVector2 _Result = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Vector2.GetValue(context).normalized);
		}

	}

	public override string ToString(){
	return string.Format("Get normalized from {0}", _Vector2.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Vector2#.normalized");
	}

}