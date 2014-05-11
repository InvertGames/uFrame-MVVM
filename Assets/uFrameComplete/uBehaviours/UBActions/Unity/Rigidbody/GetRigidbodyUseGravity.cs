using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Controls whether gravity affects this rigidbody.")]
[UBCategory("Rigidbody")]
public class GetRigidbodyUseGravity : UBConditionAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	public override bool PerformCondition(IUBContext context){
return _Rigidbody.GetValueAs<Rigidbody>(context).useGravity		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_Rigidbody#.useGravity");
	}

}