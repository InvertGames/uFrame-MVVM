using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("GameObject")]
public class GetGameObjectRigidbody2D : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject(typeof(Rigidbody2D));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _GameObject.GetValue(context).rigidbody2D);
		}

	}

	public override string ToString(){
	return string.Format("Get rigidbody2D from {0}", _GameObject.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_GameObject#.rigidbody2D");
	}

}