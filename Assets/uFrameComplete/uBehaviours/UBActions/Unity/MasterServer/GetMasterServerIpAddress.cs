using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The IP address of the master server.")]
[UBCategory("MasterServer")]
public class GetMasterServerIpAddress : UBAction {

	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, MasterServer.ipAddress);
		}

	}

	public override string ToString(){
	return string.Format("Get ipAddress from {0}", "MasterServer");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = MasterServer.ipAddress");
	}

}