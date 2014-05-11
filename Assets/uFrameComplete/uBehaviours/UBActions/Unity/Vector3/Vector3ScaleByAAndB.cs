using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Multiplies every component of this vector by the same component of scale.")]
[UBCategory("Vector3")]
public class Vector3ScaleByAAndB : UBAction
{

	
	[UBRequired] public UBVector3 _A = new UBVector3();
	
	[UBRequired] public UBVector3 _B = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.Scale(_A.GetValue(context),_B.GetValue(context)));
		}

	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.Scale(#_A#, #_B#)");
	}

}