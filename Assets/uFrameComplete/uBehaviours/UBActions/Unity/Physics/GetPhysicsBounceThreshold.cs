using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Two colliding objects with a relative velocity below this will not bounce (default 2). Must be positive.")]
[UBCategory("Physics")]
public class GetPhysicsBounceThreshold : UBAction {

	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Physics.bounceThreshold);
		}

	}

	public override string ToString(){
	return string.Format("Get bounceThreshold from {0}", "Physics");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Physics.bounceThreshold");
	}

}