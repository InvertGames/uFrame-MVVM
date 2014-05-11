using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Connect to the specified host (ip or domain name) and server port.")]
[UBCategory("Network")]
public class ConnectByIPAndRemotePort : UBAction {

	
	[UBRequired] public UBString _IP = new UBString();
	
	[UBRequired] public UBInt _RemotePort = new UBInt();
	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(NetworkConnectionError));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.Connect(_IP.GetValue(context),_RemotePort.GetValue(context)));
		}
	}

	public override string ToString(){
	    return string.Format("Connect to {0}:{1}", _IP.ToString(RootContainer), _RemotePort.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.Connect(#_IP#, #_RemotePort#)");
	}

}