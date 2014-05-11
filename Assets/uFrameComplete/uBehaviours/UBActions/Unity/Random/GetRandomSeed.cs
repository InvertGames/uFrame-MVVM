using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Sets the seed for the random number generator.")]
[UBCategory("Random")]
public class GetRandomSeed : UBAction {

	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
            _Result.SetValue(context, UnityEngine.Random.seed);
		}

	}

	public override string ToString(){
	return string.Format("Get seed from {0}", "Random");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Random.seed");
	}

}