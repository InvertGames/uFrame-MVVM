using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Controls whether physics will change the rotation of the object.")]
[UBCategory("Rigidbody")]
public class GetRigidbodyFreezeRotation : UBConditionAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	public override bool PerformCondition(IUBContext context){
return _Rigidbody.GetValueAs<Rigidbody>(context).freezeRotation		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("#_Rigidbody#.freezeRotation");
	}

}