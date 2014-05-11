using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns a random float number between and min [inclusive] and max [inclusive] (Read Only).")]
[UBCategory("Random")]
public class RangeByMinAndMax : UBAction {

	
	[UBRequired] public UBInt _Min = new UBInt();
	
	[UBRequired] public UBInt _Max = new UBInt();
	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
            _Result.SetValue(context, UnityEngine.Random.Range(_Min.GetValue(context), _Max.GetValue(context)));
		}

	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Random.Range(#_Min#, #_Max#)");
	}

}