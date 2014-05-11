using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Physics2D")]
public class GetPhysics2DGravity : UBAction {

	[UBRequireVariable] [UBRequired] public UBVector2 _Result = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Physics2D.gravity);
		}

	}

	public override string ToString(){
	return string.Format("Get gravity from {0}", "Physics2D");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Physics2D.gravity");
	}

}