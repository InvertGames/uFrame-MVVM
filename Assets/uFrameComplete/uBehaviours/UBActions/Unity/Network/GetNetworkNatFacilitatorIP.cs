using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The IP address of the NAT punchthrough facilitator.")]
[UBCategory("Network")]
public class GetNetworkNatFacilitatorIP : UBAction {

	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Network.natFacilitatorIP);
		}

	}

	public override string ToString(){
	return string.Format("Get natFacilitatorIP from {0}", "Network");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Network.natFacilitatorIP");
	}

}