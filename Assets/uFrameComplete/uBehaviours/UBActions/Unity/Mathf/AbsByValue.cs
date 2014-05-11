using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the absolute value of f.")]
[UBCategory("Mathf")]
public class AbsByValue : UBAction {

	
	[UBRequired] public UBInt _Value = new UBInt();
	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Abs(_Value.GetValue(context)));
		}

	}

	public override string ToString(){
	    return string.Format("Absolute value of {0}", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Abs(#_Value#)");
	}

}