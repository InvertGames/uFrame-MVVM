using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Interpolation allows you to smooth out the effect of running physics at a fixed frame rate.")]
[UBCategory("Rigidbody")]
public class GetRigidbodyInterpolation : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(RigidbodyInterpolation));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rigidbody.GetValueAs<Rigidbody>(context).interpolation);
		}

	}

	public override string ToString(){
	return string.Format("Get interpolation from {0}", _Rigidbody.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Rigidbody#.interpolation");
	}

}