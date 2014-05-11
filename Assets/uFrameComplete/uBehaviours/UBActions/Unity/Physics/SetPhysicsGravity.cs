using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The gravity applied to all rigid bodies in the scene.")]
[UBCategory("Physics")]
public class SetPhysicsGravity : UBAction {

	[UBRequired] public UBVector3 _Value = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Physics.gravity = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s gravity to {1}", "Physics", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics.gravity = #_Value# ");
	}

}