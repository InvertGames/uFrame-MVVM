using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Shorthand for writing Vector2(1, 0)")]
[UBCategory("Vector2")]
public class GetVector2Right : UBAction {

	[UBRequireVariable] [UBRequired] public UBVector2 _Result = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector2.right);
		}

	}

	public override string ToString(){
	return string.Format("Get right from {0}", "Vector2");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Vector2.right");
	}

}