using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;


[UBCategory("Vector3")]
public class Vector3Exclude : UBAction
{

	
	[UBRequired] public UBVector3 _ExcludeThis = new UBVector3();
	
	[UBRequired] public UBVector3 _FromThat = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector3.Exclude(_ExcludeThis.GetValue(context),_FromThat.GetValue(context)));
		}
	}

	public override string ToString(){
	return string.Format("Call {0}'s Exclude w/ {1} and {2}", "Vector3", _ExcludeThis.ToString(RootContainer), _FromThat.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector3.Exclude(#_ExcludeThis#, #_FromThat#)");
	}

}