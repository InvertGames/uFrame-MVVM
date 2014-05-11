using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Makes this vector have a  of 1.")]
[UBCategory("Vector3")]
public class Vector3Normalize : UBAction
{

	[UBRequired] public UBVector3 _Vector3 = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		_Vector3.GetValue(context).Normalize();
	}

	public override string ToString(){
	return string.Format("Normalize {0}", _Vector3.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Vector3#.Normalize()");
	}

}