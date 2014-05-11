using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The IP address of the proxy server.")]
[UBCategory("Network")]
public class SetNetworkProxyIP : UBAction {

	[UBRequired] public UBString _Value = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Network.proxyIP = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s proxyIP to {1}", "Network", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.proxyIP = #_Value# ");
	}

}