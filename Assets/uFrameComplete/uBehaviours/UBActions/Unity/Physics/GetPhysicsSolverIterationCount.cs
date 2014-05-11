using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The default solver iteration count permitted for any rigid bodies (default 7). Must be positive.")]
[UBCategory("Physics")]
public class GetPhysicsSolverIterationCount : UBAction {

	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Physics.solverIterationCount);
		}

	}

	public override string ToString(){
	return string.Format("Get solverIterationCount from {0}", "Physics");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Physics.solverIterationCount");
	}

}