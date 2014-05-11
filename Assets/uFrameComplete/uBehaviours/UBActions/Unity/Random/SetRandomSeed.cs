using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Sets the seed for the random number generator.")]
[UBCategory("Random")]
public class SetRandomSeed : UBAction {

	[UBRequired] public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
            UnityEngine.Random.seed = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s seed to {1}", "Random", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Random.seed = #_Value# ");
	}

}