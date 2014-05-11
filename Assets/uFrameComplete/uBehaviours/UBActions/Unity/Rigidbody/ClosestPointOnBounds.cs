using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The closest point to the bounding box of the attached colliders.")]
[UBCategory("Rigidbody")]
public class ClosestPointOnBounds : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	
	[UBRequired] public UBVector3 _Position = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rigidbody.GetValueAs<Rigidbody>(context).ClosestPointOnBounds(_Position.GetValue(context)));
		}

	}

	public override string ToString(){
	return "Get ClosestPointOnBounds";
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.ClosestPointOnBounds(#_Position#)");
	}

}