using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Clamps value between 0 and 1 and returns value")]
[UBCategory("Mathf")]
public class Clamp01 : UBAction {

	
	[UBRequired] public UBFloat _Value = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Clamp01(_Value.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Clamp01 {0}", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Clamp01(#_Value#)");
	}

}