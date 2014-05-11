using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Physics2D")]
public class SetPhysics2DRaycastsHitTriggers : UBAction {

	[UBRequired] public UBBool _Value = new UBBool();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Physics2D.raycastsHitTriggers = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s raycastsHitTriggers to {1}", "Physics2D", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics2D.raycastsHitTriggers = #_Value# ");
	}

}