using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Allows you to override the solver iteration count per rigidbody.")]
[UBCategory("Rigidbody")]
public class GetRigidbodySolverIterationCount : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rigidbody.GetValueAs<Rigidbody>(context).solverIterationCount);
		}

	}

	public override string ToString(){
	return string.Format("Get solverIterationCount from {0}", _Rigidbody.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Rigidbody#.solverIterationCount");
	}

}