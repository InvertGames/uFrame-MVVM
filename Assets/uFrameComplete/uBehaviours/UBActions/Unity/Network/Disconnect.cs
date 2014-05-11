using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Close all open connections and shuts down the network interface.")]
[UBCategory("Network")]
public class Disconnect : UBAction {

	protected override void PerformExecute(IUBContext context){
		Network.Disconnect();
	}

	public override string ToString(){
	    return string.Format("Disconnect");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.Disconnect()");
	}

}