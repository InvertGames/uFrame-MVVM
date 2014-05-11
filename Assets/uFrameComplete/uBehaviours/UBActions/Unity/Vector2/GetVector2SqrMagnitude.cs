using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the squared length of this vector (Read Only).")]
[UBCategory("Vector2")]
public class GetVector2SqrMagnitude : UBAction {

	[UBRequired] public UBVector2 _Vector2 = new UBVector2();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Vector2.GetValue(context).sqrMagnitude);
		}

	}

	public override string ToString(){
	return string.Format("Get sqrMagnitude from {0}", _Vector2.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Vector2#.sqrMagnitude");
	}

}