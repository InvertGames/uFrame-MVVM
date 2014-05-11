using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Same as  but makes sure the values interpolate correctly when they wrap around 360 degrees.")]
[UBCategory("Mathf")]
public class LerpAngle : UBAction {

	
	[UBRequired] public UBFloat _A = new UBFloat();
	
	[UBRequired] public UBFloat _B = new UBFloat();
	
	[UBRequired] public UBFloat _T = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.LerpAngle(_A.GetValue(context),_B.GetValue(context),_T.GetValue(context)));
		}

	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.LerpAngle(#_A#, #_B#, #_T#)");
	}

}