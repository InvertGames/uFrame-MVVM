using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;


[UBCategory("GameObject")]
public class GetGameObjectGameObject : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	[UBRequireVariable] [UBRequired] public UBGameObject _Result = new UBGameObject();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _GameObject.GetValue(context).gameObject);
		}

	}

	public override string ToString(){
	return string.Format("Get gameObject from {0}", _GameObject.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_GameObject#.gameObject");
	}

}