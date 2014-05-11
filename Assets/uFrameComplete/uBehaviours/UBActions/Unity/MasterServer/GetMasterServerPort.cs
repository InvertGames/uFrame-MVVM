using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The connection port of the master server.")]
[UBCategory("MasterServer")]
public class GetMasterServerPort : UBAction {

	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, MasterServer.port);
		}

	}

	public override string ToString(){
	return string.Format("Get port from {0}", "MasterServer");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = MasterServer.port");
	}

}