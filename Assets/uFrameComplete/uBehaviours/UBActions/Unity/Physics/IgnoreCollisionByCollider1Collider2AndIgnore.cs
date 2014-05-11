using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Makes the collision detection system ignore all collisions between collider1 and collider2.")]
[UBCategory("Physics")]
public class IgnoreCollisionByCollider1Collider2AndIgnore : UBAction {

	
	[UBRequired] public UBObject _Collider1 = new UBObject(typeof(Collider));
	
	[UBRequired] public UBObject _Collider2 = new UBObject(typeof(Collider));
	
	[UBRequired] public UBBool _Ignore = new UBBool();
	protected override void PerformExecute(IUBContext context){
		Physics.IgnoreCollision(_Collider1.GetValueAs<Collider>(context),_Collider2.GetValueAs<Collider>(context),_Ignore.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s IgnoreCollision w/ {1}, {2} and {3}", "Physics", _Collider1.ToString(RootContainer), _Collider2.ToString(RootContainer), _Ignore.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics.IgnoreCollision(#_Collider1#, #_Collider2#, #_Ignore#)");
	}

}