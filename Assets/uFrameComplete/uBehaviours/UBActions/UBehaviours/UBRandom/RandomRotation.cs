using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("UBRandom")]
public class RandomRotation : UBAction {

	[UBRequired] public UBVector3 _Min = new UBVector3();
	[UBRequired] public UBVector3 _Max = new UBVector3();
	[UBRequireVariable] [UBRequired] public UBQuaternion _Result = new UBQuaternion();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, UBRandom.RandomRotation(_Min.GetValue(context),_Max.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s RandomRotation w/ {1} and {2}", "UBRandom", _Min.ToString(RootContainer), _Max.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("UBRandom.RandomRotation(#_Min#, #_Max#)");
	}

}