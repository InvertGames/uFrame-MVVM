using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Multiplies every component of this vector by the same component of scale.")]
[UBCategory("Vector3")]
public class Vector3ScaleByScale : UBAction
{

	[UBRequired] public UBVector3 _Vector3 = new UBVector3();
	
	[UBRequired] public UBVector3 _Scale = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		_Vector3.GetValue(context).Scale(_Scale.GetValue(context));
	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Vector3#.Scale(#_Scale#)");
	}

}