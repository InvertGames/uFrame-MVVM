using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Clear the host list which was received by .")]
[UBCategory("MasterServer")]
public class ClearHostList : UBAction {

	protected override void PerformExecute(IUBContext context){
		MasterServer.ClearHostList();
	}

	public override string ToString(){
        return string.Format("Clear {0}'s Host List", "MasterServer");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("MasterServer.ClearHostList()");
	}

}