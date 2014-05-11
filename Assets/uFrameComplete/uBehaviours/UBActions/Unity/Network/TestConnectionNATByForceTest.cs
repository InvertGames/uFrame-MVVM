using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Network")]
public class TestConnectionNATByForceTest : UBAction {

	[UBRequired] public UBBool _ForceTest = new UBBool();
	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(ConnectionTesterStatus));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.TestConnectionNAT(_ForceTest.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s TestConnectionNAT w/ {1}", "Network", _ForceTest.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Network.TestConnectionNAT(#_ForceTest#)");
	}

}