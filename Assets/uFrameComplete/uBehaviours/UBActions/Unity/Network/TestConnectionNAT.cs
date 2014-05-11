using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Test the connecction specifically for NAT punchthrough connectivity.")]
[UBCategory("Network")]
public class TestConnectionNAT : UBAction {

	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(ConnectionTesterStatus));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.TestConnectionNAT());
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s TestConnectionNAT w/ ", "Network");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.TestConnectionNAT()");
	}

}