using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The tag of this game object.")]
[UBCategory("GameObject")]
public class SetGameObjectTag : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	[UBRequired] public UBString _Value = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_GameObject.GetValue(context).tag = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s tag to {1}", _GameObject.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GameObject#.tag = #_Value# ");
	}

}