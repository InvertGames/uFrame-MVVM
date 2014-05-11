using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Two colliding objects with a relative velocity below this will not bounce (default 2). Must be positive.")]
[UBCategory("Physics")]
public class SetPhysicsBounceThreshold : UBAction {

	[UBRequired] public UBFloat _Value = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Physics.bounceThreshold = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s bounceThreshold to {1}", "Physics", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics.bounceThreshold = #_Value# ");
	}

}