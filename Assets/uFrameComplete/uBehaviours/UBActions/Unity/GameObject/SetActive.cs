using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("GameObject")]
public class SetActive : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	[UBRequired] public UBBool _Value = new UBBool();
	protected override void PerformExecute(IUBContext context){
		_GameObject.GetValue(context).SetActive(_Value.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s Active to {1}", _GameObject.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GameObject#.SetActive(#_Value#)");
	}

}