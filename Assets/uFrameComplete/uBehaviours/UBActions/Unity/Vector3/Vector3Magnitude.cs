using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;


[UBCategory("Vector3")]
public class Vector3Magnitude : UBAction
{

	
	[UBRequired] public UBVector3 _A = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.Magnitude(_A.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Get Magnitude of {0}", _A.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.Magnitude(#_A#)");
	}

}