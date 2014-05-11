using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Random")]
public class GetRandomRotationUniform : UBAction {

	[UBRequireVariable] [UBRequired] public UBQuaternion _Result = new UBQuaternion();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
            _Result.SetValue(context, UnityEngine.Random.rotationUniform);
		}

	}

	public override string ToString(){
	return string.Format("Get rotationUniform from {0}", "Random");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Random.rotationUniform");
	}

}