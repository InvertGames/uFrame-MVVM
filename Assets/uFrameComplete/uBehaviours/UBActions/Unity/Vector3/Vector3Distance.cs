using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the distance between a and b.")]
[UBCategory("Vector3")]
public class Vector3Distance : UBAction
{

	
	[UBRequired] public UBVector3 _A = new UBVector3();
	
	[UBRequired] public UBVector3 _B = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.Distance(_A.GetValue(context),_B.GetValue(context)));
		}

	}

	public override string ToString(){
	    return string.Format("Distance between {0} and {1}", _A.ToString(RootContainer), _B.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.Distance(#_A#, #_B#)");
	}

}