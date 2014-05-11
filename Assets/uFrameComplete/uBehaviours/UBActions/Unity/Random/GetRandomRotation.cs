using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns a random rotation (Read Only).")]
[UBCategory("Random")]
public class GetRandomRotation : UBAction {

	[UBRequireVariable] [UBRequired] public UBQuaternion _Result = new UBQuaternion();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
            _Result.SetValue(context, UnityEngine.Random.rotation);
		}

	}

	public override string ToString(){
	return string.Format("Get rotation from {0}", "Random");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Random.rotation");
	}

}