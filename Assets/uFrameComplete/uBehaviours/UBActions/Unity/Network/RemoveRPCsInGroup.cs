using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Remove all  functions which belong to given group number.")]
[UBCategory("Network")]
public class RemoveRPCsInGroup : UBAction {

	
	[UBRequired] public UBInt _Group = new UBInt();
	protected override void PerformExecute(IUBContext context){
		Network.RemoveRPCsInGroup(_Group.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s RemoveRPCsInGroup w/ {1}", "Network", _Group.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.RemoveRPCsInGroup(#_Group#)");
	}

}