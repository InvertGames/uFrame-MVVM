using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Returns a random point inside a circle with radius 1 (Read Only).")]
[UBCategory("Random")]
public class GetRandomInsideUnitCircle : UBAction {

	[UBRequireVariable] [UBRequired] public UBVector2 _Result = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
            _Result.SetValue(context, UnityEngine.Random.insideUnitCircle);
		}

	}

	public override string ToString(){
	return string.Format("Get insideUnitCircle from {0}", "Random");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Random.insideUnitCircle");
	}

}