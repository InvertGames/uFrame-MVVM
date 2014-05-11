using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Allows you to override the solver iteration count per rigidbody.")]
[UBCategory("Rigidbody")]
public class SetRigidbodySolverIterationCount : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	[UBRequired] public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Rigidbody.GetValueAs<Rigidbody>(context).solverIterationCount = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s solverIterationCount to {1}", _Rigidbody.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.solverIterationCount = #_Value# ");
	}

}