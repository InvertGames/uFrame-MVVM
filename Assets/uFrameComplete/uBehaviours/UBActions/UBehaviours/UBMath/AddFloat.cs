using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("UBMath")]
public class AddFloat : UBAction {

	[UBRequired] public UBFloat _A = new UBFloat();
	[UBRequired] public UBFloat _B = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, UBMath.AddFloat(_A.GetValue(context),_B.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("{0} = {1} + {2}",
        _A.ToString(RootContainer), _B.ToString(RootContainer), _Result.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("UBMath.AddFloat(#_A#, #_B#)");
	}

}