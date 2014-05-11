using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Close all open connections and shuts down the network interface.")]
[UBCategory("Network")]
public class DisconnectByTimeout : UBAction {

	
	[UBRequired] public UBInt _Timeout = new UBInt();
	protected override void PerformExecute(IUBContext context){
		Network.Disconnect(_Timeout.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s Disconnect w/ {1}", "Network", _Timeout.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.Disconnect(#_Timeout#)");
	}

}