using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns a vector that is made from the smallest components of lhs and rhs.")]
[UBCategory("Vector3")]
public class Vector3Min : UBAction
{

	
	[UBRequired] public UBVector3 _Lhs = new UBVector3();
	
	[UBRequired] public UBVector3 _Rhs = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.Min(_Lhs.GetValue(context),_Rhs.GetValue(context)));
		}

	}

	public override string ToString(){
        return string.Format("Get Min between {0} & {1}", _Lhs.ToString(RootContainer), _Rhs.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.Min(#_Lhs#, #_Rhs#)");
	}

}