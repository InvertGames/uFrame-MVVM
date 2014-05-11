using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns larger of the two values.")]
[UBCategory("Mathf")]
public class MaxByAAndB : UBAction {

	
	[UBRequired] public UBInt _A = new UBInt();
	
	[UBRequired] public UBInt _B = new UBInt();
	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Max(_A.GetValue(context),_B.GetValue(context)));
		}

	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Max(#_A#, #_B#)");
	}

}