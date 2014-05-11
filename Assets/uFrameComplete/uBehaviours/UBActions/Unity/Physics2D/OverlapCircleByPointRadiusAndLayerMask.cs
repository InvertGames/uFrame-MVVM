using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Physics2D")]
public class OverlapCircleByPointRadiusAndLayerMask : UBAction {

	[UBRequired] public UBVector2 _Point = new UBVector2();
	[UBRequired] public UBFloat _Radius = new UBFloat();
	[UBRequired] public UBInt _LayerMask = new UBInt();
	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject(typeof(Collider2D));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Physics2D.OverlapCircle(_Point.GetValue(context),_Radius.GetValue(context),_LayerMask.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s OverlapCircle w/ {1}, {2} and {3}", "Physics2D", _Point.ToString(RootContainer), _Radius.ToString(RootContainer), _LayerMask.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics2D.OverlapCircle(#_Point#, #_Radius#, #_LayerMask#)");
	}

}