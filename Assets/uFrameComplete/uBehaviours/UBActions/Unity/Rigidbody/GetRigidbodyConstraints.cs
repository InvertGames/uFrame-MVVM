using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Rigidbody")]
public class GetRigidbodyConstraints : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(RigidbodyConstraints));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rigidbody.GetValueAs<Rigidbody>(context).constraints);
		}

	}

	public override string ToString(){
	return string.Format("Get constraints from {0}", _Rigidbody.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Rigidbody#.constraints");
	}

}