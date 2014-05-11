using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The time the latest  has started (Read Only). This is the time in seconds since the start of the game.")]
[UBCategory("Time")]
public class GetTimeFixedTime : UBAction {

	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Time.fixedTime);
		}

	}

	public override string ToString(){
	return string.Format("Get fixedTime from {0}", "Time");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Time.fixedTime");
	}

}