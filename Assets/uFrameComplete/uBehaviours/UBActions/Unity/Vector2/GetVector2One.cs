using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Vector2")]
public class GetVector2One : UBAction {

	[UBRequireVariable] [UBRequired] public UBVector2 _Result = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector2.one);
		}

	}

	public override string ToString(){
	return string.Format("Get one from {0}", "Vector2");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Vector2.one");
	}

}