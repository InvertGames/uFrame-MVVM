using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The status of the peer type, i.e. if it is disconnected, connecting, server or client.")]
[UBCategory("Network")]
public class GetNetworkPeerType : UBAction {

	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(NetworkPeerType));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.peerType);
		}

	}

	public override string ToString(){
	return string.Format("Get peerType from {0}", "Network");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Network.peerType");
	}

}