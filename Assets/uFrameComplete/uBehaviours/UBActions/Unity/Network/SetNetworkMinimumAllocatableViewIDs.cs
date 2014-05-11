using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Get or set the minimum number of ViewID numbers in the ViewID pool given to clients by the server.")]
[UBCategory("Network")]
public class SetNetworkMinimumAllocatableViewIDs : UBAction {

	[UBRequired] public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Network.minimumAllocatableViewIDs = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s minimumAllocatableViewIDs to {1}", "Network", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.minimumAllocatableViewIDs = #_Value# ");
	}

}