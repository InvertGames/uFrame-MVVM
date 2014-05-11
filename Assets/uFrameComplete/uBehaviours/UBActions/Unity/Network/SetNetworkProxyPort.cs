using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The port of the proxy server.")]
[UBCategory("Network")]
public class SetNetworkProxyPort : UBAction {

	[UBRequired] public UBInt _Value = new UBInt();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Network.proxyPort = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s proxyPort to {1}", "Network", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.proxyPort = #_Value# ");
	}

}