using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The rotation as Euler angles in degrees.")]
[UBCategory("Transform")]
public class SetTransformEulerAngles : UBAction {

	[UBRequired] public UBTransform _Transform = new UBTransform();
	[UBRequired] public UBVector3 _Value = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Transform.GetValue(context).eulerAngles = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s eulerAngles to {1}", _Transform.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Transform#.eulerAngles = #_Value# ");
	}

}