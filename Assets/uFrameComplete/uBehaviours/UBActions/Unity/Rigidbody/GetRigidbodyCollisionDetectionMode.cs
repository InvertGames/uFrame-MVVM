using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Rigidbody")]
public class GetRigidbodyCollisionDetectionMode : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(CollisionDetectionMode));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rigidbody.GetValueAs<Rigidbody>(context).collisionDetectionMode);
		}

	}

	public override string ToString(){
	return string.Format("Get collisionDetectionMode from {0}", _Rigidbody.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Rigidbody#.collisionDetectionMode");
	}

}