using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Vector2")]
public class Vector2Normalize : UBAction
{

	[UBRequired] public UBVector2 _Vector2 = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		_Vector2.GetValue(context).Normalize();
	}

	public override string ToString(){
        return string.Format("Normalize {0}", _Vector2.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Vector2#.Normalize()");
	}

}