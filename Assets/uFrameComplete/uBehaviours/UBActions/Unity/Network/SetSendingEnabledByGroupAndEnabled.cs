using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Enables or disables transmission of messages and  calls on a specific network group number.")]
[UBCategory("Network")]
public class SetSendingEnabledByGroupAndEnabled : UBAction {

	
	[UBRequired] public UBInt _Group = new UBInt();
	
	[UBRequired] public UBBool _Enabled = new UBBool();
	protected override void PerformExecute(IUBContext context){
		Network.SetSendingEnabled(_Group.GetValue(context),_Enabled.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s SetSendingEnabled w/ {1} and {2}", "Network", _Group.ToString(RootContainer), _Enabled.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.SetSendingEnabled(#_Group#, #_Enabled#)");
	}

}