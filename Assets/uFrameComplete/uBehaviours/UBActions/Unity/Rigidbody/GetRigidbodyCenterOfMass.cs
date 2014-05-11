using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The center of mass relative to the transform's origin.")]
[UBCategory("Rigidbody")]
public class GetRigidbodyCenterOfMass : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rigidbody.GetValueAs<Rigidbody>(context).centerOfMass);
		}

	}

	public override string ToString(){
	return string.Format("Get centerOfMass from {0}", _Rigidbody.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Rigidbody#.centerOfMass");
	}

}