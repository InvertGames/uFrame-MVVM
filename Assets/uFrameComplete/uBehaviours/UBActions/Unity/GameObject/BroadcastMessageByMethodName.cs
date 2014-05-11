using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Calls the method named methodName on every  in this game object or any of its children.")]
[UBCategory("GameObject")]
public class BroadcastMessageByMethodName : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	
	[UBRequired] public UBString _MethodName = new UBString();
	protected override void PerformExecute(IUBContext context){
		_GameObject.GetValue(context).BroadcastMessage(_MethodName.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Broadcast {0} Message", _GameObject.ToString(RootContainer), _MethodName.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GameObject#.BroadcastMessage(#_MethodName#)");
	}

}