using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Network")]
public class GetNetworkLogLevel : UBAction {

	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(NetworkLogLevel));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.logLevel);
		}

	}

	public override string ToString(){
	return string.Format("Get logLevel from {0}", "Network");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Network.logLevel");
	}

}