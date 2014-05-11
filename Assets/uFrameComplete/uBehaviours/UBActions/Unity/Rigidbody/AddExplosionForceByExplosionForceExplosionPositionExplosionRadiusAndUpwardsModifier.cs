using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Applies a force to the rigidbody that simulates explosion effects. The explosion force will fall off linearly with distance to the rigidbody.")]
[UBCategory("Rigidbody")]
public class AddExplosionForceByExplosionForceExplosionPositionExplosionRadiusAndUpwardsModifier : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	
	[UBRequired] public UBFloat _ExplosionForce = new UBFloat();
	
	[UBRequired] public UBVector3 _ExplosionPosition = new UBVector3();
	
	[UBRequired] public UBFloat _ExplosionRadius = new UBFloat();
	
	[UBRequired] public UBFloat _UpwardsModifier = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		_Rigidbody.GetValueAs<Rigidbody>(context).AddExplosionForce(_ExplosionForce.GetValue(context),_ExplosionPosition.GetValue(context),_ExplosionRadius.GetValue(context),_UpwardsModifier.GetValue(context));
	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.AddExplosionForce(#_ExplosionForce#, #_ExplosionPosition#, #_ExplosionRadius#, #_UpwardsModifier#)");
	}

}