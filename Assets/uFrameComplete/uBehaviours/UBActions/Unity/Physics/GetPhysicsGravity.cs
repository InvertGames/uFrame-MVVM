using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The gravity applied to all rigid bodies in the scene.")]
[UBCategory("Physics")]
public class GetPhysicsGravity : UBAction {

	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Physics.gravity);
		}

	}

	public override string ToString(){
	return string.Format("Get gravity from {0}", "Physics");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Physics.gravity");
	}

}