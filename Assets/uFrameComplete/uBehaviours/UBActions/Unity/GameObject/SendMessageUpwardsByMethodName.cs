using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Calls the method named methodName on every  in this game object and on every ancestor of the behaviour")]
[UBCategory("GameObject")]
public class SendMessageUpwardsByMethodName : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	
	[UBRequired] public UBString _MethodName = new UBString();
	protected override void PerformExecute(IUBContext context){
		_GameObject.GetValue(context).SendMessageUpwards(_MethodName.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Send Message {0} Upwards", _MethodName.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GameObject#.SendMessageUpwards(#_MethodName#)");
	}

}