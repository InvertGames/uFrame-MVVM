using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The port of the proxy server.")]
[UBCategory("Network")]
public class GetNetworkProxyPort : UBAction {

	[UBRequireVariable] [UBRequired] public UBInt _Result = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.proxyPort);
		}

	}

	public override string ToString(){
	return string.Format("Get proxyPort from {0}", "Network");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Network.proxyPort");
	}

}