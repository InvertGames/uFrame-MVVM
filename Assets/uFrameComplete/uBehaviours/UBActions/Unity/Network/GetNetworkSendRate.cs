using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The default send rate of network updates for all Network Views.")]
[UBCategory("Network")]
public class GetNetworkSendRate : UBAction {

	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.sendRate);
		}

	}

	public override string ToString(){
	return string.Format("Get sendRate from {0}", "Network");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Network.sendRate");
	}

}