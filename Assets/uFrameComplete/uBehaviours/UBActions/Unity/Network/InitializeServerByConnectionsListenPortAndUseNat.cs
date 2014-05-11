using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Network")]
public class InitializeServerByConnectionsListenPortAndUseNat : UBAction {

	[UBRequired] public UBInt _Connections = new UBInt();
	[UBRequired] public UBInt _ListenPort = new UBInt();
	[UBRequired] public UBBool _UseNat = new UBBool();
	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(NetworkConnectionError));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.InitializeServer(_Connections.GetValue(context),_ListenPort.GetValue(context),_UseNat.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s InitializeServer w/ {1}, {2} and {3}", "Network", _Connections.ToString(RootContainer), _ListenPort.ToString(RootContainer), _UseNat.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.InitializeServer(#_Connections#, #_ListenPort#, #_UseNat#)");
	}

}