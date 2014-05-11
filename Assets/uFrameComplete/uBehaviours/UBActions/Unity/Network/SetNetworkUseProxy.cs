using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Indicate if proxy support is needed, in which case traffic is relayed through the proxy server.")]
[UBCategory("Network")]
public class SetNetworkUseProxy : UBAction {

	[UBRequired] public UBBool _Value = new UBBool();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Network.useProxy = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s useProxy to {1}", "Network", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.useProxy = #_Value# ");
	}

}