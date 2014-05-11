using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the length of this vector (Read Only).")]
[UBCategory("Vector3")]
public class GetVector3Magnitude : UBAction {

	[UBRequired] public UBVector3 _Vector3 = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Vector3.GetValue(context).magnitude);
		}

	}

	public override string ToString(){
	return string.Format("Get magnitude from {0}", _Vector3.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Vector3#.magnitude");
	}

}