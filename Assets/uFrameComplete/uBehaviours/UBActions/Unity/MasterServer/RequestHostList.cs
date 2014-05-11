using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Request a host list from the master server.")]
[UBCategory("MasterServer")]
public class RequestHostList : UBAction {

	
	[UBRequired] public UBString _SceneManagerName = new UBString();
	protected override void PerformExecute(IUBContext context){
		MasterServer.RequestHostList(_SceneManagerName.GetValue(context));
	}

	public override string ToString(){
        return string.Format("Request {0}'s HostList", "MasterServer");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("MasterServer.RequestHostList(#_SceneManagerName#)");
	}

}