using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Set the minimum update rate for master server host information update.")]
[UBCategory("MasterServer")]
public class GetMasterServerUpdateRate : UBAction {

	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, MasterServer.updateRate);
		}

	}

	public override string ToString(){
	return string.Format("Get updateRate from {0}", "MasterServer");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = MasterServer.updateRate");
	}

}