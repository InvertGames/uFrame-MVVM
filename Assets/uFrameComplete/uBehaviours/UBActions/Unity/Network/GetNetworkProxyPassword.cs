using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Set the proxy server password.")]
[UBCategory("Network")]
public class GetNetworkProxyPassword : UBAction {

	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.proxyPassword);
		}

	}

	public override string ToString(){
	return string.Format("Get proxyPassword from {0}", "Network");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Network.proxyPassword");
	}

}