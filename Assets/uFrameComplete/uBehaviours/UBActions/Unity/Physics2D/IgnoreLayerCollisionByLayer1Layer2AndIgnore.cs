using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Physics2D")]
public class IgnoreLayerCollision2DByLayer1Layer2AndIgnore : UBAction {

	[UBRequired] public UBInt _Layer1 = new UBInt();
	[UBRequired] public UBInt _Layer2 = new UBInt();
	[UBRequired] public UBBool _Ignore = new UBBool();
	protected override void PerformExecute(IUBContext context){
		Physics2D.IgnoreLayerCollision(_Layer1.GetValue(context),_Layer2.GetValue(context),_Ignore.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s IgnoreLayerCollision w/ {1}, {2} and {3}", "Physics2D", _Layer1.ToString(RootContainer), _Layer2.ToString(RootContainer), _Ignore.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics2D.IgnoreLayerCollision(#_Layer1#, #_Layer2#, #_Ignore#)");
	}

}