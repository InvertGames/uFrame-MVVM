using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The minimum contact penetration value in order to apply a penalty force (default 0.05). Must be positive.")]
[UBCategory("Physics")]
public class SetPhysicsMinPenetrationForPenalty : UBAction {

	[UBRequired] public UBFloat _Value = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Physics.minPenetrationForPenalty = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s minPenetrationForPenalty to {1}", "Physics", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics.minPenetrationForPenalty = #_Value# ");
	}

}