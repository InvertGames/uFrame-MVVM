using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Initializes security layer.")]
[UBCategory("Network")]
public class InitializeSecurity : UBAction {

	protected override void PerformExecute(IUBContext context){
		Network.InitializeSecurity();
	}

	public override string ToString(){
	return string.Format("Call {0}'s InitializeSecurity w/ ", "Network");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.InitializeSecurity()");
	}

}