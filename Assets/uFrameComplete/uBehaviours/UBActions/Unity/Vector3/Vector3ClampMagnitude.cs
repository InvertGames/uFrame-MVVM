using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Vector3")]
public class Vector3ClampMagnitude : UBAction
{

	[UBRequired] public UBVector3 _Vector = new UBVector3();
	[UBRequired] public UBFloat _MaxLength = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.ClampMagnitude(_Vector.GetValue(context),_MaxLength.GetValue(context)));
		}

	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.ClampMagnitude(#_Vector#, #_MaxLength#)");
	}

}