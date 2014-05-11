using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Vector2")]
public class Vector2MoveTowards : UBAction
{

	[UBRequired] public UBVector2 _Current = new UBVector2();
	[UBRequired] public UBVector2 _Target = new UBVector2();
	[UBRequired] public UBFloat _MaxDistanceDelta = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBVector2 _Result = new UBVector2();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Vector2.MoveTowards(_Current.GetValue(context),_Target.GetValue(context),_MaxDistanceDelta.GetValue(context)));
		}

	}

	public override string ToString(){
	    return string.Format("Move {0} Towards {1}", _Current.ToString(RootContainer), _Target.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Vector2.MoveTowards(#_Current#, #_Target#, #_MaxDistanceDelta#)");
	}

}