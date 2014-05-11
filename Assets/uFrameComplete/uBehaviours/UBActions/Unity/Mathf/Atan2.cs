using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns the angle in radians whose  is y/x.")]
[UBCategory("Mathf")]
public class Atan2 : UBAction {

	
	[UBRequired] public UBFloat _Y = new UBFloat();
	
	[UBRequired] public UBFloat _X = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Mathf.Atan2(_Y.GetValue(context),_X.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Angle in Radians {0}/{1}", _Y.ToString(RootContainer), _X.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Mathf.Atan2(#_Y#, #_X#)");
	}

}