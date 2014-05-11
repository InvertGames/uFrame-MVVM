using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("GameObject")]
public class SetGameObjectIsStatic : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	[UBRequired] public UBBool _Value = new UBBool();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_GameObject.GetValue(context).isStatic = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s isStatic to {1}", _GameObject.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GameObject#.isStatic = #_Value# ");
	}

}