using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Physics2D")]
public class OverlapPointByPoint : UBAction {

	[UBRequired] public UBVector2 _Point = new UBVector2();
	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject(typeof(Collider2D));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Physics2D.OverlapPoint(_Point.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s OverlapPoint w/ {1}", "Physics2D", _Point.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics2D.OverlapPoint(#_Point#)");
	}

}