using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Should collision detection be enabled? (By default always enabled)")]
[UBCategory("Rigidbody")]
public class GetRigidbodyDetectCollisions : UBConditionAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	public override bool PerformCondition(IUBContext context){
return _Rigidbody.GetValueAs<Rigidbody>(context).detectCollisions		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_Rigidbody#.detectCollisions");
	}

}