using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns this vector with a  of 1 (Read Only).")]
[UBCategory("Vector3")]
public class GetVector3Normalized : UBAction {

	[UBRequired] public UBVector3 _Vector3 = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Vector3.GetValue(context).normalized);
		}

	}

	public override string ToString(){
	return string.Format("Get normalized from {0}", _Vector3.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Vector3#.normalized");
	}

}