using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Calculates the shortest difference between two given angles.")]
[UBCategory("Mathf")]
public class DeltaAngle : UBAction {

	
	[UBRequired] public UBFloat _Current = new UBFloat();
	
	[UBRequired] public UBFloat _Target = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.DeltaAngle(_Current.GetValue(context),_Target.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("DeltaAngle between {0} & {1}", _Current.ToString(RootContainer), _Target.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.DeltaAngle(#_Current#, #_Target#)");
	}

}