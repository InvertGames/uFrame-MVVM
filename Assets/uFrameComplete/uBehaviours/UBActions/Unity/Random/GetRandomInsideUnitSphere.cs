using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns a random point inside a sphere with radius 1 (Read Only).")]
[UBCategory("Random")]
public class GetRandomInsideUnitSphere : UBAction {

	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
            _Result.SetValue(context, UnityEngine.Random.insideUnitSphere);
		}

	}

	public override string ToString(){
	return string.Format("Get insideUnitSphere from {0}", "Random");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Random.insideUnitSphere");
	}

}