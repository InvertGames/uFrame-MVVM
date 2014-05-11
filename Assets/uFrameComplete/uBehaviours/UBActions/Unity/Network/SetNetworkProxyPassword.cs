using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Set the proxy server password.")]
[UBCategory("Network")]
public class SetNetworkProxyPassword : UBAction {

	[UBRequired] public UBString _Value = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Network.proxyPassword = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s proxyPassword to {1}", "Network", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.proxyPassword = #_Value# ");
	}

}