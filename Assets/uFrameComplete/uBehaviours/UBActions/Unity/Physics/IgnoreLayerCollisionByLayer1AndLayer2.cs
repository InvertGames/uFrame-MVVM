using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Physics")]
public class IgnoreLayerCollisionByLayer1AndLayer2 : UBAction {

	[UBRequired] public UBInt _Layer1 = new UBInt();
	[UBRequired] public UBInt _Layer2 = new UBInt();
	protected override void PerformExecute(IUBContext context){
		Physics.IgnoreLayerCollision(_Layer1.GetValue(context),_Layer2.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s IgnoreLayerCollision w/ {1} and {2}", "Physics", _Layer1.ToString(RootContainer), _Layer2.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics.IgnoreLayerCollision(#_Layer1#, #_Layer2#)");
	}

}