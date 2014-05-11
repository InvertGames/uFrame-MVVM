using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Set the maximum amount of connections/players allowed.")]
[UBCategory("Network")]
public class GetNetworkMaxConnections : UBAction {

	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.maxConnections);
		}

	}

	public override string ToString(){
	return string.Format("Get maxConnections from {0}", "Network");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Network.maxConnections");
	}

}