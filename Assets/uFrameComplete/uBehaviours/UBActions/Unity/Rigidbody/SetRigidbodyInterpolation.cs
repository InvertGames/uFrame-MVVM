using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Interpolation allows you to smooth out the effect of running physics at a fixed frame rate.")]
[UBCategory("Rigidbody")]
public class SetRigidbodyInterpolation : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	[UBRequired] public UBEnum _Value = new UBEnum(typeof(RigidbodyInterpolation));
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Rigidbody.GetValueAs<Rigidbody>(context).interpolation = ((RigidbodyInterpolation)_Value.GetIntValue(context));
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s interpolation to {1}", _Rigidbody.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.interpolation = #_Value# ");
	}

}