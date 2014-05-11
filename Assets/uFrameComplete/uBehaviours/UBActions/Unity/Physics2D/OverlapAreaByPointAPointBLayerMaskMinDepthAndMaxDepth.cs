using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBCategory("Physics2D")]
public class OverlapAreaByPointAPointBLayerMaskMinDepthAndMaxDepth : UBAction {

	[UBRequired] public UBVector2 _PointA = new UBVector2();
	[UBRequired] public UBVector2 _PointB = new UBVector2();
	[UBRequired] public UBInt _LayerMask = new UBInt();
	[UBRequired] public UBFloat _MinDepth = new UBFloat();
	[UBRequired] public UBFloat _MaxDepth = new UBFloat();
	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject(typeof(Collider2D));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Physics2D.OverlapArea(_PointA.GetValue(context),_PointB.GetValue(context),_LayerMask.GetValue(context),_MinDepth.GetValue(context),_MaxDepth.GetValue(context)));
		}

	}

	public override string ToString(){
	return string.Format("Call {0}'s OverlapArea w/ {1}, {2}, {3}, {4} and {5}", "Physics2D", _PointA.ToString(RootContainer), _PointB.ToString(RootContainer), _LayerMask.ToString(RootContainer), _MinDepth.ToString(RootContainer), _MaxDepth.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("Physics2D.OverlapArea(#_PointA#, #_PointB#, #_LayerMask#, #_MinDepth#, #_MaxDepth#)");
	}

}