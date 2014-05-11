using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Calls the method named methodName on every  in this game object or any of its children.")]
[UBCategory("GameObject")]
public class BroadcastMessageByMethodNameAndOptions : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	
	[UBRequired] public UBString _MethodName = new UBString();
	
	[UBRequired] public UBEnum _Options = new UBEnum(typeof(SendMessageOptions));
	protected override void PerformExecute(IUBContext context){
		_GameObject.GetValue(context).BroadcastMessage(_MethodName.GetValue(context),((SendMessageOptions)_Options.GetIntValue(context)));
	}

	public override string ToString(){
	return string.Format("Broadcast {0} Message w/ {1}",
        _MethodName.ToString(RootContainer), _Options.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GameObject#.BroadcastMessage(#_MethodName#, #_Options#)");
	}

}