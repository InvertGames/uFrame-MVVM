using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("UBRandom")]
public class RandomVector3 : UBAction {

	[UBRequired] public UBVector3 _Min = new UBVector3();
	[UBRequired] public UBVector3 _Max = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, UBRandom.RandomVector3(_Min.GetValue(context),_Max.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s RandomVector3 w/ {1} and {2}", "UBRandom", _Min.ToString(RootContainer), _Max.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("UBRandom.RandomVector3(#_Min#, #_Max#)");
	}

}