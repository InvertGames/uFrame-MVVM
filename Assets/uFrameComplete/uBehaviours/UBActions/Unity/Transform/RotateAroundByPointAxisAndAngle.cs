using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Rotates the transform about axis passsing through point in world coordinates by angle degrees.")]
[UBCategory("Transform")]
public class RotateAroundByPointAxisAndAngle : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	
	[UBRequired] public UBVector3 _Point = new UBVector3();
	
	[UBRequired] public UBVector3 _Axis = new UBVector3();
	
	[UBRequired] public UBFloat _Angle = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		_Transform.GetValue(context).RotateAround(_Point.GetValue(context),_Axis.GetValue(context),_Angle.GetValue(context));
	}

	

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.RotateAround(#_Point#, #_Axis#, #_Angle#)");
	}

}