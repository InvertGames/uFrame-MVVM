using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Dot Product of two vectors.")]
[UBCategory("Vector3")]
public class Vector3Dot : UBAction
{

	
	[UBRequired] public UBVector3 _Lhs = new UBVector3();
	
	[UBRequired] public UBVector3 _Rhs = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.Dot(_Lhs.GetValue(context),_Rhs.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Dot Product of {0} and {1}", _Lhs.ToString(RootContainer), _Rhs.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.Dot(#_Lhs#, #_Rhs#)");
	}

}