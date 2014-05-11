using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Cross Product of two vectors.")]
[UBCategory("Vector3")]
public class Vector3Cross : UBAction
{

	
	[UBRequired] public UBVector3 _Lhs = new UBVector3();
	
	[UBRequired] public UBVector3 _Rhs = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.Cross(_Lhs.GetValue(context),_Rhs.GetValue(context)));
		}

	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.Cross(#_Lhs#, #_Rhs#)");
	}

}