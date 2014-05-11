using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Report this machine as a dedicated server.")]
[UBCategory("MasterServer")]
public class GetMasterServerDedicatedServer : UBConditionAction {

	public override bool PerformCondition(IUBContext context){
return MasterServer.dedicatedServer		;

	}

	public override string WriteExpressionCode(IUBCSharpGenerator sb){
		return sb.Expression("MasterServer.dedicatedServer");
	}

}