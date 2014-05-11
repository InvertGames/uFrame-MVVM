using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The default solver iteration count permitted for any rigid bodies (default 7). Must be positive.")]
[UBCategory("Physics")]
public class SetPhysicsSolverIterationCount : UBAction {

	[UBRequired] public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Physics.solverIterationCount = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s solverIterationCount to {1}", "Physics", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics.solverIterationCount = #_Value# ");
	}

}