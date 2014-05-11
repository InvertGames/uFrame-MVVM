using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Controls whether physics affects the rigidbody.")]
[UBCategory("Rigidbody")]
public class GetRigidbodyIsKinematic : UBConditionAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	public override bool PerformCondition(IUBContext context){
return _Rigidbody.GetValueAs<Rigidbody>(context).isKinematic		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_Rigidbody#.isKinematic");
	}

}