using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Controls whether gravity affects this rigidbody.")]
[UBCategory("Rigidbody")]
public class SetRigidbodyUseGravity : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	[UBRequired] public UBBool _Value = new UBBool();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Rigidbody.GetValueAs<Rigidbody>(context).useGravity = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s useGravity to {1}", _Rigidbody.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.useGravity = #_Value# ");
	}

}