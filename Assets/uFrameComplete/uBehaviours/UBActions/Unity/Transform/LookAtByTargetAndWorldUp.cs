using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Rotates the transform so the forward vector points at /target/'s current position.")]
[UBCategory("Transform")]
public class LookAtByTargetAndWorldUp : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	
	[UBRequired] public UBTransform _Target = new UBTransform();
	
	[UBRequired] public UBVector3 _WorldUp = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		_Transform.GetValue(context).LookAt(_Target.GetValue(context),_WorldUp.GetValue(context));
	}

    public override string ToString()
    {
        return string.Format("Make {0} Look At w/ {1}", _Transform.ToString(RootContainer), _Target.ToString(RootContainer));
    }


	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.LookAt(#_Target#, #_WorldUp#)");
	}

}