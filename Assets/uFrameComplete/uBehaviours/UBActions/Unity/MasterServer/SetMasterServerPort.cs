using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The connection port of the master server.")]
[UBCategory("MasterServer")]
public class SetMasterServerPort : UBAction {

	[UBRequired] public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			MasterServer.port = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s port to {1}", "MasterServer", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("MasterServer.port = #_Value# ");
	}

}