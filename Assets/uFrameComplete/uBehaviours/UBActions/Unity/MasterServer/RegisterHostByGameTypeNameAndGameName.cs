using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Register this server on the master server.")]
[UBCategory("MasterServer")]
public class RegisterHostBySceneManagerNameAndGameName : UBAction {

	
	[UBRequired] public UBString _SceneManagerName = new UBString();
	
	[UBRequired] public UBString _GameName = new UBString();
	protected override void PerformExecute(IUBContext context){
		MasterServer.RegisterHost(_SceneManagerName.GetValue(context),_GameName.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s RegisterHost w/ {1} and {2}", "MasterServer", _SceneManagerName.ToString(RootContainer), _GameName.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("MasterServer.RegisterHost(#_SceneManagerName#, #_GameName#)");
	}

}