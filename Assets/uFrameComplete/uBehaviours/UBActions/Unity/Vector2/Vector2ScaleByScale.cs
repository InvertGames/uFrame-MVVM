using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Vector2")]
public class Vector2ScaleByScale : UBAction
{

	[UBRequired] public UBVector2 _Vector2 = new UBVector2();
	[UBRequired] public UBVector2 _Scale = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		_Vector2.GetValue(context).Scale(_Scale.GetValue(context));
	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Vector2#.Scale(#_Scale#)");
	}

}