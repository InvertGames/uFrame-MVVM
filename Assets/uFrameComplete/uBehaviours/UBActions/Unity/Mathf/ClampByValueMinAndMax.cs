using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Clamps a value between a minimum float and maximum float value.")]
[UBCategory("Mathf")]
public class ClampByValueMinAndMax : UBAction {

	
	[UBRequired] public UBInt _Value = new UBInt();
	
	[UBRequired] public UBInt _Min = new UBInt();
	
	[UBRequired] public UBInt _Max = new UBInt();
	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Clamp(_Value.GetValue(context),_Min.GetValue(context),_Max.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Clamp {0} between {1} & {2}", _Value.ToString(RootContainer), _Min.ToString(RootContainer), _Max.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Clamp(#_Value#, #_Min#, #_Max#)");
	}

}