using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Physics2D")]
public class SetPhysics2DPositionIterations : UBAction {

	[UBRequired] public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Physics2D.positionIterations = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s positionIterations to {1}", "Physics2D", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics2D.positionIterations = #_Value# ");
	}

}