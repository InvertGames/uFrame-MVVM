using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The drag of the object.")]
[UBCategory("Rigidbody")]
public class GetRigidbodyDrag : UBAction {

	[UBRequired] public UBObject _Rigidbody = new UBObject(typeof(Rigidbody));
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Rigidbody.GetValueAs<Rigidbody>(context).drag);
		}

	}

	public override string ToString(){
	return string.Format("Get drag from {0}", _Rigidbody.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Rigidbody#.drag");
	}

}