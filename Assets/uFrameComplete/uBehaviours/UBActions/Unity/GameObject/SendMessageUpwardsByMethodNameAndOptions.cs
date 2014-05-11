using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Calls the method named methodName on every  in this game object and on every ancestor of the behaviour")]
[UBCategory("GameObject")]
public class SendMessageUpwardsByMethodNameAndOptions : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	
	[UBRequired] public UBString _MethodName = new UBString();
	
	[UBRequired] public UBEnum _Options = new UBEnum(typeof(SendMessageOptions));
	protected override void PerformExecute(IUBContext context){
		_GameObject.GetValue(context).SendMessageUpwards(_MethodName.GetValue(context),((SendMessageOptions)_Options.GetIntValue(context)));
	}

	public override string ToString(){
	return string.Format("Send {0} Message Upwards w/ {1}", _MethodName.ToString(RootContainer), _Options.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GameObject#.SendMessageUpwards(#_MethodName#, #_Options#)");
	}

}