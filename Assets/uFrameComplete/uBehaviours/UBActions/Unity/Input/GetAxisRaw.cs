using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the value of the virtual axis identified by axisName with no smoothing filtering applied.")]
[UBCategory("Input")]
public class GetAxisRaw : UBAction {

	
	[UBRequired] public UBString _AxisName = new UBString();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Input.GetAxisRaw(_AxisName.GetValue(context)));
		}

	}

	public override string ToString(){
        return string.Format("Get {0}'s Raw Axis", _AxisName.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Input.GetAxisRaw(#_AxisName#)");
	}

}