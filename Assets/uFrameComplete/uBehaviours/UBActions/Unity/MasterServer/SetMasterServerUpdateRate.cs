using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Set the minimum update rate for master server host information update.")]
[UBCategory("MasterServer")]
public class SetMasterServerUpdateRate : UBAction {

	[UBRequired] public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			MasterServer.updateRate = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s updateRate to {1}", "MasterServer", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("MasterServer.updateRate = #_Value# ");
	}

}