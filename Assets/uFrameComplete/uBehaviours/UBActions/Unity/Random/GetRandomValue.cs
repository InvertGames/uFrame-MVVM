using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns a random number between 0.0 [inclusive] and 1.0 [inclusive] (Read Only).")]
[UBCategory("Random")]
public class GetRandomValue : UBAction {

	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
            _Result.SetValue(context, UnityEngine.Random.value);
		}

	}

	public override string ToString(){
	return string.Format("Get value from {0}", "Random");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Random.value");
	}

}