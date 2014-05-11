using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns a random point on the surface of a sphere with radius 1 (Read Only).")]
[UBCategory("Random")]
public class GetRandomOnUnitSphere : UBAction {

	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
            _Result.SetValue(context, UnityEngine.Random.onUnitSphere);
		}

	}

	public override string ToString(){
	return string.Format("Get onUnitSphere from {0}", "Random");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Random.onUnitSphere");
	}

}