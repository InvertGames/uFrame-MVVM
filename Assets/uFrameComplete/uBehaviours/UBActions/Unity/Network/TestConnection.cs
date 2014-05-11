using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Test this machines network connection.")]
[UBCategory("Network")]
public class TestConnection : UBAction {

	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(ConnectionTesterStatus));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.TestConnection());
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s TestConnection w/ ", "Network");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.TestConnection()");
	}

}