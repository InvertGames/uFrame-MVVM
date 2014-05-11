using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Vector2")]
public class Vector2Angle : UBAction
{

	[UBRequired] public UBVector2 _From = new UBVector2();
	[UBRequired] public UBVector2 _To = new UBVector2();
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector2.Angle(_From.GetValue(context),_To.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s Angle w/ {1} and {2}", "Vector2", _From.ToString(RootContainer), _To.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector2.Angle(#_From#, #_To#)");
	}

}