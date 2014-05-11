using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Network")]
public class SetNetworkLogLevel : UBAction {

	[UBRequired] public UBEnum _Value = new UBEnum(typeof(NetworkLogLevel));
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			Network.logLevel = ((NetworkLogLevel)_Value.GetIntValue(context));
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s logLevel to {1}", "Network", _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.logLevel = #_Value# ");
	}

}