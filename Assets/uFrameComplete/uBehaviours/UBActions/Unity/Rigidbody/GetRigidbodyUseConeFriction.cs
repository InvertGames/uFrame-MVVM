using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Force cone friction to be used for this rigidbody.")]
[UBCategory("Rigidbody")]
public class GetRigidbodyUseConeFriction : UBConditionAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	public override bool PerformCondition(IUBContext context){
return _Rigidbody.GetValueAs<Rigidbody>(context).useConeFriction		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_Rigidbody#.useConeFriction");
	}

}