using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;


[UBCategory("Vector2")]
public class Vector2SqrMagnitudeByA : UBAction
{
	[UBRequired] public UBVector2 _A = new UBVector2();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector2.SqrMagnitude(_A.GetValue(context)));
		}
	}

	public override string ToString() {
        return string.Format("SqrMagnitude of {0}", _A.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector2.SqrMagnitude(#_A#)");
	}

}