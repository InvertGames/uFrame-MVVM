using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The velocity relative to the rigidbody at the point relativePoint.")]
[UBCategory("Rigidbody")]
public class GetRelativePointVelocity : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	
	[UBRequired] public UBVector3 _RelativePoint = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rigidbody.GetValueAs<Rigidbody>(context).GetRelativePointVelocity(_RelativePoint.GetValue(context)));
		}

	}


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Rigidbody#.GetRelativePointVelocity(#_RelativePoint#)");
	}

}