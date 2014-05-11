using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Calls the method named methodName on every  in this game object.")]
[UBCategory("GameObject")]
public class SendMessageByMethodName : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	
	[UBRequired] public UBString _MethodName = new UBString();
	protected override void PerformExecute(IUBContext context){
		_GameObject.GetValue(context).SendMessage(_MethodName.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Send {1} Message to {0}", _GameObject.ToString(RootContainer), _MethodName.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GameObject#.SendMessage(#_MethodName#)");
	}

}