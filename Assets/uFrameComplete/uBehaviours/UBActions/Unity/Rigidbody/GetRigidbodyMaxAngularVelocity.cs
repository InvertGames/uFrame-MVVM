using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The maximimum angular velocity of the rigidbody. (Default 7) range { 0, infinity }")]
[UBCategory("Rigidbody")]
public class GetRigidbodyMaxAngularVelocity : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rigidbody.GetValueAs<Rigidbody>(context).maxAngularVelocity);
		}

	}

	public override string ToString(){
	return string.Format("Get maxAngularVelocity from {0}", _Rigidbody.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Rigidbody#.maxAngularVelocity");
	}

}