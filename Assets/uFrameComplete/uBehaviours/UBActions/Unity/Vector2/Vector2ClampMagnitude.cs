using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Vector2")]
public class Vector2ClampMagnitude : UBAction
{

	[UBRequired] public UBVector2 _Vector = new UBVector2();
	[UBRequired] public UBFloat _MaxLength = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBVector2 _Result = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector2.ClampMagnitude(_Vector.GetValue(context),_MaxLength.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s ClampMagnitude w/ {1} and {2}", "Vector2", _Vector.ToString(RootContainer), _MaxLength.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector2.ClampMagnitude(#_Vector#, #_MaxLength#)");
	}

}