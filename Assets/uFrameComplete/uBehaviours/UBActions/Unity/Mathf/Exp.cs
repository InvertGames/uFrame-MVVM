using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns e raised to the specified power.")]
[UBCategory("Mathf")]
public class Exp : UBAction {

	
	[UBRequired] public UBFloat _Power = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Exp(_Power.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("E Raised to the power of {0}", _Power.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Exp(#_Power#)");
	}

}