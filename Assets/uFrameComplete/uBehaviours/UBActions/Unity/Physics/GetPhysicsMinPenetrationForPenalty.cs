using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The minimum contact penetration value in order to apply a penalty force (default 0.05). Must be positive.")]
[UBCategory("Physics")]
public class GetPhysicsMinPenetrationForPenalty : UBAction {

	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Physics.minPenetrationForPenalty);
		}

	}

	public override string ToString(){
	return string.Format("Get minPenetrationForPenalty from {0}", "Physics");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Physics.minPenetrationForPenalty");
	}

}