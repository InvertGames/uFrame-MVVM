using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Get or set the minimum number of ViewID numbers in the ViewID pool given to clients by the server.")]
[UBCategory("Network")]
public class GetNetworkMinimumAllocatableViewIDs : UBAction {

	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.minimumAllocatableViewIDs);
		}

	}

	public override string ToString(){
	return string.Format("Get minimumAllocatableViewIDs from {0}", "Network");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Network.minimumAllocatableViewIDs");
	}

}