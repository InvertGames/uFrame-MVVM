using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Shorthand for writing Vector3(1, 1, 1)")]
[UBCategory("Vector3")]
public class GetVector3One : UBAction {

	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.one);
		}

	}

	public override string ToString(){
	return string.Format("Get one from {0}", "Vector3");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Vector3.one");
	}

}