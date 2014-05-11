using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The layer the game object is in. A layer is in the range [0...32]")]
[UBCategory("GameObject")]
public class SetGameObjectLayer : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	[UBRequired] public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_GameObject.GetValue(context).layer = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s layer to {1}", _GameObject.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GameObject#.layer = #_Value# ");
	}

}