using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns a vector that is made from the largest components of lhs and rhs.")]
[UBCategory("Vector3")]
public class Vector3Max : UBAction
{

	
	[UBRequired] public UBVector3 _Lhs = new UBVector3();
	
	[UBRequired] public UBVector3 _Rhs = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.Max(_Lhs.GetValue(context),_Rhs.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Get Max between {0} & {1}", _Lhs.ToString(RootContainer), _Rhs.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.Max(#_Lhs#, #_Rhs#)");
	}

}